using Newtonsoft.Json;
using POC.Service.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace POC.Service
{
    public class ServiceMenu
    {
        private string _path = @"./Archivos/Menu.json";
        private ServiceMenuColumn _srvMenuColumn;

        public ServiceMenu()
        {
            _srvMenuColumn = new ServiceMenuColumn();
        }

        public int GetNextId()
        {
            List<MenuDto> listaMenu = this.GetAll();

            if (listaMenu != null && listaMenu.Any())
                return listaMenu.Max(x => x.id) + 1;
            else
                return 1;
        }

        public bool AddNew(MenuDto pMenu)
        {
            if(pMenu.cols != null && pMenu.cols.Any())
            {
                foreach (MenuColumnDto iCols in pMenu.cols)
                    _srvMenuColumn.AddNew(iCols);
            }
            pMenu.id = this.GetNextId();
            Save(pMenu);

            return true;
        }

        public void Save(MenuDto pMenu)
        {
            string json = JsonConvert.SerializeObject(pMenu);

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

        public void Save(List<MenuDto> listaMenu)
        {
            foreach (MenuDto iMenu in listaMenu)
                this.Save(iMenu);
        }

        public bool Modify(MenuDto pMenu)
        {
            List<MenuDto> listaMenu = this.GetAll();

            if (pMenu != null && listaMenu.FirstOrDefault(x => x.id == pMenu.id) != null)
            {
                listaMenu.FirstOrDefault(x => x.id == pMenu.id).name = pMenu.name;
                listaMenu.FirstOrDefault(x => x.id == pMenu.id).link = pMenu.link;
                listaMenu.FirstOrDefault(x => x.id == pMenu.id).cols = pMenu.cols;

                File.Delete(_path);
                this.Save(listaMenu);

                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            List<MenuDto> listaMenu = this.GetAll();

            if (listaMenu != null && listaMenu.FirstOrDefault(x => x.id == id) != null)
            {
                listaMenu = listaMenu.FindAll(x => x.id != id);
                File.Delete(_path);
                this.Save(listaMenu);

                return true;
            }

            return false;
        }

        public List<MenuDto> GetAll()
        {
            List<MenuDto> listaMenues = new List<MenuDto>();
            if (File.Exists(_path))
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(_path);
                while ((line = file.ReadLine()) != null)
                {
                    var MenuDto = JsonConvert.DeserializeObject<MenuDto>(line);
                    listaMenues.Add(MenuDto);
                }

                file.Close();
            }
            return listaMenues;
        }

        public MenuDto Get(int id)
        {
            var listaMenues = GetAll();

            return listaMenues.FirstOrDefault(x => x.id == id);
        }
    }
}
