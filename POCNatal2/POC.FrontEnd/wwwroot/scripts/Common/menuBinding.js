var items = [
    {
        name: 'Usuarios',
        link: '/Usuario/List'
    },
    {
        name: 'Paises',
        link: '/Pais/List'
    },
    {
        name: 'Provincias',
        link: '/Provincia/List'
    },
    {
        name: 'Ciudades',
        link: '/Ciudad/List'
    }
];


// Definición del View Model
var ViewModel = {
    el: '#menu',
    lang: 'es-AR',
    data: {
        items: items,
        logo: {
            src: 'https://natalfwk-pre.gruposancorseguros.com/2.3.1/media/logo_prevenet.svg',
            alt: 'Logo Prevenet'
        },
        notifications: [
            {
                icon: 'https://natalfwk.gruposancorseguros.com/2.3.1/media/notification.svg',
                title: 'Alta médico',
                text: 'Juan Perez solicita registrarse',
                isNew: true
            },
            {
                icon: 'https://natalfwk.gruposancorseguros.com/2.3.1/media/notification.svg',
                title: 'Certificados',
                text: '8 certificados creados hoy',
                isNew: true
            }
        ],
        hasFavorites: true,
        favorites: []
    },

    watch: {

    },

    mounted: function () {
      
    },


    methods: {

        onClickNotification: function (notificacion) {
            NF.Notification.show({
                type: NF.Notification.Type.INFO,
                title: 'Noticia',
                content: 'Se selecciono una notificacion que contiene el texto: ' + notificacion.text,
                position: NF.Notification.Positions.BOTTOM_RIGHT,
                clickClose: true
            });
        }
    }
};

// Instanciamos el View Model
var vmM = new NF(ViewModel);