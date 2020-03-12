// Endpoints
var _endpoint = {
    Ciudad: {
        Create: 'https://localhost:44309/api/CiudadController/Save',
        Get: 'https://localhost:44309/api/CiudadController/Get/',
        GetAll: 'https://localhost:44309/api/CiudadController/GetAll',
        GetByProvincia: 'https://localhost:44309/api/CiudadController/GetCiudadesPorProvincia/',
        Update: 'https://localhost:44309/api/CiudadController/Modify/',
        Delete: 'https://localhost:44309/api/CiudadController/Delete/'
    },
    Pais: {
        Create: 'https://localhost:44309/api/PaisController/Save',
        Get: 'https://localhost:44309/api/PaisController/Get/',
        GetAll: 'https://localhost:44309/api/PaisController/GetAll',
        Update: 'https://localhost:44309/api/PaisController/Modify',
        Delete: 'https://localhost:44309/api/PaisController/Delete/'
    },
    Provincia: {
        Create: 'https://localhost:44309/api/ProvinciaController/Save',
        Get: 'https://localhost:44309/api/ProvinciaController/Get/',
        GetAll: 'https://localhost:44309/api/ProvinciaController/GetAll',
        GetByPais: 'https://localhost:44309/api/ProvinciaController/GetProvinciasPorPais/',
        Update: 'https://localhost:44309/api/ProvinciaController/Modify',
        Delete: 'https://localhost:44309/api/ProvinciaController/Delete/'
    },
    Usuario: {
        Create: 'https://localhost:44309/api/UserController/Save',
        Get: 'https://localhost:44309/api/UserController/Get/',
        GetAll: 'https://localhost:44309/api/UserController/GetAll',
        Update: 'https://localhost:44309/api/UserController/Modify',
        Delete: 'https://localhost:44309/api/UserController/Delete/'
    }
};