using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Dtos
{
    [DataContract]
    public class PalabraDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string NombreIngles { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string DescripcionIngles { get; set; }

        [DataMember]
        public int IdCategoria { get; set; }

        [DataMember]
        public int IdDificultad { get; set; }
    }

}
