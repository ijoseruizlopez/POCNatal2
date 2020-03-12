using Newtonsoft.Json;
using POC.Service.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace POC.Service
{
    public class ServiceUsuario
    {
        private string _path = @"./Archivos/Persona.json";

        public ServiceUsuario()
        {
        }

        public int GetNextId()
        {
            List<UsuarioDto> listaUsuario = this.GetAll();

            if (listaUsuario != null && listaUsuario.Any())
                return listaUsuario.Max(x => x.Id) + 1;
            else
                return 1;
        }

        public bool AddNew(UsuarioDto usuario)
        {
            usuario.Id = this.GetNextId();
            this.Save(usuario);
            return true;
        }

        public void Save(UsuarioDto usuario) {
            string json = JsonConvert.SerializeObject(usuario);

            if (File.Exists(_path))
            {
                using (var writer = new StreamWriter(_path, true))
                {
                    writer.WriteLine(json);
                    writer.Close();
                }
            }
            else {
                File.WriteAllText(_path, json);
                using (var writer = new StreamWriter(_path, true))
                {
                    writer.WriteLine("");
                    writer.Close();
                }
            }

        }

        public void Save(List<UsuarioDto> listaUsuario)
        {
            foreach (UsuarioDto usuario in listaUsuario)
                this.Save(usuario);
        }

        public bool Modify(UsuarioDto usuario)
        {
            List<UsuarioDto> listaUsuarios = this.GetAll();

            if(usuario != null && listaUsuarios != null && listaUsuarios.FirstOrDefault(x => x.Id == usuario.Id) != null)
            {
                listaUsuarios.FirstOrDefault(x => x.Id == usuario.Id).Nombre = usuario.Nombre;
                listaUsuarios.FirstOrDefault(x => x.Id == usuario.Id).Ciudad = usuario.Ciudad;
                listaUsuarios.FirstOrDefault(x => x.Id == usuario.Id).Perfil = usuario.Perfil;
                File.Delete(_path);
                this.Save(listaUsuarios);

                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            List<UsuarioDto> listaUsuarios = this.GetAll();

            if (listaUsuarios.FirstOrDefault(x => x.Id == id) != null)
            {
                listaUsuarios = listaUsuarios.FindAll(x => x.Id != id);
                File.Delete(_path);
                this.Save(listaUsuarios);

                return true;
            }

            return false;
        }

        public List<UsuarioDto> GetAll()
        {
            List<UsuarioDto> listaUsuarios = new List<UsuarioDto>();
            if (File.Exists(_path))
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(_path);
                while ((line = file.ReadLine()) != null)
                {
                    var usuarioDto = JsonConvert.DeserializeObject<UsuarioDto>(line);
                    listaUsuarios.Add(usuarioDto);
                }
                file.Close();
            }
            return listaUsuarios;
        }

        public UsuarioDto Get(int id)
        {
            var listaUsuarios = GetAll();

            return listaUsuarios.FirstOrDefault(x => x.Id == id);
        }
    }
}
