using Newtonsoft.Json;
using POC.Service.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace POC.Service
{
    public class ServiceCiudad
    {
        private string _path = @"./Archivos/Ciudad.json";

        public ServiceCiudad()
        {
        }

        public int GetNextId()
        {
            List<CiudadDto> listaCiudad = this.GetAll();

            if (listaCiudad != null && listaCiudad.Any())
                return listaCiudad.Max(x => x.Id) + 1;
            else
                return 1;
        }

        public bool AddNew(CiudadDto ciudad)
        {
            List<CiudadDto> listaCiudad = this.GetAll();
            if (listaCiudad != null && listaCiudad.FirstOrDefault(x => x.Nombre.Trim() == ciudad.Nombre.Trim()) != null)
                return false;

            ciudad.Id = this.GetNextId();
            Save(ciudad);
            return true;
        }

        public void Save(CiudadDto ciudad)
        {
            string json = JsonConvert.SerializeObject(ciudad);

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

        public void Save(List<CiudadDto> listaCiudad)
        {
            foreach (CiudadDto ciudad in listaCiudad)
                this.Save(ciudad);
        }

        public bool Modify(CiudadDto ciudad)
        {
            List<CiudadDto> listaCiudad = this.GetAll();

            if (ciudad != null && listaCiudad.FirstOrDefault(x => x.Id == ciudad.Id) != null)
            {
                listaCiudad.FirstOrDefault(x => x.Id == ciudad.Id).Nombre = ciudad.Nombre;
                listaCiudad.FirstOrDefault(x => x.Id == ciudad.Id).Provincia = ciudad.Provincia;
                File.Delete(_path);
                this.Save(listaCiudad);

                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            List<CiudadDto> listaCiudad = this.GetAll();

            if (listaCiudad != null && listaCiudad.FirstOrDefault(x => x.Id == id) != null)
            {
                ServiceUsuario _srvUsuario = new ServiceUsuario();
                foreach (UsuarioDto usuario in _srvUsuario.GetAll().FindAll(x => x.Ciudad.Id == id))
                    _srvUsuario.Delete(usuario.Id);

                listaCiudad = listaCiudad.FindAll(x => x.Id != id);
                File.Delete(_path);
                this.Save(listaCiudad);

                return true;
            }

            return false;
        }

        public List<CiudadDto> GetAll()
        {
            List<CiudadDto> listaCiudades = new List<CiudadDto>();
            if (File.Exists(_path))
            {

                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(_path);
                while ((line = file.ReadLine()) != null)
                {
                    var ciudadDto = JsonConvert.DeserializeObject<CiudadDto>(line);
                    listaCiudades.Add(ciudadDto);
                }

                file.Close();
            }
            return listaCiudades;
        }

        public CiudadDto Get(int id)
        {
            var listaCiudades = GetAll();

            return listaCiudades.FirstOrDefault(x => x.Id == id);
        }

        public List<CiudadDto> GetCiudadesPorProvincia(int idProvincia)
        {
            List<CiudadDto> listaCiudad = this.GetAll();

            return listaCiudad.FindAll(x => x.Provincia.Id == idProvincia);
        }
    }
}
