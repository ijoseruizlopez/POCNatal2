using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Service.Dto
{
    public class MenuItemDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public bool isFavorite { get; set; }
        public string prefix { get; set; }
    }
}
