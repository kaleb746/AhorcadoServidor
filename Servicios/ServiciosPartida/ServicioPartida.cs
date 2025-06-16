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
        public int CrearPartida(int idJugador, int idPalabra)
        {
            var dao = new PartidaDAO();
            int idPartida = dao.CrearPartida(idJugador, idPalabra); 

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
            var (acierto, estadoActualPalabra) = dao.IntentarLetra(idPartida, letra);

            Task.Run(() =>
            {
                using (var context = new JuegoAhorcadoEntities())
                {
                    var jugadores = context.JugadoresPartidas
                        .Where(jp => jp.IdPartida == idPartida)
                        .Select(jp => jp.IdJugador)
                        .ToList();

                    foreach (int jugador in jugadores)
                    {
                        CallbackManager.IntentarNotificarIntentoLetra(jugador, letra, acierto, estadoActualPalabra);
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
        public bool FinalizarPartida(int idPartida, int idJugadorGanador)
        {
            try
            {
                using (var context = new JuegoAhorcadoEntities())
                {
                    // Cambiar estado a finalizado
                    var partida = context.Partidas.FirstOrDefault(p => p.Id == idPartida);
                    if (partida != null)
                        partida.IdEstadoPartida = 3;

                    // Marcar como ganador
                    var ganador = context.JugadoresPartidas
                        .FirstOrDefault(jp => jp.IdPartida == idPartida && jp.IdJugador == idJugadorGanador);
                    if (ganador != null)
                        ganador.Ganador = true;

                    // Buscar al perdedor
                    var perdedor = context.JugadoresPartidas
                        .FirstOrDefault(jp => jp.IdPartida == idPartida && jp.IdJugador != idJugadorGanador);

                    context.SaveChanges();

                    if (perdedor != null)
                    {
                        CallbackManager.NotificarFinDePartida(idJugadorGanador, perdedor.IdJugador);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FinalizarPartida] Error: {ex.Message}");
                return false;
            }
        }

    }
}
