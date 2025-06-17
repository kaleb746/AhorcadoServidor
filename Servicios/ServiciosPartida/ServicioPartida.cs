using Modelo;
using Servicios.Dtos;
using Servicios.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace Servicios.ServiciosPartida
{
    public class ServicioPartida : IPartidaManager
    {
        public int CrearPartida(int idJugador, int idPalabra, string idioma)
        {
            var dao = new PartidaDAO();
            int idPartida = dao.CrearPartida(idJugador, idPalabra, idioma);

            if (idPartida > 0)
            {
                var callback = OperationContext.Current.GetCallbackChannel<IPartidaCallback>();
                CallbackManager.RegistrarCallback(idJugador, callback);
            }

            return idPartida;
        }
        public List<PartidaDisponibleDTO> ObtenerPartidasDisponibles(int idJugadorActual)
        {
            var dao = new PartidaDAO();
            return dao.ObtenerPartidasDisponibles(idJugadorActual);
        }
        public List<HistorialPartidaDTO> ObtenerHistorialDeJugador(int idJugador)
        {
            var dao = new PartidaDAO();
            return dao.ObtenerHistorialDeJugador(idJugador);
        }
        public bool UnirseAPartida(int idPartida, int idJugador, string usernameInvitado)
        {
            var dao = new PartidaDAO();
            bool registrado = dao.RegistrarInvitadoEnPartida(idPartida, idJugador);
            if (!registrado) return false;

            var callback = OperationContext.Current.GetCallbackChannel<IPartidaCallback>();
            CallbackManager.RegistrarCallback(idJugador, callback);

            using (var context = new JuegoAhorcadoEntities())
            {
                var anfitrion = context.JugadoresPartidas
                    .FirstOrDefault(jp => jp.IdPartida == idPartida && jp.Rol == "Anfitrion");

                if (anfitrion != null)
                {
                    bool notificado = CallbackManager.IntentarNotificar(anfitrion.IdJugador, usernameInvitado);
                    if (!notificado)
                    {
                        Console.WriteLine($"[Callback] No se pudo notificar al anfitrión con ID {anfitrion.IdJugador}");
                    }
                }

                bool notificadoInvitado = CallbackManager.IntentarNotificar(idJugador, usernameInvitado);
                if (!notificadoInvitado)
                {
                    Console.WriteLine($"[Callback] No se pudo notificar al invitado con ID {idJugador}");
                }
            }

            return true;
        }
        public bool IntentarLetra(int idPartida, int idJugador, char letra)
        {
            var dao = new PartidaDAO();
            var (acierto, estadoActualPalabra, erroresActuales) = dao.IntentarLetra(idPartida, idJugador, letra);

            bool palabraCompletada = !estadoActualPalabra.Contains('_');
            const int MAX_ERRORES = 5;

            Task.Run(() =>
            {
                using (var context = new JuegoAhorcadoEntities())
                {
                    var jugadores = context.JugadoresPartidas
                        .Where(jp => jp.IdPartida == idPartida)
                        .ToList();

                    foreach (var jugador in jugadores)
                    {
                        CallbackManager.IntentarNotificarIntentoLetra(jugador.IdJugador, letra, acierto, estadoActualPalabra);
                    }

                    if (palabraCompletada || erroresActuales >= MAX_ERRORES)
                    {
                        var partida = context.Partidas.FirstOrDefault(p => p.Id == idPartida);
                        if (partida != null)
                            partida.IdEstadoPartida = 3;

                        foreach (var jugador in jugadores)
                        {
                            jugador.Ganador = false;
                        }

                        int idGanador = palabraCompletada
                            ? idJugador
                            : jugadores.FirstOrDefault(j => j.IdJugador != idJugador)?.IdJugador ?? 0;

                        var jugadorGanador = jugadores.FirstOrDefault(j => j.IdJugador == idGanador);
                        if (jugadorGanador != null)
                            jugadorGanador.Ganador = true;

                        var jugadorBD = context.Jugadores.FirstOrDefault(j => j.Id == idGanador);
                        if (jugadorBD != null)
                            jugadorBD.Puntaje += 10;

                        context.SaveChanges();

                        foreach (var jugador in jugadores)
                        {
                            bool esGanador = jugador.IdJugador == idGanador;
                            string idioma = partida.IdiomaPartida ?? "es";

                            string mensaje = esGanador
                                ? (idioma.StartsWith("en") ? "Congratulations! You won the game." : "¡Felicidades! Has ganado la partida.")
                                : erroresActuales >= MAX_ERRORES
                                    ? (idioma.StartsWith("en") ? "You ran out of attempts. You lost." : "Te quedaste sin intentos. Has perdido.")
                                    : (idioma.StartsWith("en") ? "You lost. The other player guessed the word." : "Has perdido. El otro jugador adivinó la palabra.");


                            CallbackManager.IntentarNotificarFinPartida(jugador.IdJugador, esGanador, mensaje);
                        }
                    }
                }
            });
            return acierto;
        }
        public string ObtenerEstadoPalabra(int idPartida)
        {
            var dao = new PartidaDAO();
            return dao.ObtenerEstadoActualPalabra(idPartida);
        }
    }
}
