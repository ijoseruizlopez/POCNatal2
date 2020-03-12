using Newtonsoft.Json;
using POC.Service.Dto;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POC.Service
{
    public class ServicePais
    {
        private string _path = @"./Archivos/Pais.json";

        public ServicePais() { }

        public int GetNextId()
        {
            List<PaisDto> listaPais = this.GetAll();

            if (listaPais != null && listaPais.Any())
                return listaPais.Max(x => x.Id) + 1;
            else
                return 1;
        }

        public bool AddNew(PaisDto pais)
        {
            List<PaisDto> listaPais = this.GetAll();
            if (listaPais != null && listaPais.FirstOrDefault(x => x.Nombre.Trim().ToLower() == pais.Nombre.Trim().ToLower()) != null)
                return false;

            pais.Id = this.GetNextId();
            Save(pais);
            return true;
        }

        public void Save(PaisDto pais)
        {
            string json = JsonConvert.SerializeObject(pais);

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

        public void Save(List<PaisDto> listaPais)
        {
            foreach (PaisDto pais in listaPais)
                this.Save(pais);
        }

        public bool Modify(PaisDto pais)
        {
            List<PaisDto> listaPais = this.GetAll();
            if(pais != null && listaPais.FirstOrDefault(x => x.Id == pais.Id) != null)
            {
                listaPais.FirstOrDefault(x => x.Id == pais.Id).Nombre = pais.Nombre;
                File.Delete(_path);
                this.Save(listaPais);
                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            List<PaisDto> listaPais = this.GetAll();

            if (listaPais != null && listaPais.FirstOrDefault(x => x.Id == id) != null)
            {
                ServiceProvincia _srvProvincia = new ServiceProvincia();
                foreach (ProvinciaDto provincia in _srvProvincia.GetProvinciasPorPais(id))
                    _srvProvincia.Delete(provincia.Id);

                listaPais = listaPais.FindAll(x => x.Id != id);
                File.Delete(_path);
                this.Save(listaPais);

                return true;
            }

            return false;
        }

        public List<PaisDto> GetAll()
        {
            List<PaisDto> listaPaises = new List<PaisDto>();
            if (File.Exists(_path))
            {
                string line;
                StreamReader file = new StreamReader(_path);
                while ((line = file.ReadLine()) != null)
                {
                    var _paisDto = JsonConvert.DeserializeObject<PaisDto>(line);
                    listaPaises.Add(_paisDto);
                }

                file.Close();
            }
            return listaPaises;
        }

        public PaisDto Get(int id)
        {
            List<PaisDto> listaPaises = GetAll();
            return listaPaises.FirstOrDefault(x => x.Id == id);
        }

    }
}
