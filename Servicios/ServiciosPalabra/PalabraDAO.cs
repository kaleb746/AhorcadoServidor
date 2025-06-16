using Modelo;
using Servicios.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servicios.ServiciosPalabra
{
    public class PalabraDAO
    {
        public List<CategoriaDTO> ObtenerCategorias()
        {
            try
            {
                using (var context = new JuegoAhorcadoEntities())
                {
                    return context.Categorias
                        .Select(c => new CategoriaDTO
                        {
                            Id = c.Id,
                            Nombre = c.Nombre,
                            NombreIngles = c.NombreIngles
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DAO - ObtenerCategorias] {ex.Message}");
                return new List<CategoriaDTO>();
            }
        }

        public List<DificultadDTO> ObtenerDificultades()
        {
            try
            {
                using (var context = new JuegoAhorcadoEntities())
                {
                    return context.Dificultades
                        .Select(d => new DificultadDTO
                        {
                            Id = d.Id,
                            Nombre = d.Nombre,
                            NombreIngles = d.NombreIngles
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DAO - ObtenerDificultades] {ex.Message}");
                return new List<DificultadDTO>();
            }
        }

        public List<PalabraDTO> ObtenerPalabrasPorCategoriaYDificultad(int idCategoria, int idDificultad)
        {
            try
            {
                using (var context = new JuegoAhorcadoEntities())
                {
                    return context.Palabras
                        .Where(p => p.IdCategoria == idCategoria && p.IdDificultad == idDificultad)
                        .Select(p => new PalabraDTO
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            NombreIngles = p.NombreIngles,
                            Descripcion = p.Descripcion,
                            DescripcionIngles = p.DescripcionIngles,
                            IdCategoria = p.IdCategoria,
                            IdDificultad = p.IdDificultad
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error DAO - ObtenerPalabras] {ex.Message}");
                return new List<PalabraDTO>();
            }
        }
    }
}
