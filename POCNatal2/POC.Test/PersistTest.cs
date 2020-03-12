using POC.Service;
using POC.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace POC.Test
{
    public class PersistTest
    {
        private ServiceUsuario _srvUsuario = new ServiceUsuario();
        private ServiceProvincia _srvProvincia = new ServiceProvincia();
        private ServiceCiudad _srvCiudad = new ServiceCiudad();
        private ServicePais _srvPais = new ServicePais();
        private ServiceMenu _srvMenu = new ServiceMenu();
        private ServicePerfil _srvPerfil = new ServicePerfil();
        private ServicePerfilMenu _srvPerfilMenu = new ServicePerfilMenu();

        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //[Obsolete]
        //[Fact]
        //public void PopulateJson()
        //{
        //    var ciudades = GenerateCiudades();
        //    for (int i = 1; i < 50; i++)
        //    {
        //        UsuarioDto usuario = new UsuarioDto();
        //        usuario.Id = i;
        //        usuario.Nombre = RandomString(10);

        //        if (i % 2==0)
        //            usuario.Ciudad = ciudades.ElementAt(0);
        //        else
        //            usuario.Ciudad = ciudades.ElementAt(1);

        //        _srvUsuario.Save(usuario);
        //    }
        //}

        #region --- USUARIO ---

        [Fact]
        public void PopulateJsonUsuario()
        {
            List<CiudadDto> listaCiudad = _srvCiudad.GetAll();
            List<PerfilDto> listaPerfil = _srvPerfil.GetAll().FindAll(x => x.Activo == true);
            List<UsuarioDto> listaUsuario = null;

            foreach (CiudadDto ciudad in listaCiudad)
            {
                if (listaUsuario == null)
                    listaUsuario = new List<UsuarioDto>();

                for (int i = 1; i < 3; i++)
                {
                    UsuarioDto usuario = new UsuarioDto
                    {
                        //Id = idUsuario++,
                        Nombre = RandomString(10),
                        Ciudad = ciudad
                    };

                    listaUsuario.Add(usuario);
                }
            }

            int idxPerfil = 0;
            foreach (UsuarioDto usuario in listaUsuario)
            {
                usuario.Perfil = listaPerfil[idxPerfil];
                _srvUsuario.AddNew(usuario);

                if (idxPerfil == (listaPerfil.Count - 1))
                    idxPerfil = 0;
                else
                    idxPerfil++;

            }
        }

        [Fact]
        public void ModifyUser()
        {
            var usuarios = _srvUsuario.GetAll();
            var usuario = usuarios.Last();

            usuario.Nombre = "Editado el " + DateTime.Now.ToString();

            _srvUsuario.Modify(usuario);
        }

        [Fact]
        public void EliminarUsuario()
        {
            var usuarios = _srvUsuario.GetAll();
            int idParaBorrar = usuarios.Last().Id;

            _srvUsuario.Delete(idParaBorrar);

            var itemBorrado = _srvUsuario.Get(idParaBorrar);
            Assert.Null(itemBorrado);
        }

        #endregion --- USUARIO ---

        private static List<CiudadDto> GenerateCiudades() {

            List<CiudadDto> ciudades = new List<CiudadDto>();

            CiudadDto ciudad = new CiudadDto();
            ciudad.Id = 1;
            ciudad.Nombre = "Cordoba";
            ciudad.Provincia = new ProvinciaDto();
            ciudad.Provincia.Nombre = "Cordoba";
            ciudad.Provincia.Pais = new PaisDto();
            ciudad.Provincia.Pais.Id = 1;
            ciudad.Provincia.Pais.Nombre = "Argentina";

            ciudades.Add(ciudad);

            CiudadDto ciudad2 = new CiudadDto();
            ciudad2.Id = 2;
            ciudad2.Nombre = "Lima";
            ciudad2.Provincia = new ProvinciaDto();
            ciudad2.Provincia.Nombre = "Lima";
            ciudad2.Provincia.Pais = new PaisDto();
            ciudad2.Provincia.Pais.Id = 2;
            ciudad2.Provincia.Pais.Nombre = "Peru";

            ciudades.Add(ciudad2);



            return ciudades;
        }

        #region --- PAIS ---

        [Fact]
        public void PopulateJsonPaises()
        {
            List<PaisDto> listaPais = new List<PaisDto>()
            {
                new PaisDto{Nombre="Argentina"},
                new PaisDto{Nombre="Peru"},
                new PaisDto{Nombre="Brasil"},
                new PaisDto{Nombre="Chile"},
                new PaisDto{Nombre="Paraguay"},
                new PaisDto{Nombre="Uruguay"},
                new PaisDto{Nombre="Bolivia"},
                new PaisDto{Nombre="Colombia"},
                new PaisDto{Nombre="Ecuador"}
            };

            foreach (PaisDto pais in listaPais)
                _srvPais.AddNew(pais);
        }

        [Fact]
        public void AgregarNuevoPais()
        {
            PaisDto pais = new PaisDto { Nombre = "Venezuela" };
            Assert.True(_srvPais.AddNew(pais), "No se insertó el nuevo registro de PAIS.");
        }

        [Fact]
        public void ModificarPais()
        {
            PaisDto pais = _srvPais.Get(1);
            Assert.NotNull(pais);
            pais.Nombre = pais.Nombre.ToUpper();

            Assert.True(_srvPais.Modify(pais), "No se actualizó el registro de PAIS.");
        }

        [Fact]
        public void GetProvinciasPorPais()
        {
            List<PaisDto> listaPais = _srvPais.GetAll();
            List<ProvinciaDto> listaProvincia = null;

            foreach (PaisDto pais in listaPais)
            {
                listaProvincia = _srvProvincia.GetProvinciasPorPais(pais.Id);
                Assert.True(listaProvincia.Any());
                break;
            }
        }

        #endregion --- PAIS ---

        #region --- PROVINCIA ---

        [Fact]
        public void PopulateJsonProvincias()
        {
            List<PaisDto> listaPais = _srvPais.GetAll();
            List<ProvinciaDto> listaProvincia = null;

            foreach (PaisDto pais in listaPais)
            {
                if (listaProvincia == null)
                    listaProvincia = new List<ProvinciaDto>();

                for (int i = 1; i < 10; i++)
                {
                    ProvinciaDto provincia = new ProvinciaDto{
                        Nombre = string.Format("Provincia_{0}", string.Concat(pais.Id, i)),
                        Pais = pais
                    };

                    listaProvincia.Add(provincia);
                }
            }

            foreach (ProvinciaDto provincia in listaProvincia)
                _srvProvincia.AddNew(provincia);
        }

        [Fact]
        public void AgregarNuevaProvincia()
        {
            PaisDto pais = _srvPais.Get(3);
            ProvinciaDto provincia = new ProvinciaDto { Nombre = pais.Nombre + "_Provincia_" + RandomString(3), Pais = pais };
            Assert.True(_srvProvincia.AddNew(provincia), "No se insertó el nuevo registro de PROVINCIA.");
        }

        [Fact]
        public void ModificarProvincia()
        {
            PaisDto pais = _srvPais.Get(1);
            Assert.NotNull(pais);
            ProvinciaDto provincia = _srvProvincia.Get(31);
            Assert.NotNull(provincia);
            provincia.Nombre = string.Format("{0}_Provincia_{1}", pais.Nombre, provincia.Id);
            provincia.Pais = pais;

            Assert.True(_srvProvincia.Modify(provincia), "No se actualizó el registro de PROVINCIA.");
        }

        #endregion --- PROVINCIA ---

        #region --- CIUDAD ---

        [Fact]
        public void PopulateJsonCiudades()
        {
            List<ProvinciaDto> listaProvincia = _srvProvincia.GetAll();
            List<CiudadDto> listaCiudad = null;

            foreach (ProvinciaDto provincia in listaProvincia)
            {
                if (listaCiudad == null)
                    listaCiudad = new List<CiudadDto>();

                for (int i = 1; i < 5; i++)
                {
                    CiudadDto ciudad = new CiudadDto{
                        Nombre = string.Format("Ciudad_{0}", string.Concat(provincia.Id, i)),
                        Provincia = provincia
                    };

                    listaCiudad.Add(ciudad);
                }
            }

            foreach (CiudadDto ciudad in listaCiudad)
                _srvCiudad.AddNew(ciudad);
        }

        [Fact]
        public void AgregarNuevaCiudad()
        {
            ProvinciaDto provincia = _srvProvincia.Get(31);
            CiudadDto ciudad = new CiudadDto { Nombre = provincia.Pais.Nombre + "_Provincia_Ciudad_" + RandomString(3), Provincia = provincia };
            Assert.True(_srvCiudad.AddNew(ciudad), "No se insertó el nuevo registro de CIUDAD.");
        }

        [Fact]
        public void ModificarCiudad()
        {
            ProvinciaDto provincia = _srvProvincia.Get(21);
            Assert.NotNull(provincia);
            CiudadDto ciudad = _srvCiudad.Get(311);
            Assert.NotNull(ciudad);
            ciudad.Nombre = string.Format("{0}_Provincia_Ciudad_{1}", provincia.Pais.Nombre, ciudad.Id);
            ciudad.Provincia = provincia;

            Assert.True(_srvCiudad.Modify(ciudad), "No se actualizó el registro de CIUDAD.");
        }

        [Fact]
        public void GetCiudadesPorProvincia()
        {
            List<ProvinciaDto> listaProvincia = _srvProvincia.GetAll();
            List<CiudadDto> listaCiudad = null;

            foreach (ProvinciaDto provincia in listaProvincia)
            {
                listaCiudad = _srvCiudad.GetCiudadesPorProvincia(provincia.Id);
                Assert.True(listaCiudad.Any());
                break;
            }
        }

        #endregion --- CIUDAD ---

        #region --- MENU ---

        [Fact]
        public void PopulateJsonMenuSANCOR()
        {
            this.PopulateJsonMenuSANCOR_AsesorLaboral();
            this.PopulateJsonMenuSANCOR_Empresa();
            this.PopulateJsonMenuSANCOR_EmpresaAfiliadaART();
            this.PopulateJsonMenuSANCOR_EstudioJuridico();
            this.PopulateJsonMenuSANCOR_PrestadorMedico();
            this.PopulateJsonMenuSANCOR_PruebasVarias();
        }

        [Fact]
        private void PopulateJsonMenuSANCOR_AsesorLaboral()
        {
            List<MenuDto> listaMenu = new List<MenuDto>
            {
                new MenuDto{ id=-1, name="Datos Estudio Contable", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Datos Particulares", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Actualización Datos", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Suscripción Comunicaciones", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Consulta Contratos", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Contratos con Deuda", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Contratos Vigentes", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Futuros Aumentos de Alícuotas", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Gestiones", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Administración de Empresas", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Asignación de Empresas", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Solicitud de Cotización", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Pedido de Cotización", link="", prefix="", isFavorite=false }
                    } }
                } }
            };

            PerfilDto _perfil = _srvPerfil.GetAll().FirstOrDefault(x => x.Nombre.Trim().ToUpper().Equals("ASESOR LABORAL"));
            foreach (MenuDto iMenu in listaMenu)
            {
                _srvMenu.AddNew(iMenu);

                if (_perfil != null)
                {
                    PerfilMenuDto _perfilMenu;
                    _perfilMenu = new PerfilMenuDto { Id = -1, Perfil = _perfil, Menu = iMenu };
                    _srvPerfilMenu.AddNew(_perfilMenu);
                }
            }
        }

        [Fact]
        private void PopulateJsonMenuSANCOR_Empresa()
        {
            List<MenuDto> listaMenu = new List<MenuDto>
            {
                new MenuDto{ id=-1, name="Accidentes", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Denuncia de Accidente", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Impresión de Denuncias", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Detalle de Accidentes", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Notificaciones", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Citaciones en Junta Médica de ART", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Audiencias en Comisión Médica", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Exámenes Periódicos", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Resumen de Gestión", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Resultados de Exámenes", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Notas", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Suscripción a Comunicaciones", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Contacto", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Relevamiento de Riesgos - RGRL", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Consulta Establecimientos", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Carga Relevamiento", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Consulta Relevamientos", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Plan de Capacitación", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Nuevo Plan", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Registros y Certificados", link="", prefix="", isFavorite=false }
                    } }
                } }
            };

            PerfilDto _perfil = _srvPerfil.GetAll().FirstOrDefault(x => x.Nombre.Trim().ToUpper().Equals("EMPRESA"));
            foreach (MenuDto iMenu in listaMenu)
            {
                _srvMenu.AddNew(iMenu);

                if (_perfil != null)
                {
                    PerfilMenuDto _perfilMenu;
                    _perfilMenu = new PerfilMenuDto { Id = -1, Perfil = _perfil, Menu = iMenu };
                    _srvPerfilMenu.AddNew(_perfilMenu);
                }
            }
        }

        [Fact]
        private void PopulateJsonMenuSANCOR_EmpresaAfiliadaART()
        {
            List<MenuDto> listaMenu = new List<MenuDto>
            {
                new MenuDto{ id=-1, name="Principal", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Datos Empresa", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Actualización Datos", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Carga Form. Transf. Electrónica", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Contrato Afiliación", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Indicadores", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Juicios Activos", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Mediaciones Activas", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Suscripción Comunicaciones", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Estudio Contable", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Movimientos Personal", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Altas y Bajas", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Nómina de Personal Informada en Excel", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Movimientos", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Actualización Datos Empleados", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Cuenta Corriente", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Detalle Cuenta Corriente", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Reintegro de Gastos", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Gestión de Reintegro", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Consulta Reintegros Gestionados", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Dinerarias", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Gestión de Jornales", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Consulta de Jornales Pagados", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Fechas de Pago de la Incapacidad", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Plan de Capacitación", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Planes de Capacitación", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Nuevo Plan", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Registros y Certificados", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Accidentes", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Denuncia de Accidente", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Impresión de Denuncias", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Detalle de Accidentes", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Notificaciones", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Citaciones en Junta Médica de ART", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Audiencias en Comisión Médica", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Certificados", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Certificado de Cobertura", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Impresión de Credenciales", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Período sin Denuncias", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Período con Denuncias", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Nivel de H&S", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Calendario de Envíos", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Viajes al Exterior", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Aviso de Obra", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Alta Aviso de Obra", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Avisos de Obra Cargados", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Relevamiento de Riesgos - RGRL", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Consulta Establecimientos", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Carga Relevamiento", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Consulta Relevamientos", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Exámenes Periódicos", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Resumen de Gestión", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Resultados de Exámenes", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Notas", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Suscripción a Comunicaciones", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Contacto", link="", prefix="", isFavorite=false }
                    } }
                } }
            };

            PerfilDto _perfil = _srvPerfil.GetAll().FirstOrDefault(x => x.Nombre.Trim().ToUpper().Equals("EMPRESA AFILIADA ART"));
            foreach (MenuDto iMenu in listaMenu)
            {
                _srvMenu.AddNew(iMenu);

                if (_perfil != null)
                {
                    PerfilMenuDto _perfilMenu;
                    _perfilMenu = new PerfilMenuDto { Id = -1, Perfil = _perfil, Menu = iMenu };
                    _srvPerfilMenu.AddNew(_perfilMenu);
                }
            }
        }

        [Fact]
        private void PopulateJsonMenuSANCOR_EstudioJuridico()
        {
            List<MenuDto> listaMenu = new List<MenuDto>
            {
                new MenuDto{ id=-1, name="Principal", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Datos del Estudio", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Actualización de Datos", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Derivaciones", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Contratos Anulados", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Gestión de Cobranzas", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Detalle de Contratos", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Cheques Rechazados", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Consultas", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Cuenta Corriente", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Liquidación Previa", link="", prefix="", isFavorite=false }
                    } }
                } }
            };

            PerfilDto _perfil = _srvPerfil.GetAll().FirstOrDefault(x => x.Nombre.Trim().ToUpper().Equals("ESTUDIO JURÍDICO"));
            foreach (MenuDto iMenu in listaMenu)
            {
                _srvMenu.AddNew(iMenu);

                if (_perfil != null)
                {
                    PerfilMenuDto _perfilMenu;
                    _perfilMenu = new PerfilMenuDto { Id = -1, Perfil = _perfil, Menu = iMenu };
                    _srvPerfilMenu.AddNew(_perfilMenu);
                }
            }
        }

        [Fact]
        private void PopulateJsonMenuSANCOR_PrestadorMedico()
        {
            List<MenuDto> listaMenu = new List<MenuDto>
            {
                new MenuDto{ id=-1, name="Principal", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Datos Particulares", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Actualización de Datos", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Instructivos", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Prestadores Kinésicos", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Prestadores Médicos", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Mediclick", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Hotel", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Escribanos", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="E.I.R.", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Centros de Diagnóstico", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Farmacias", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Centros Oftalmológicos", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Anestesia", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Entidades Médicas", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Varios Comprobantes por Rendición ", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Remises", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Sepelio", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Emergencias", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Psicólogos y Psiquiatras", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Ortopedias", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Autorizaciones", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Circuito de Autorizaciones", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Facturas", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Detalle de Facturas", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Formularios", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Denuncias", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Parte Médico Ingreso", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Evolutivo", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Asistensia Médica", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Alta Médica / Fin de Tratamiento", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Reingreso", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Rehabilitación", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Autorización Pago Electrónico", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Planilla de Facturación", link="", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="Protocolos Médicos SRT", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Miembros Inferiores", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Psiquiatria", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Miembros Superiores", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Columna", link="", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Disfonias", link="", prefix="", isFavorite=false }
                    } }
                } }
            };

            PerfilDto _perfil = _srvPerfil.GetAll().FirstOrDefault(x => x.Nombre.Trim().ToUpper().Equals("PRESTADOR MÉDICO"));
            foreach (MenuDto iMenu in listaMenu)
            {
                _srvMenu.AddNew(iMenu);

                if(_perfil != null)
                {
                    PerfilMenuDto _perfilMenu;
                    _perfilMenu = new PerfilMenuDto { Id = -1, Perfil = _perfil, Menu = iMenu };
                    _srvPerfilMenu.AddNew(_perfilMenu);
                }
            }
        }

        [Fact]
        private void PopulateJsonMenuSANCOR_PruebasVarias()
        {
            List<MenuDto> listaMenu = new List<MenuDto>
            {
                new MenuDto{ id=-1, name="MENU #1", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{ id=-1, title="Columna 1", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Item 1.1", link="http://localhost:8080/UsuarioList", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Item 1.2", link="http://localhost:8080/UsuarioList", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Item 1.3", link="http://localhost:8080/UsuarioList", prefix="", isFavorite=false }
                    } },
                    new MenuColumnDto{ id=-1, title="Columna 2", items=new List<MenuItemDto>{
                        new MenuItemDto{ id=-1, name="Item 2.1", link="http://localhost:8080/UsuarioList", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Item 2.2", link="http://localhost:8080/UsuarioList", prefix="", isFavorite=false },
                        new MenuItemDto{ id=-1, name="Item 2.3", link="http://localhost:8080/UsuarioList", prefix="", isFavorite=false }
                    } }
                } },
                new MenuDto{ id=-1, name="MENU #2", link="", cols=new List<MenuColumnDto>{
                    new MenuColumnDto{
                        id =-1,
                        title ="Columna 1",
                        items =new List<MenuItemDto>{
                            new MenuItemDto{ id=-1, name="Item 1.1", link="http://localhost:8080/UsuarioList", prefix="", isFavorite=false },
                            new MenuItemDto{ id=-1, name="Item 1.2", link="http://localhost:8080/UsuarioList", prefix="", isFavorite=false },
                            new MenuItemDto{ id=-1, name="Item 1.3", link="http://localhost:8080/UsuarioList", prefix="", isFavorite=false }
                        },
                        groups=new List<MenuGroupDto>{
                            new MenuGroupDto {
                                id=-1,
                                title="Grupo 1",
                                items =new List<MenuItemDto> {
                                    new MenuItemDto{ id=-1, name="Item G 1.1", link="http://localhost:8080/UsuarioList", prefix="", isFavorite=false },
                                    new MenuItemDto{ id=-1, name="Item G 1.2", link="http://localhost:8080/UsuarioList", prefix="", isFavorite=false }
                                }
                            }
                        }
                    },
                    new MenuColumnDto{
                        id =-1,
                        title ="Columna 2",
                        items =new List<MenuItemDto>{
                            new MenuItemDto{ id=-1, name="Item 2.1", link="http://localhost:8080/UsuarioList", prefix="", isFavorite=false }
                        },
                        groups=new List<MenuGroupDto>{
                            new MenuGroupDto {
                                id=-1,
                                title="Grupo 1",
                                items =new List<MenuItemDto> {
                                    new MenuItemDto{ id=-1, name="Item G 1.1", link="http://localhost:8080/UsuarioList", prefix="fix_004", isFavorite=false }
                                }
                            },
                            new MenuGroupDto {
                                id=-1,
                                title="Grupo 2",
                                items =new List<MenuItemDto> {
                                    new MenuItemDto{ id=-1, name="Item G 2.1", link="http://localhost:8080/UsuarioList", prefix="fix_002", isFavorite=false },
                                    new MenuItemDto{ id=-1, name="Item G 2.2", link="http://localhost:8080/UsuarioList", prefix="fix_001", isFavorite=false }
                                }
                            }
                        }
                    }
                } }
            };

            PerfilDto _perfil = _srvPerfil.GetAll().FirstOrDefault(x => x.Nombre.Trim().ToUpper().Equals("PRUEBAS VARIAS"));
            foreach (MenuDto iMenu in listaMenu)
            {
                _srvMenu.AddNew(iMenu);

                if (_perfil != null)
                {
                    PerfilMenuDto _perfilMenu;
                    _perfilMenu = new PerfilMenuDto { Id = -1, Perfil = _perfil, Menu = iMenu };
                    _srvPerfilMenu.AddNew(_perfilMenu);
                }
            }
        }

        [Fact]
        public void AgregarNuevoMenu_ConHijo()
        {
            MenuDto menu = new MenuDto {
                id = -1,
                name = "Menu con HIJO NUEVO",
                link = "",
                cols = new List<MenuColumnDto> {
                    new MenuColumnDto { id = -1, title = "SubMenu Hijo NUEVO", items = null, groups=null }
                }
            };
            Assert.True(_srvMenu.AddNew(menu), "No se insertó el nuevo registro de MENU.");
        }

        [Fact]
        public void AgregarNuevoMenu_SinHijo()
        {
            MenuDto menu = new MenuDto { id = -1, name = "Menu NUEVO", link = "", cols = null };
            Assert.True(_srvMenu.AddNew(menu), "No se insertó el nuevo registro de MENU.");
        }

        [Fact]
        public void ModificarMenu_ConHijos_QuitarHijo()
        {
            MenuDto menu = _srvMenu.GetAll().FirstOrDefault(x => x.tieneCols == true);
            menu.cols.RemoveAt(0);

            Assert.True(_srvMenu.Modify(menu), "No se actualizó el registro de MENU.");
        }

        [Fact]
        public void ModificarMenu_SinHijos_AgregarHijo()
        {
            MenuDto menu = _srvMenu.GetAll().FirstOrDefault(x => x.tieneCols == false);
            menu.cols = new List<MenuColumnDto>{
                new MenuColumnDto{ id=-1, title="New Cols" }
            };

            Assert.True(_srvMenu.Modify(menu), "No se actualizó el registro de MENU.");
        }

        [Fact]
        public void EliminarMenu_ConHijos_Cascada()
        {
            MenuDto menu = _srvMenu.GetAll().FirstOrDefault(x => x.tieneCols == true);

            Assert.True(_srvMenu.Delete(menu.id), "No se eliminó el registro de MENU.");
        }

        [Fact]
        public void EliminarMenu_ConHijos_EliminaHijo()
        {
            MenuDto menu = _srvMenu.GetAll().FirstOrDefault(x => x.tieneCols == true);

            Assert.True(_srvMenu.Delete(menu.cols[0].id), "No se eliminó el registro de MENU.");
        }

        [Fact]
        public void EliminarMenu_SinHijos()
        {
            MenuDto menu = _srvMenu.GetAll().FirstOrDefault(x => x.tieneCols == false);

            Assert.True(_srvMenu.Delete(menu.id), "No se eliminó el registro de MENU.");
        }

        #endregion --- MENU ---

        #region --- PERFIL ---

        [Fact]
        public void PopulateJsonPerfil()
        {
            List<PerfilDto> listaPerfil = new List<PerfilDto>
            {
                new PerfilDto{ Id=-1, Nombre="Empresa", Descripcion="", Activo=true },
                new PerfilDto{ Id=-1, Nombre="Asesor Laboral", Descripcion="", Activo=true },
                new PerfilDto{ Id=-1, Nombre="Estudio Jurídico", Descripcion="", Activo=true },
                new PerfilDto{ Id=-1, Nombre="Prestador Médico", Descripcion="", Activo=true },
                new PerfilDto{ Id=-1, Nombre="Empresa Afiliada ART", Descripcion="", Activo=true },
                new PerfilDto{ Id=-1, Nombre="Pruebas Varias", Descripcion="Perfil de prueba para MENUES COMPLEX", Activo=true }
            };

            foreach (PerfilDto item in listaPerfil)
                _srvPerfil.AddNew(item);
        }

        [Fact]
        public void AgregarNuevoPerfil()
        {
            PerfilDto perfil = new PerfilDto { Id = -1, Nombre = "Perfil NUEVO", Descripcion = "", Activo = true };
            Assert.True(_srvPerfil.AddNew(perfil), "No se insertó el nuevo registro de PERFIL.");
        }

        [Fact]
        public void ModificarPerfil()
        {
            PerfilDto perfil = _srvPerfil.GetAll().FirstOrDefault();
            perfil.Descripcion = (string.IsNullOrEmpty(perfil.Descripcion) ? "Se carga DESCRIPCION" : string.Empty);

            Assert.True(_srvPerfil.Modify(perfil), "No se actualizó el registro de PERFIL.");
        }

        [Fact]
        public void EliminarPerfil()
        {
            PerfilDto perfil = _srvPerfil.GetAll().LastOrDefault();
            Assert.True(_srvPerfil.Delete(perfil.Id), "No se eliminó el registro de PERFIL.");
        }

        #endregion ---PERFIL ---
    }
}
