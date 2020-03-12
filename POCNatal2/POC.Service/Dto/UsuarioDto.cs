using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Service.Dto
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public CiudadDto Ciudad { get; set; }
        public PerfilDto Perfil { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
