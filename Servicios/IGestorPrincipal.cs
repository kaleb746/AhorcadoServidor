using Servicios.ServiciosJugador;
using Servicios.ServiciosSesion;
using Servicios.ServiciosPalabra;
using Servicios.ServiciosPartida;
using System.ServiceModel;

namespace Servicios
{
    [ServiceContract(CallbackContract = typeof(IPartidaCallback))]
    public interface IGestorPrincipal : IJugadorManager, ISesionManager, IPalabraManager, IPartidaManager
    {
        [OperationContract]
        bool Ping();
    }
}
