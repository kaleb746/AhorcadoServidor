using Servicios.Dtos;
using Servicios.ServiciosJugador;
using Servicios.ServiciosPalabra;
using Servicios.ServiciosPartida;
using Servicios.ServiciosSesion;
using System.Collections.Generic;
using System.ServiceModel;

namespace Servicios
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ServicioPrincipal : IGestorPrincipal
    {
        private readonly IJugadorManager _jugadorManager;
        private readonly ISesionManager _sesionManager;
        private readonly IPalabraManager _palabraManager;
        private readonly IPartidaManager _partidaManager;
        public ServicioPrincipal()
        {
            _jugadorManager = new ServicioJugador();
            _sesionManager = new ServicioSesion();
            _palabraManager = new ServicioPalabra();
            _partidaManager = new ServicioPartida();
        }
        public bool Ping()
        {
            return true;
        }
        public int RegistrarJugador(JugadorDTO jugador) => _jugadorManager.RegistrarJugador(jugador);

        public int ActualizarJugador(JugadorDTO jugador) => _jugadorManager.ActualizarJugador(jugador);

        public int EliminarJugador(int idJugador) => _jugadorManager.EliminarJugador(idJugador);

        public List<JugadorDTO> ObtenerTodosLosJugadores() => _jugadorManager.ObtenerTodosLosJugadores();

        public JugadorDTO IniciarSesion(string usuario, string contrsena) => _sesionManager.IniciarSesion(usuario, contrsena);

        public List<CategoriaDTO> ObtenerCategorias() => _palabraManager.ObtenerCategorias();

        public List<DificultadDTO> ObtenerDificultades() => _palabraManager.ObtenerDificultades();

        public List<PalabraDTO> ObtenerPalabrasPorCategoriaYDificultad(int idCategoria, int idDificultad) => _palabraManager.ObtenerPalabrasPorCategoriaYDificultad(idCategoria, idDificultad);

        public int CrearPartida(int idJugador, int idPalabra, string idioma) => _partidaManager.CrearPartida(idJugador, idPalabra, idioma);

        public List<PartidaDisponibleDTO> ObtenerPartidasDisponibles(int idJugadorActual) => _partidaManager.ObtenerPartidasDisponibles(idJugadorActual);

        public bool UnirseAPartida(int idPartida, int idJugador, string usernameInvitado)=> _partidaManager.UnirseAPartida(idPartida, idJugador, usernameInvitado);

        public bool IntentarLetra(int idPartida, int idJugador, char letra) => _partidaManager.IntentarLetra(idPartida, idJugador, letra);

        public string ObtenerEstadoPalabra(int idPartida) => _partidaManager.ObtenerEstadoPalabra(idPartida);

        public List<HistorialPartidaDTO> ObtenerHistorialDeJugador(int idJugador) => _partidaManager.ObtenerHistorialDeJugador(idJugador);

        public string ObtenerDescripcionPalabra(int idPartida) => _partidaManager.ObtenerDescripcionPalabra(idPartida);
    }
}
