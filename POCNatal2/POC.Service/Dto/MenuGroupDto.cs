using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Service.Dto
{
    public class MenuGroupDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<MenuItemDto> items { get; set; }
    }
}
