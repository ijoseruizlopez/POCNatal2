using Newtonsoft.Json;
using POC.Service.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace POC.Service
{
    public class ServiceProvincia
    {
        private string _path = @"./Archivos/Provincia.json";

        public ServiceProvincia()
        {

        }

        public int GetNextId()
        {
            List<ProvinciaDto> listaProvincia = this.GetAll();

            if (listaProvincia != null && listaProvincia.Any())
                return listaProvincia.Max(x => x.Id) + 1;
            else
                return 1;
        }

        public bool AddNew(ProvinciaDto provincia)
        {
            List<ProvinciaDto> listaProvincia = this.GetAll();
            if (listaProvincia != null && listaProvincia.FirstOrDefault(x => x.Nombre.Trim() == provincia.Nombre.Trim()) != null)
                return false;

            provincia.Id = this.GetNextId();
            Save(provincia);
            return true;
        }

        public void Save(ProvinciaDto provincia)
        {
            string json = JsonConvert.SerializeObject(provincia);

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

        public void Save(List<ProvinciaDto> listaProvincia)
        {
            foreach (ProvinciaDto provincia in listaProvincia)
                this.Save(provincia);
        }

        public bool Modify(ProvinciaDto provincia)
        {
            List<ProvinciaDto> listaProvincia = this.GetAll();

            if (provincia != null && listaProvincia.FirstOrDefault(x => x.Id == provincia.Id) != null)
            {
                listaProvincia.FirstOrDefault(x => x.Id == provincia.Id).Nombre = provincia.Nombre;
                listaProvincia.FirstOrDefault(x => x.Id == provincia.Id).Pais = provincia.Pais;
                File.Delete(_path);
                this.Save(listaProvincia);

                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            List<ProvinciaDto> listaProvincia = this.GetAll();

            if (listaProvincia != null && listaProvincia.FirstOrDefault(x => x.Id == id) != null)
            {
                ServiceCiudad _srvCiudad = new ServiceCiudad();
                foreach (CiudadDto ciudad in _srvCiudad.GetCiudadesPorProvincia(id))
                    _srvCiudad.Delete(ciudad.Id);

                listaProvincia = listaProvincia.FindAll(x => x.Id != id);
                File.Delete(_path);
                this.Save(listaProvincia);

                return true;
            }

            return false;
        }

        public List<ProvinciaDto> GetAll()
        {
            List<ProvinciaDto> listaProvincias = new List<ProvinciaDto>();
            if (File.Exists(_path))
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(_path);
                while ((line = file.ReadLine()) != null)
                {
                    var provinciaDto = JsonConvert.DeserializeObject<ProvinciaDto>(line);
                    listaProvincias.Add(provinciaDto);
                }

                file.Close();
            }
            return listaProvincias;
        }

        public ProvinciaDto Get(int id)
        {
            var listaProvincias = GetAll();

            return listaProvincias.FirstOrDefault(x => x.Id == id);
        }

        public List<ProvinciaDto> GetProvinciasPorPais(int idPais)
        {
            List<ProvinciaDto> listaProvincia = this.GetAll();

            return listaProvincia.FindAll(x => x.Pais.Id == idPais);
        }

    }
}
