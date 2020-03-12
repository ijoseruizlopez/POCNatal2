// Configuración de las columnas de la tabla
var configColumns = [
    {
        title: "ID",
        field: "Id",
        visible: true
    },
    {
        title: "Nombre",
        field: "Nombre",
        visible: true,
        sort: 'asc'

    },
    {
        title: "Ciudad",
        field: "Ciudad.Nombre",
        visible: true,
        sort: 'asc'

    },
    {
        title: "Provincia",
        field: "Ciudad.Provincia.Nombre",
        visible: true,
        sort: 'asc'
    },
    {
        title: "Pais",
        field: "Ciudad.Provincia.Pais.Nombre",
        visible: true,
        sort: 'asc'
    }
];