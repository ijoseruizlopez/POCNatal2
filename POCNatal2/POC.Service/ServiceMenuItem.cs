using Newtonsoft.Json;
using POC.Service.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace POC.Service
{
    public class ServiceMenuItem
    {
        private string _path = @"./Archivos/MenuItem.json";

        public ServiceMenuItem()
        {
        }

        public int GetNextId()
        {
            List<MenuItemDto> listaMenuItem = this.GetAll();

            if (listaMenuItem != null && listaMenuItem.Any())
                return listaMenuItem.Max(x => x.id) + 1;
            else
                return 1;
        }

        public bool AddNew(MenuItemDto pMenuItem)
        {
            pMenuItem.id = this.GetNextId();
            Save(pMenuItem);

            return true;
        }

        public void Save(MenuItemDto pMenuItem)
        {
            string json = JsonConvert.SerializeObject(pMenuItem);

            if (File.Exists(_path))
            {
                using (var writer = new StreamWriter(_path, true))
                {
                    writer.WriteLine(json);
                    writer.Close();
                }
            }
            else
            {
                File.WriteAllText(_path, json);
                using (var writer = new StreamWriter(_path, true))
                {
                    writer.WriteLine(string.Empty);
                    writer.Close();
                }
            }
        }

        public void Save(List<MenuItemDto> listaMenuItem)
        {
            foreach (MenuItemDto iMenuItem in listaMenuItem)
                this.Save(iMenuItem);
        }

        public bool Modify(MenuItemDto pMenuItem)
        {
            List<MenuItemDto> listaMenuItem = this.GetAll();

            if (pMenuItem != null && listaMenuItem.FirstOrDefault(x => x.id == pMenuItem.id) != null)
            {
                listaMenuItem.FirstOrDefault(x => x.id == pMenuItem.id).name = pMenuItem.name;
                listaMenuItem.FirstOrDefault(x => x.id == pMenuItem.id).link = pMenuItem.link;
                listaMenuItem.FirstOrDefault(x => x.id == pMenuItem.id).isFavorite = pMenuItem.isFavorite;
                listaMenuItem.FirstOrDefault(x => x.id == pMenuItem.id).prefix = pMenuItem.prefix;

                File.Delete(_path);
                this.Save(listaMenuItem);

                this.ModificarMenuItemEnMenuGroup(pMenuItem);
                this.ModificarMenuItemEnMenuColumn(pMenuItem);

                return true;
            }

            return false;
        }

        public bool Delete(int idMenuItem)
        {
            List<MenuItemDto> listaMenuItem = this.GetAll();

            if (listaMenuItem != null && listaMenuItem.FirstOrDefault(x => x.id == idMenuItem) != null)
            {
                listaMenuItem = listaMenuItem.FindAll(x => x.id != idMenuItem);
                File.Delete(_path);
                this.Save(listaMenuItem);

                this.EliminarMenuItemDeMenuGroup(idMenuItem);
                this.EliminarMenuItemDeMenuColumn(idMenuItem);

                return true;
            }

            return false;
        }

        public List<MenuItemDto> GetAll()
        {
            List<MenuItemDto> listaMenuItem = new List<MenuItemDto>();
            if (File.Exists(_path))
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(_path);
                while ((line = file.ReadLine()) != null)
                {
                    var _menuItemDto = JsonConvert.DeserializeObject<MenuItemDto>(line);
                    listaMenuItem.Add(_menuItemDto);
                }

                file.Close();
            }
            return listaMenuItem;
        }

        public MenuItemDto Get(int id)
        {
            var listaMenuItem = this.GetAll();
            return listaMenuItem.FirstOrDefault(x => x.id == id);
        }

        private void EliminarMenuItemDeMenuGroup(int idMenuItem)
        {
            ServiceMenuGroup _srvMenuGroup = new ServiceMenuGroup();
            foreach (MenuGroupDto iMenuGroup in _srvMenuGroup.GetAll())
            {
                if(iMenuGroup.items != null && iMenuGroup.items.FirstOrDefault(x => x.id == idMenuItem) != null)
                {
                    iMenuGroup.items = iMenuGroup.items.FindAll(x => x.id != idMenuItem);
                    _srvMenuGroup.Modify(iMenuGroup);
                }
            }
        }

        private void ModificarMenuItemEnMenuGroup(MenuItemDto pMenuItem)
        {
            ServiceMenuGroup _srvMenuGroup = new ServiceMenuGroup();
            foreach (MenuGroupDto iMenuGroup in _srvMenuGroup.GetAll())
            {
                if (iMenuGroup.items != null && iMenuGroup.items.FirstOrDefault(x => x.id == pMenuItem.id) != null)
                {
                    iMenuGroup.items.FirstOrDefault(x => x.id == pMenuItem.id).name = pMenuItem.name;
                    iMenuGroup.items.FirstOrDefault(x => x.id == pMenuItem.id).link = pMenuItem.link;
                    iMenuGroup.items.FirstOrDefault(x => x.id == pMenuItem.id).prefix = pMenuItem.prefix;
                    iMenuGroup.items.FirstOrDefault(x => x.id == pMenuItem.id).isFavorite = pMenuItem.isFavorite;

                    _srvMenuGroup.Modify(iMenuGroup);
                }
            }
        }

        private void EliminarMenuItemDeMenuColumn(int idMenuItem)
        {
            ServiceMenuColumn _srvMenuColumn = new ServiceMenuColumn();
            foreach (MenuColumnDto iMenuColumn in _srvMenuColumn.GetAll())
            {
                if (iMenuColumn.items != null && iMenuColumn.items.FirstOrDefault(x => x.id == idMenuItem) != null)
                {
                    iMenuColumn.items = iMenuColumn.items.FindAll(x => x.id != idMenuItem);
                    _srvMenuColumn.Modify(iMenuColumn);
                }
            }
        }

        private void ModificarMenuItemEnMenuColumn(MenuItemDto pMenuItem)
        {
            ServiceMenuColumn _srvMenuColumn = new ServiceMenuColumn();
            foreach (MenuColumnDto iMenuColumn in _srvMenuColumn.GetAll())
            {
                if (iMenuColumn.items != null && iMenuColumn.items.FirstOrDefault(x => x.id == pMenuItem.id) != null)
                {
                    iMenuColumn.items.FirstOrDefault(x => x.id == pMenuItem.id).name = pMenuItem.name;
                    iMenuColumn.items.FirstOrDefault(x => x.id == pMenuItem.id).link = pMenuItem.link;
                    iMenuColumn.items.FirstOrDefault(x => x.id == pMenuItem.id).prefix = pMenuItem.prefix;
                    iMenuColumn.items.FirstOrDefault(x => x.id == pMenuItem.id).isFavorite = pMenuItem.isFavorite;

                    _srvMenuColumn.Modify(iMenuColumn);
                }
            }
        }
    }
}
