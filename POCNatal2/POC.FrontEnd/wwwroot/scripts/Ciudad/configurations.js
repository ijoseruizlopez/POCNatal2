// Configuración de las columnas de la tabla
var configColumns = [
    {
        title: "ID",
        field: "Id",
        visible: true
    },
    {
        title: "Ciudad",
        field: "Nombre",
        visible: true,
        sort: 'asc'
    },
    {
        title: "Provincia",
        field: "Provincia.Nombre",
        visible: true,
        sort: 'asc'

    },
    {
        title: "Pais",
        field: "Provincia.Pais.Nombre",
        visible: true,
        sort: 'asc'
    }
];