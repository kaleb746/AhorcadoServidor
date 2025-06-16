using Servicios.Dtos;
using System.ServiceModel;

namespace Servicios.ServiciosSesion
{
    [ServiceContract]
    public interface ISesionManager
    {
        [OperationContract]
        JugadorDTO IniciarSesion(string usuario, string contrasena);
    }
}
