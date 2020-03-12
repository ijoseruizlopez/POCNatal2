using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POC.Service;
using POC.Service.Dto;

namespace POC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private ServiceMenu _srvMenu = null;
        private ServiceMenuItem _srvMenuItem = null;
        private ServiceMenuGroup _srvMenuGroup = null;
        private ServiceMenuColumn _srvMenuColumn = null;

        //Falta inyectar el servicio atraves de un conteiner
        public MenuController()
        {
            _srvMenu = new ServiceMenu();
        }

        #region --- Métodos de MENU ---
        [HttpGet("/api/MenuController/Get/{id}")]
        public MenuDto Get(int id)
        {
            return _srvMenu.Get(id);
        }

        [HttpPost("/api/MenuController/Save")]
        public int Save(MenuDto menu)
        {
            _srvMenu.AddNew(menu);
            return menu.id;
        }

        [HttpPut("/api/MenuController/Modify")]
        public void Modify(MenuDto menu)
        {
            _srvMenu.Modify(menu);
        }

        [HttpDelete("/api/MenuController/Delete/{id}")]
        public bool Delete(int id)
        {
            return _srvMenu.Delete(id);
        }

        [HttpGet("/api/MenuController/GetAll")]
        public List<MenuDto> GetAll()
        {
            return _srvMenu.GetAll();
        }

        [HttpGet("/api/MenuController/GetMenuesRaiz")]
        public List<MenuDto> GetMenuesRaiz()
        {
            return _srvMenu.GetAll().FindAll(x => x.tieneCols == true);
        }

        [HttpGet("/api/MenuController/GetMenuPorUsuario/{idUsuario}")]
        public List<MenuDto> GetMenuPorUsuario(int idUsuario)
        {
            ServiceUsuario _srvUsuario = new ServiceUsuario();
            UsuarioDto usuario = _srvUsuario.Get(idUsuario);

            ServicePerfilMenu _srvPerfilMenu = new ServicePerfilMenu();
            List<PerfilMenuDto> listaPerfilMenu = _srvPerfilMenu.GetAll().FindAll(x => x.Perfil.Id == usuario.Perfil.Id);

            List<MenuDto> listaRETURN = new List<MenuDto>();
            if(listaPerfilMenu!= null && listaPerfilMenu.Any())
            {
                foreach (PerfilMenuDto iPerfilMenu in listaPerfilMenu)
                    listaRETURN.Add(_srvMenu.Get(iPerfilMenu.Menu.id));
            }

            return listaRETURN;
        }

        [HttpGet("/api/MenuController/GetMenuPorPerfil/{idPerfil}")]
        public List<MenuDto> GetMenuPorPerfil(int idPerfil)
        {
            ServicePerfilMenu _srvPerfilMenu = new ServicePerfilMenu();
            List<PerfilMenuDto> listaPerfilMenu = _srvPerfilMenu.GetAll().FindAll(x => x.Perfil.Id == idPerfil);

            List<MenuDto> listaRETURN = new List<MenuDto>();
            if (listaPerfilMenu != null && listaPerfilMenu.Any())
            {
                foreach (PerfilMenuDto iPerfilMenu in listaPerfilMenu)
                    listaRETURN.Add(_srvMenu.Get(iPerfilMenu.Menu.id));
            }

            return listaRETURN;
        }
        #endregion --- Métodos de MENU ---

        #region --- Métodos de MENU ITEMS ---
        [HttpGet("/api/MenuController/GetMenuItem/{id}")]
        public MenuItemDto GetMenuItem(int id)
        {
            _srvMenuItem = new ServiceMenuItem();
            return _srvMenuItem.Get(id);
        }

        [HttpPost("/api/MenuController/SaveMenuItem")]
        public int SaveMenuItem(MenuItemDto menuItem)
        {
            _srvMenuItem = new ServiceMenuItem();
            _srvMenuItem.AddNew(menuItem);
            return menuItem.id;
        }

        [HttpPut("/api/MenuController/ModifyMenuItem")]
        public void ModifyMenuItem(MenuItemDto menuItem)
        {
            _srvMenuItem = new ServiceMenuItem();
            _srvMenuItem.Modify(menuItem);
        }

        [HttpDelete("/api/MenuController/DeleteMenuItem/{id}")]
        public bool DeleteMenuItem(int id)
        {
            _srvMenuItem = new ServiceMenuItem();
            return _srvMenuItem.Delete(id);
        }

        [HttpGet("/api/MenuController/GetAllMenuItem")]
        public List<MenuItemDto> GetAllMenuItem()
        {
            _srvMenuItem = new ServiceMenuItem();
            return _srvMenuItem.GetAll();
        }
        #endregion --- Métodos de MENU ITEMS ---

        #region --- Métodos de MENU GROUPS ---
        [HttpGet("/api/MenuController/GetMenuGroup/{id}")]
        public MenuGroupDto GetMenuGroup(int id)
        {
            _srvMenuGroup = new ServiceMenuGroup();
            return _srvMenuGroup.Get(id);
        }

        [HttpPost("/api/MenuController/SaveMenuGroup")]
        public int SaveMenuGroup(MenuGroupDto menuGroup)
        {
            _srvMenuGroup = new ServiceMenuGroup();
            _srvMenuGroup.AddNew(menuGroup);
            return menuGroup.id;
        }

        [HttpPut("/api/MenuController/ModifyMenuGroup")]
        public void ModifyMenuGroup(MenuGroupDto menuGroup)
        {
            _srvMenuGroup = new ServiceMenuGroup();
            _srvMenuGroup.Modify(menuGroup);
        }

        [HttpDelete("/api/MenuController/DeleteMenuGroup/{id}")]
        public bool DeleteMenuGroup(int id)
        {
            _srvMenuGroup = new ServiceMenuGroup();
            return _srvMenuGroup.Delete(id);
        }

        [HttpGet("/api/MenuController/GetAllMenuGroup")]
        public List<MenuGroupDto> GetAllMenuGroup()
        {
            _srvMenuGroup = new ServiceMenuGroup();
            return _srvMenuGroup.GetAll();
        }
        #endregion --- Métodos de MENU GROUPS ---

        #region --- Métodos de MENU COLUMNS ---
        [HttpGet("/api/MenuController/GetMenuColumn/{id}")]
        public MenuColumnDto GetMenuColumn(int id)
        {
            _srvMenuColumn = new ServiceMenuColumn();
            return _srvMenuColumn.Get(id);
        }

        [HttpPost("/api/MenuController/SaveMenuColumn")]
        public int SaveMenuColumn(MenuColumnDto menuColumn)
        {
            _srvMenuColumn = new ServiceMenuColumn();
            _srvMenuColumn.AddNew(menuColumn);
            return menuColumn.id;
        }

        [HttpPut("/api/MenuController/ModifyMenuColumn")]
        public void ModifyMenuColumn(MenuColumnDto menuColumn)
        {
            _srvMenuColumn = new ServiceMenuColumn();
            _srvMenuColumn.Modify(menuColumn);
        }

        [HttpDelete("/api/MenuController/DeleteMenuColumn/{id}")]
        public bool DeleteMenuColumn(int id)
        {
            _srvMenuColumn = new ServiceMenuColumn();
            return _srvMenuColumn.Delete(id);
        }

        [HttpGet("/api/MenuController/GetAllMenuColumn")]
        public List<MenuColumnDto> GetAllMenuColumn()
        {
            _srvMenuColumn = new ServiceMenuColumn();
            return _srvMenuColumn.GetAll();
        }
        #endregion --- Métodos de MENU COLUMNS ---
    }
}