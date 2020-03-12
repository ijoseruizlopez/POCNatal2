using Newtonsoft.Json;
using POC.Service.Dto;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POC.Service
{
    public class ServicePerfil
    {
        private string _path = @"./Archivos/Perfil.json";

        public ServicePerfil()
        {

        }

        public int GetNextId()
        {
            List<PerfilDto> listaPerfil = this.GetAll();

            if (listaPerfil != null && listaPerfil.Any())
                return listaPerfil.Max(x => x.Id) + 1;
            else
                return 1;
        }

        public bool AddNew(PerfilDto pPerfil)
        {
            List<PerfilDto> listaPerfil = this.GetAll();
            if (listaPerfil != null && listaPerfil.FirstOrDefault(x => x.Nombre.Trim().ToLower() == pPerfil.Nombre.Trim().ToLower()) != null)
                return false;

            pPerfil.Id = this.GetNextId();
            Save(pPerfil);
            return true;
        }

        public void Save(PerfilDto pPerfil)
        {
            string json = JsonConvert.SerializeObject(pPerfil);

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

        public void Save(List<PerfilDto> listaPerfil)
        {
            foreach (PerfilDto iPerfil in listaPerfil)
                this.Save(iPerfil);
        }

        public bool Modify(PerfilDto pPerfil)
        {
            List<PerfilDto> listaPerfil = this.GetAll();
            if (pPerfil != null && listaPerfil.FirstOrDefault(x => x.Id == pPerfil.Id) != null)
            {
                listaPerfil.FirstOrDefault(x => x.Id == pPerfil.Id).Nombre = pPerfil.Nombre;
                listaPerfil.FirstOrDefault(x => x.Id == pPerfil.Id).Descripcion = pPerfil.Descripcion;
                listaPerfil.FirstOrDefault(x => x.Id == pPerfil.Id).Activo = pPerfil.Activo;

                File.Delete(_path);
                this.Save(listaPerfil);
                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            List<PerfilDto> listaPerfil = this.GetAll();

            if (listaPerfil != null && listaPerfil.FirstOrDefault(x => x.Id == id) != null)
            {
                //TO DO: Eliminar relación con demas Modelos (Usuarios / Menues / etc)

                listaPerfil = listaPerfil.FindAll(x => x.Id != id);
                File.Delete(_path);
                this.Save(listaPerfil);

                return true;
            }

            return false;
        }

        public List<PerfilDto> GetAll()
        {
            List<PerfilDto> listaPerfiles = new List<PerfilDto>();
            if (File.Exists(_path))
            {
                string line;
                StreamReader file = new StreamReader(_path);
                while ((line = file.ReadLine()) != null)
                {
                    var _perfilDto = JsonConvert.DeserializeObject<PerfilDto>(line);
                    listaPerfiles.Add(_perfilDto);
                }

                file.Close();
            }
            return listaPerfiles;
        }

        public PerfilDto Get(int id)
        {
            List<PerfilDto> listaPerfiles = GetAll();
            return listaPerfiles.FirstOrDefault(x => x.Id == id);
        }
    }
}
