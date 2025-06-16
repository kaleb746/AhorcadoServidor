using Servicios.Dtos;

namespace Servicios.ServiciosSesion
{
    public class ServicioSesion : ISesionManager
    {
        public JugadorDTO IniciarSesion(string usuario, string contrasena)
        {
            var dao = new SesionDAO();
            return dao.IniciarSesion(usuario, contrasena);
        }
    }
}
