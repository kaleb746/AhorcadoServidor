using Modelo;
using Servicios.Dtos;
using System;
using System.Linq;

namespace Servicios.ServiciosSesion
{
    public class SesionDAO
    {
        public JugadorDTO IniciarSesion(string usuario, string contrasena)
        {
            using (var contexto = new JuegoAhorcadoEntities())
            {
                var entidad = contexto.Jugadores
                    .FirstOrDefault(j => j.Username == usuario && j.Contrasena == contrasena);

                if (entidad != null)
                {
                    return new JugadorDTO
                    {
                        Id = entidad.Id,
                        Nombre = entidad.Nombre,
                        ApellidoPaterno = entidad.ApellidoPaterno,
                        ApellidoMaterno = entidad.ApellidoMaterno,
                        Username = entidad.Username,
                        Contrasena = entidad.Contrasena,
                        Correo = entidad.Correo,
                        Telefono = entidad.Telefono,
                        Puntaje = entidad.Puntaje,
                        FechaDeNacimiento = entidad.FechaDeNacimiento
                    };
                }

                return null;
            }
        }
    }
}
