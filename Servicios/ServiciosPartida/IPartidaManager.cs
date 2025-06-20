using Servicios.Dtos;
using System.Collections.Generic;
using System.ServiceModel;

namespace Servicios.ServiciosPartida
{
    [ServiceContract(CallbackContract = typeof(IPartidaCallback))]
    public interface IPartidaManager
    {
        [OperationContract]
        int CrearPartida(int idJugador, int idPalabra, string idioma);
        [OperationContract]
        List<PartidaDisponibleDTO> ObtenerPartidasDisponibles(int idJugadorActual);
        [OperationContract]
        bool UnirseAPartida(int idPartida, int idJugador, string usernameInvitado);
        [OperationContract]
        bool IntentarLetra(int idPartida, int idJugador, char letra);
        [OperationContract]
        string ObtenerEstadoPalabra(int idPartida);
        [OperationContract]
        List<HistorialPartidaDTO> ObtenerHistorialDeJugador(int idJugador);
        [OperationContract]
        string ObtenerDescripcionPalabra(int idPartida);
        [OperationContract]
        int AbandonarPartida(int idJugador, int idPartida);
        [OperationContract]
        int ObtenerPartidaActivaDeJugador(int idJugador);
    }
    public interface IPartidaCallback
    {
        [OperationContract(IsOneWay = true)]
        void NotificarJugadorUnido(string usernameInvitado);
        [OperationContract(IsOneWay = true)]
        void NotificarIntentoLetra(char letra, bool acierto, string estadoActualPalabra);
        [OperationContract(IsOneWay = true)]
        void NotificarFinPartida(bool gano, string mensaje);
    }
}