using Servicios.Dtos;
using System.Collections.Generic;
using System.ServiceModel;

namespace Servicios.ServiciosPalabra
{
    [ServiceContract]
    public interface IPalabraManager
    {
        [OperationContract]
        List<CategoriaDTO> ObtenerCategorias();

        [OperationContract]
        List<DificultadDTO> ObtenerDificultades();

        [OperationContract]
        List<PalabraDTO> ObtenerPalabrasPorCategoriaYDificultad(int idCategoria, int idDificultad);
    }
}
