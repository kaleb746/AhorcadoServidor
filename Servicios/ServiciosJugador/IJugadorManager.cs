using Servicios.Dtos;
using System.Collections.Generic;
using System.ServiceModel;

namespace Servicios.ServiciosJugador
{
    [ServiceContract]
    public interface IJugadorManager
    {
        [OperationContract]
        int RegistrarJugador(JugadorDTO jugador);
        [OperationContract]
        int ActualizarJugador(JugadorDTO jugador);
        [OperationContract]
        int EliminarJugador(int idJugador);
        [OperationContract]
        List<JugadorDTO> ObtenerTodosLosJugadores();
    }
}
