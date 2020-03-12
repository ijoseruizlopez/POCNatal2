// Definición del View Model
var ViewModel = {
    el: '#ciudadTemplate',
    lang: 'es-AR',
    data: {
        data: [],
        configColumns: configColumns,
        select: NF.Table.Select.SINGLE,
        toolbar: '#myToolbar',
        mode: NF.Table.Mode.FULL,
        responsive: false,
        selection: null,
        isEditing: false,

        paisSource: [],
        provinciaSource: [],
        selectedPais: null,
        selectedProvince: null,

        modal: {
            Id: -1,
            Nombre: '',
            Provincia: {
                Id: -1,
                Nombre: "",
                Pais: {
                    Id: -1,
                    Nombre: ""
                }
            }
        }
    },
    mixins: [modalMixin, crudMixin, tableMixin],
    watch: {

    },
    methods: {
        loadEntities(data) {
            var entitys = [];
            for (var i = 0; i < data.length; i++) {
                var entity = { id: 0, text: "" };
                entity.id = data[i].Id;
                entity.text = data[i].Nombre;
                entitys.push(entity);
            }
            return entitys;
        },
        loadTable(self) {
            $.ajax({
                type: "GET",
                dataType: "json",
                async: false,
                url: _endpoint.Ciudad.GetAll,
                success: function (data) {
                    console.log(data);
                    self.data = data;
                }
            });
        },
        loadPaises(self) {
            $.ajax({
                type: "GET",
                dataType: "json",
                async: false,
                url: _endpoint.Pais.GetAll,
                success: function (data) {
                    self.paisSource = self.loadEntities(data);
                }
            });
        }
    },
    mounted: function () {
        var self = this;
        self.loadTable(self);
        self.loadPaises(self);
    }
};

// Instanciamos el View Model
var vm = new NF(ViewModel);