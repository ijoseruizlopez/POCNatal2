using Newtonsoft.Json;
using POC.Service.Dto;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POC.Service
{
    public class ServicePerfilMenu
    {
        private string _path = @"./Archivos/PerfilMenu.json";

        public ServicePerfilMenu()
        {

        }

        public int GetNextId()
        {
            List<PerfilMenuDto> listaPerfilMenu = this.GetAll();

            if (listaPerfilMenu != null && listaPerfilMenu.Any())
                return listaPerfilMenu.Max(x => x.Id) + 1;
            else
                return 1;
        }

        public bool AddNew(PerfilMenuDto pPerfilMenu)
        {
            pPerfilMenu.Id = this.GetNextId();
            Save(pPerfilMenu);
            return true;
        }

        public void Save(PerfilMenuDto pPerfilMenu)
        {
            string json = JsonConvert.SerializeObject(pPerfilMenu);

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

        public void Save(List<PerfilMenuDto> listaPerfilMenu)
        {
            foreach (PerfilMenuDto iPerfilMenu in listaPerfilMenu)
                this.Save(iPerfilMenu);
        }

        public bool Modify(PerfilMenuDto pPerfilMenu)
        {
            List<PerfilMenuDto> listaPerfilMenu = this.GetAll();
            if (pPerfilMenu != null && listaPerfilMenu.FirstOrDefault(x => x.Id == pPerfilMenu.Id) != null)
            {
                listaPerfilMenu.FirstOrDefault(x => x.Id == pPerfilMenu.Id).Perfil = pPerfilMenu.Perfil;
                listaPerfilMenu.FirstOrDefault(x => x.Id == pPerfilMenu.Id).Menu = pPerfilMenu.Menu;

                File.Delete(_path);
                this.Save(listaPerfilMenu);
                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            List<PerfilMenuDto> listaPerfilMenu = this.GetAll();

            if (listaPerfilMenu != null && listaPerfilMenu.FirstOrDefault(x => x.Id == id) != null)
            {
                listaPerfilMenu = listaPerfilMenu.FindAll(x => x.Id != id);
                File.Delete(_path);
                this.Save(listaPerfilMenu);

                return true;
            }

            return false;
        }

        public List<PerfilMenuDto> GetAll()
        {
            List<PerfilMenuDto> listaPerfilMenu = new List<PerfilMenuDto>();
            if (File.Exists(_path))
            {
                string line;
                StreamReader file = new StreamReader(_path);
                while ((line = file.ReadLine()) != null)
                {
                    var _perfilMenuDto = JsonConvert.DeserializeObject<PerfilMenuDto>(line);
                    listaPerfilMenu.Add(_perfilMenuDto);
                }

                file.Close();
            }
            return listaPerfilMenu;
        }

        public PerfilMenuDto Get(int id)
        {
            List<PerfilMenuDto> listaPerfilMenu = GetAll();
            return listaPerfilMenu.FirstOrDefault(x => x.Id == id);
        }
    }
}
