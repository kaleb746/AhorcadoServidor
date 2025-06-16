using Modelo;
using Servicios.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servicios.ServiciosJugador
{
    public class JugadorDAO
    {
        public int RegistrarJugador(JugadorDTO dto)
        {
            try
            {
                using (var context = new JuegoAhorcadoEntities())
                {
                    bool existe = context.Jugadores.Any(j =>
                        j.Username.ToLower() == dto.Username.ToLower() ||
                        j.Correo.ToLower() == dto.Correo.ToLower());

                    if (existe)
                    {
                        Console.WriteLine("Ya existe un jugador con el mismo username o correo.");
                        return -1;
                    }

                    var jugador = new Jugadores
                    {
                        Nombre = dto.Nombre,
                        ApellidoPaterno = dto.ApellidoPaterno,
                        ApellidoMaterno = dto.ApellidoMaterno,
                        Username = dto.Username,
                        Contrasena = dto.Contrasena,
                        Correo = dto.Correo,
                        Telefono = dto.Telefono,
                        Puntaje = dto.Puntaje,
                        FechaDeNacimiento = dto.FechaDeNacimiento
                    };

                    context.Jugadores.Add(jugador);
                    context.SaveChanges();

                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DAO] {ex.GetType().Name}: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"[Inner] {ex.InnerException.Message}");

                return 0;
            }
        }
        public int ActualizarJugador(JugadorDTO dto)
        {
            try
            {
                using (var context = new JuegoAhorcadoEntities())
                {
                    var jugador = context.Jugadores.FirstOrDefault(j => j.Id == dto.Id);
                    if (jugador == null)
                        return 0;

                    // Verifica que no haya otro jugador con mismo correo/usuario
                    bool duplicado = context.Jugadores.Any(j =>
                        j.Id != dto.Id &&
                        (j.Username.ToLower() == dto.Username.ToLower() ||
                         j.Correo.ToLower() == dto.Correo.ToLower()));

                    if (duplicado)
                        return -1;

                    jugador.Nombre = dto.Nombre;
                    jugador.ApellidoPaterno = dto.ApellidoPaterno;
                    jugador.ApellidoMaterno = dto.ApellidoMaterno;
                    jugador.Username = dto.Username;
                    jugador.Contrasena = dto.Contrasena;
                    jugador.Correo = dto.Correo;
                    jugador.Telefono = dto.Telefono;
                    jugador.FechaDeNacimiento = dto.FechaDeNacimiento;

                    context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DAO - ActualizarJugador] {ex.Message}");
                return 0;
            }
        }
        public int EliminarJugador(int idJugador)
        {
            try
            {
                using (var context = new JuegoAhorcadoEntities())
                {
                    var jugador = context.Jugadores.FirstOrDefault(j => j.Id == idJugador);
                    if (jugador == null)
                    {
                        Console.WriteLine("[EliminarJugador] Jugador no encontrado.");
                        return 0;
                    }

                    context.Jugadores.Remove(jugador);
                    context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DAO - EliminarJugador] {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"[Inner] {ex.InnerException.Message}");

                return -1;
            }
        }
        public List<JugadorDTO> ObtenerTodos()
        {
            using (var context = new JuegoAhorcadoEntities())
            {
                return context.Jugadores
                    .Select(j => new JugadorDTO
                    {
                        Id = j.Id,
                        Nombre = j.Nombre,
                        ApellidoPaterno = j.ApellidoPaterno,
                        ApellidoMaterno = j.ApellidoMaterno,
                        Correo = j.Correo,
                        Username = j.Username,
                        Contrasena = j.Contrasena,
                        Telefono = j.Telefono,
                        Puntaje = j.Puntaje,
                        FechaDeNacimiento = j.FechaDeNacimiento
                    })
                    .ToList();
            }
        }
    }
}
