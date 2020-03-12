using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Service.Dto
{
    public class MenuDto
    {
        //public int id { get; set; }
        //public string link { get; set; }
        //public string name { get; set; }
        //public List<MenuDto> items { get; set; }
        //public bool isFavorite { get; set; }
        //public string title { get; set; }
        //public string prefix { get; set; }

        //public bool esPadre { get { return (items != null && items.Count > 0); } }
        public int id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public List<MenuColumnDto> cols { get; set; }

        public bool tieneCols { get { return (cols != null && cols.Count > 0); } }
    }
}
