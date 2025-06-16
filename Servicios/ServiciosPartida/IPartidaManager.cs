using Servicios.Dtos;
using System.Collections.Generic;
using System.ServiceModel;

namespace Servicios.ServiciosPartida
{
    [ServiceContract(CallbackContract = typeof(IPartidaCallback))]
    public interface IPartidaManager
    {
        [OperationContract]
        int CrearPartida(int idJugador, int idPalabra);
        [OperationContract]
        List<PartidaDisponibleDTO> ObtenerPartidasDisponibles(int idJugadorActual);
        [OperationContract]
        bool UnirseAPartida(int idPartida, int idJugador, string usernameInvitado);
        [OperationContract]
        bool IntentarLetra(int idPartida, int idJugador, char letra);
        [OperationContract]
        string ObtenerEstadoPalabra(int idPartida);
        [OperationContract]
        bool FinalizarPartida(int idPartida, int idJugadorGanador);
    }
    public interface IPartidaCallback
    {
        [OperationContract(IsOneWay = true)]
        void NotificarJugadorUnido(string usernameInvitado);
        [OperationContract(IsOneWay = true)]
        void NotificarIntentoLetra(char letra, bool acierto, string estadoActualPalabra);
        [OperationContract(IsOneWay = true)]
        void NotificarFinDePartida(string mensajeResultado, bool ganaste);

    }
}