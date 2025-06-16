using Modelo;
using Servicios.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servicios.ServiciosPartida
{
    public class PartidaDAO
    {
        public int CrearPartida(int idJugador, int idPalabra)
        {
            try
            {
                using (var context = new JuegoAhorcadoEntities())
                {
                    const int ID_ESTADO_EN_ESPERA = 1;

                    var existeEstado = context.EstadosPartida.Any(e => e.Id == ID_ESTADO_EN_ESPERA);
                    if (!existeEstado)
                    {
                        Console.WriteLine($"[CrearPartida] Estado con ID {ID_ESTADO_EN_ESPERA} no existe.");
                        return -1;
                    }

                    var partida = new Partidas
                    {
                        IdPalabraSelecionada = idPalabra,
                        IdEstadoPartida = ID_ESTADO_EN_ESPERA,
                        FechaCreacionPartida = DateTime.Now,
                        IdiomaPartida = "es"
                    };

                    context.Partidas.Add(partida);
                    context.SaveChanges();

                    var jugadorPartida = new JugadoresPartidas
                    {
                        IdJugador = idJugador,
                        IdPartida = partida.Id,
                        Rol = "Anfitrion",
                        Ganador = false
                    };

                    context.JugadoresPartidas.Add(jugadorPartida);
                    context.SaveChanges();

                    return partida.Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DAO - CrearPartida] {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"[Inner] {ex.InnerException.Message}");
                return 0;
            }
        }
        public List<PartidaDisponibleDTO> ObtenerPartidasDisponibles(int idJugadorActual)
        {
            using (var context = new JuegoAhorcadoEntities())
            {
                var partidas = (from partida in context.Partidas
                                join jp in context.JugadoresPartidas on partida.Id equals jp.IdPartida
                                join jugador in context.Jugadores on jp.IdJugador equals jugador.Id
                                where partida.IdEstadoPartida == 1
                                      && jp.Rol == "Anfitrion"
                                      && jp.IdJugador != idJugadorActual
                                select new PartidaDisponibleDTO
                                {
                                    IdPartida = partida.Id,
                                    NombrePalabra = partida.Palabras.Nombre,
                                    NombreCategoria = partida.Palabras.Categorias.Nombre,
                                    NombreDificultad = partida.Palabras.Dificultades.Nombre,
                                    FechaCreacion = partida.FechaCreacionPartida,
                                    Idioma = partida.IdiomaPartida,
                                    IdJugadorAnfitrion = jp.IdJugador,
                                    UsuarioAnfitrion = jugador.Username
                                }).ToList();

                return partidas;
            }
        }
        public bool RegistrarInvitadoEnPartida(int idPartida, int idJugador)
        {
            try
            {
                using (var context = new JuegoAhorcadoEntities())
                {
                    bool yaRegistrado = context.JugadoresPartidas.Any(jp =>
                        jp.IdPartida == idPartida && jp.IdJugador == idJugador);
                    if (yaRegistrado)
                        return false;

                    bool yaHayInvitado = context.JugadoresPartidas.Any(jp =>
                        jp.IdPartida == idPartida && jp.Rol == "Invitado");
                    if (yaHayInvitado)
                        return false;

                    var nuevo = new JugadoresPartidas
                    {
                        IdPartida = idPartida,
                        IdJugador = idJugador,
                        Rol = "Invitado",
                        Ganador = false
                    };

                    context.JugadoresPartidas.Add(nuevo);

                    const int ID_ESTADO_EN_CURSO = 2;
                    var partida = context.Partidas.FirstOrDefault(p => p.Id == idPartida);
                    if (partida != null)
                    {
                        partida.IdEstadoPartida = ID_ESTADO_EN_CURSO;
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DAO - RegistrarInvitadoEnPartida] {ex.Message}");
                return false;
            }
        }
        private static readonly Dictionary<int, HashSet<char>> LetrasAdivinadasPorPartida = new Dictionary<int, HashSet<char>>();

        public (bool acierto, string estadoActualPalabra) IntentarLetra(int idPartida, char letra)
        {
            using (var context = new JuegoAhorcadoEntities())
            {
                var partida = context.Partidas
                    .Include("Palabras")
                    .FirstOrDefault(p => p.Id == idPartida);

                if (partida == null)
                {
                    Console.WriteLine($"[DAO - IntentarLetra] No se encontró la partida con ID {idPartida}");
                    return (false, "");
                }

                string palabra = partida.Palabras.Nombre.ToUpperInvariant();
                letra = char.ToUpperInvariant(letra);

                if (!LetrasAdivinadasPorPartida.ContainsKey(idPartida))
                    LetrasAdivinadasPorPartida[idPartida] = new HashSet<char>();

                LetrasAdivinadasPorPartida[idPartida].Add(letra);

                bool acierto = palabra.Contains(letra);

                string estadoActual = new string(palabra.Select(c => LetrasAdivinadasPorPartida[idPartida].Contains(c) ? c : '_').ToArray());

                return (acierto, estadoActual);
            }
        }
        public string ObtenerEstadoActualPalabra(int idPartida)
        {
            using (var context = new JuegoAhorcadoEntities())
            {
                var partida = context.Partidas.Find(idPartida);
                if (partida == null) return "";

                var palabra = partida.Palabras;
                if (palabra == null) return "";

                var letrasAdivinadas = ObtenerLetrasAdivinadas(idPartida);

                return new string(palabra.Nombre
                    .Select(c => letrasAdivinadas.Contains(char.ToUpper(c)) ? c : '_')
                    .ToArray());
            }
        }
        public bool FinalizarPartida(int idPartida, int idJugadorGanador)
        {
            try
            {
                using (var context = new JuegoAhorcadoEntities())
                {
                    var partida = context.Partidas.FirstOrDefault(p => p.Id == idPartida);
                    if (partida == null) return false;

                    const int ID_ESTADO_FINALIZADA = 3;
                    partida.IdEstadoPartida = ID_ESTADO_FINALIZADA;

                    var jugadorPartida = context.JugadoresPartidas
                        .FirstOrDefault(jp => jp.IdPartida == idPartida && jp.IdJugador == idJugadorGanador);

                    if (jugadorPartida != null)
                    {
                        jugadorPartida.Ganador = true;
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DAO - FinalizarPartida] Error: {ex.Message}");
                return false;
            }
        }
        private HashSet<char> ObtenerLetrasAdivinadas(int idPartida)
        {
            if (!LetrasAdivinadasPorPartida.TryGetValue(idPartida, out var letras))
            {
                letras = new HashSet<char>();
                LetrasAdivinadasPorPartida[idPartida] = letras;
            }

            return letras;
        }
    }
}
