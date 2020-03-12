using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Service.Dto
{
    public class MenuColumnDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<MenuItemDto> items { get; set; }
        public List<MenuGroupDto> groups { get; set; }
    }
}
