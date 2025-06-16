using System;
using System.Runtime.Serialization;

namespace Servicios.Dtos
{
    [DataContract]
    public class PartidaDisponibleDTO
    {
        [DataMember]
        public int IdPartida { get; set; }

        [DataMember]
        public string NombrePalabra { get; set; }

        [DataMember]
        public string NombreCategoria { get; set; }

        [DataMember]
        public string NombreDificultad { get; set; }

        [DataMember]
        public DateTime FechaCreacion { get; set; }

        [DataMember]
        public string Idioma { get; set; }

        [DataMember]
        public int IdJugadorAnfitrion { get; set; }
        [DataMember]
        public string UsuarioAnfitrion { get; set; }
    }
}
