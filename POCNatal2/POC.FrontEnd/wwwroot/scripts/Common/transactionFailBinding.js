
// Definición del View Model
var ViewModel = {
    el: '#TransactionServiceFail',
    lang: 'es-AR',
    data: {

    },

    watch: {

    },

    mounted: function () {
      
    },
    busEvents: {
        'OpenModalTransactionFail': function () {
            this.OpenModal();
        }
    },

    methods: {
        OpenModal: function () {
            this.$refs.modal.open();
        },
        VolverAIntentar: function () {
            this.Bus.$emit('DatosDeTransactioFail', "DATOS EXTRAS");
            this.$refs.modal.close();
        },

        Closed: function () {
      
        },

        WriteUs: function () {

        }
    }
};

// Instanciamos el View Model
var vmT = new NF(ViewModel);