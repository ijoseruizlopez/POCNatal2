using Newtonsoft.Json;
using POC.Service.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace POC.Service
{
    public class ServiceMenuColumn
    {
        private string _path = @"./Archivos/MenuColumn.json";
        private ServiceMenuGroup _srvMenuGroup;
        private ServiceMenuItem _srvMenuItem;

        public ServiceMenuColumn()
        {
            _srvMenuGroup = new ServiceMenuGroup();
            _srvMenuItem = new ServiceMenuItem();
        }

        public int GetNextId()
        {
            List<MenuColumnDto> listaMenuColumn = this.GetAll();

            if (listaMenuColumn != null && listaMenuColumn.Any())
                return listaMenuColumn.Max(x => x.id) + 1;
            else
                return 1;
        }

        public bool AddNew(MenuColumnDto pMenuColumn)
        {
            if (pMenuColumn.items != null && pMenuColumn.items.Any())
            {
                foreach (MenuItemDto item in pMenuColumn.items)
                    _srvMenuItem.AddNew(item);
            }
            if (pMenuColumn.groups != null && pMenuColumn.groups.Any())
            {
                foreach (MenuGroupDto iGroup in pMenuColumn.groups)
                    _srvMenuGroup.AddNew(iGroup);
            }

            pMenuColumn.id = this.GetNextId();
            Save(pMenuColumn);

            return true;
        }

        public void Save(MenuColumnDto pMenuColumn)
        {
            string json = JsonConvert.SerializeObject(pMenuColumn);

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

        public void Save(List<MenuColumnDto> listaMenuColumn)
        {
            foreach (MenuColumnDto iMenuColumn in listaMenuColumn)
                this.Save(iMenuColumn);
        }

        public bool Modify(MenuColumnDto pMenuColumn)
        {
            List<MenuColumnDto> listaMenuColumn = this.GetAll();

            if (pMenuColumn != null && listaMenuColumn.FirstOrDefault(x => x.id == pMenuColumn.id) != null)
            {
                listaMenuColumn.FirstOrDefault(x => x.id == pMenuColumn.id).title = pMenuColumn.title;
                listaMenuColumn.FirstOrDefault(x => x.id == pMenuColumn.id).items = pMenuColumn.items;
                listaMenuColumn.FirstOrDefault(x => x.id == pMenuColumn.id).groups = pMenuColumn.groups;

                File.Delete(_path);
                this.Save(listaMenuColumn);

                this.ModificarMenuColumnEnMenu(pMenuColumn);

                return true;
            }

            return false;
        }

        public bool Delete(int idMenuColumn)
        {
            List<MenuColumnDto> listaMenuColumn = this.GetAll();

            if (listaMenuColumn != null && listaMenuColumn.FirstOrDefault(x => x.id == idMenuColumn) != null)
            {
                listaMenuColumn = listaMenuColumn.FindAll(x => x.id != idMenuColumn);
                File.Delete(_path);
                this.Save(listaMenuColumn);

                this.EliminarMenuColumnDeMenu(idMenuColumn);

                return true;
            }

            return false;
        }

        public List<MenuColumnDto> GetAll()
        {
            List<MenuColumnDto> listaMenuColumn = new List<MenuColumnDto>();
            if (File.Exists(_path))
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(_path);
                while ((line = file.ReadLine()) != null)
                {
                    var _menuColumnDto = JsonConvert.DeserializeObject<MenuColumnDto>(line);
                    listaMenuColumn.Add(_menuColumnDto);
                }

                file.Close();
            }
            return listaMenuColumn;
        }

        public MenuColumnDto Get(int id)
        {
            var listaMenuColumn = this.GetAll();
            return listaMenuColumn.FirstOrDefault(x => x.id == id);
        }

        private void EliminarMenuColumnDeMenu(int idMenuColumn)
        {
            ServiceMenu _srvMenu = new ServiceMenu();
            foreach (MenuDto iMenu in _srvMenu.GetAll())
            {
                if (iMenu.cols != null && iMenu.cols.FirstOrDefault(x => x.id == idMenuColumn) != null)
                {
                    iMenu.cols = iMenu.cols.FindAll(x => x.id != idMenuColumn);
                    _srvMenu.Modify(iMenu);
                }
            }
        }

        private void ModificarMenuColumnEnMenu(MenuColumnDto pMenuColumn)
        {
            ServiceMenu _srvMenu = new ServiceMenu();
            foreach (MenuDto iMenu in _srvMenu.GetAll())
            {
                if (iMenu.cols != null && iMenu.cols.FirstOrDefault(x => x.id == pMenuColumn.id) != null)
                {
                    iMenu.cols.FirstOrDefault(x => x.id == pMenuColumn.id).title = pMenuColumn.title;
                    iMenu.cols.FirstOrDefault(x => x.id == pMenuColumn.id).items = pMenuColumn.items;
                    iMenu.cols.FirstOrDefault(x => x.id == pMenuColumn.id).groups = pMenuColumn.groups;

                    _srvMenu.Modify(iMenu);
                }
            }
        }
    }
}
