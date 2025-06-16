using Servicios.Dtos;
using System.Collections.Generic;

namespace Servicios.ServiciosPalabra
{
    public class ServicioPalabra : IPalabraManager
    {
        private readonly PalabraDAO dao = new PalabraDAO();

        public List<CategoriaDTO> ObtenerCategorias()
        {
            return dao.ObtenerCategorias();
        }

        public List<DificultadDTO> ObtenerDificultades()
        {
            return dao.ObtenerDificultades();
        }

        public List<PalabraDTO> ObtenerPalabrasPorCategoriaYDificultad(int idCategoria, int idDificultad)
        {
            return dao.ObtenerPalabrasPorCategoriaYDificultad(idCategoria, idDificultad);
        }
    }
}
