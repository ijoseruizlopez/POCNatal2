// Definición del View Model
var ViewModel = {
    el: '#paisTemplate',
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

        modal: {
            Id: -1,
            Nombre: ''
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
                url: _endpoint.Pais.GetAll,
                success: function (data) {
                    console.log(data);
                    self.data = data;
                }
            });
        }
    },
    mounted: function () {
        var self = this;
        self.loadTable(self);
    }
};

// Instanciamos el View Model
var vm = new NF(ViewModel);