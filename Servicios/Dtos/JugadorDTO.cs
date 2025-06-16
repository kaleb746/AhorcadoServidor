using System;
using System.Runtime.Serialization;

namespace Servicios.Dtos
{
    [DataContract]
    public class JugadorDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Correo { get; set; }

        [DataMember]
        public string Contrasena { get; set; }

        [DataMember]
        public string Telefono { get; set; }

        [DataMember]
        public int Puntaje { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public DateTime FechaDeNacimiento { get; set; }

        [DataMember]
        public string ApellidoMaterno { get; set; }

        [DataMember]
        public string ApellidoPaterno { get; set; }
    }
}
