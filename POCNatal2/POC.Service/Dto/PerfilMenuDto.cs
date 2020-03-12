using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Service.Dto
{
    public class PerfilMenuDto
    {
        public int Id { get; set; }
        public PerfilDto Perfil { get; set; }
        public MenuDto Menu { get; set; }
    }
}
