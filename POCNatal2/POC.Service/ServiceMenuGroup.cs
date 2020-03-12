using Newtonsoft.Json;
using POC.Service.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace POC.Service
{
    public class ServiceMenuGroup
    {
        private string _path = @"./Archivos/MenuGroup.json";
        private ServiceMenuItem _srvMenuItem;

        public ServiceMenuGroup()
        {
            _srvMenuItem = new ServiceMenuItem();
        }

        public int GetNextId()
        {
            List<MenuGroupDto> listaMenuGroup = this.GetAll();

            if (listaMenuGroup != null && listaMenuGroup.Any())
                return listaMenuGroup.Max(x => x.id) + 1;
            else
                return 1;
        }

        public bool AddNew(MenuGroupDto pMenuGroup)
        {
            if (pMenuGroup.items != null && pMenuGroup.items.Any())
            {
                foreach (MenuItemDto item in pMenuGroup.items)
                    _srvMenuItem.AddNew(item);
            }

            pMenuGroup.id = this.GetNextId();
            Save(pMenuGroup);

            return true;
        }

        public void Save(MenuGroupDto pMenuGroup)
        {
            string json = JsonConvert.SerializeObject(pMenuGroup);

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

        public void Save(List<MenuGroupDto> listaMenuGroup)
        {
            foreach (MenuGroupDto iMenuGroup in listaMenuGroup)
                this.Save(iMenuGroup);
        }

        public bool Modify(MenuGroupDto pMenuGroup)
        {
            List<MenuGroupDto> listaMenuGroup = this.GetAll();

            if (pMenuGroup != null && listaMenuGroup.FirstOrDefault(x => x.id == pMenuGroup.id) != null)
            {
                listaMenuGroup.FirstOrDefault(x => x.id == pMenuGroup.id).title = pMenuGroup.title;
                listaMenuGroup.FirstOrDefault(x => x.id == pMenuGroup.id).items = pMenuGroup.items;

                File.Delete(_path);
                this.Save(listaMenuGroup);

                this.ModificarMenuGroupEnMenuColumn(pMenuGroup);

                return true;
            }

            return false;
        }

        public bool Delete(int idMenuGroup)
        {
            List<MenuGroupDto> listaMenuGroup = this.GetAll();

            if (listaMenuGroup != null && listaMenuGroup.FirstOrDefault(x => x.id == idMenuGroup) != null)
            {
                listaMenuGroup = listaMenuGroup.FindAll(x => x.id != idMenuGroup);
                File.Delete(_path);
                this.Save(listaMenuGroup);

                this.EliminarMenuGroupDeMenuColumn(idMenuGroup);

                return true;
            }

            return false;
        }

        public List<MenuGroupDto> GetAll()
        {
            List<MenuGroupDto> listaMenuGroup = new List<MenuGroupDto>();
            if (File.Exists(_path))
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(_path);
                while ((line = file.ReadLine()) != null)
                {
                    var _menuGroupDto = JsonConvert.DeserializeObject<MenuGroupDto>(line);
                    listaMenuGroup.Add(_menuGroupDto);
                }

                file.Close();
            }
            return listaMenuGroup;
        }

        public MenuGroupDto Get(int id)
        {
            var listaMenuGroup = this.GetAll();
            return listaMenuGroup.FirstOrDefault(x => x.id == id);
        }

        private void EliminarMenuGroupDeMenuColumn(int idMenuGroup)
        {
            ServiceMenuColumn _srvMenuColumn = new ServiceMenuColumn();
            foreach (MenuColumnDto iMenuColumn in _srvMenuColumn.GetAll())
            {
                if (iMenuColumn.groups != null && iMenuColumn.groups.FirstOrDefault(x => x.id == idMenuGroup) != null)
                {
                    iMenuColumn.groups = iMenuColumn.groups.FindAll(x => x.id != idMenuGroup);
                    _srvMenuColumn.Modify(iMenuColumn);
                }
            }
        }

        private void ModificarMenuGroupEnMenuColumn(MenuGroupDto pMenuGroup)
        {
            ServiceMenuColumn _srvMenuColumn = new ServiceMenuColumn();
            foreach (MenuColumnDto iMenuColumn in _srvMenuColumn.GetAll())
            {
                if (iMenuColumn.groups != null && iMenuColumn.groups.FirstOrDefault(x => x.id == pMenuGroup.id) != null)
                {
                    iMenuColumn.groups.FirstOrDefault(x => x.id == pMenuGroup.id).title = pMenuGroup.title;
                    iMenuColumn.groups.FirstOrDefault(x => x.id == pMenuGroup.id).items = pMenuGroup.items;

                    _srvMenuColumn.Modify(iMenuColumn);
                }
            }
        }
    }
}
