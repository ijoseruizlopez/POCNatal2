// Definición del View Model
var ViewModel = {
    el: '#userTemplate', // id del View Model
    lang: 'es-AR',
    data: {

        //Configuraciones de componentes
        configColumns: configColumns,
        select: NF.Table.Select.SINGLE,
        toolbar: '#myToolbar',
        mode: NF.Table.Mode.FULL,
        responsive: false,

        //Objetos que contienen el valor del componente seleccionado.
        selection: null,
        selectedLocality: null,
        selectedProvince: null,
        selectedPais: null,

        //Source de tablas o combos
        userSource: [],
        paisSource: [],
        provinciaSource: [],
        localidadSource: [],


        //Modelo que maneja el modal
        modal: {
            Id: null,
            Nombre: null,
            Ciudad: {
                Id: null,
                Nombre: null,
                Provincia: {
                    Id: null,
                    Nombre: null,
                    Pais: {
                        Id: null,
                        Nombre: null
                    }
                }
            }
        },

        //Banderas
        isEditing: false
    },
    mixins: [modalMixin],

    busEvents: {
        'DatosDeTransactioFail': function (datos) {

            NF.Notification.show({
                type: NF.Notification.Type.NOTICE,
                title: 'Noticia',
                content: 'Te invocaron desde otro view model y te mandaron: ' + datos,
                position: NF.Notification.Positions.BOTTOM_RIGHT,
                clickClose: false,
                autoHideDelay: 0
            });
        }
    },

    watch: {

    },

    mounted () {
        console.info("MOUNTED");
    },

    created() {
        console.info("CREATED");
        this.Init();
    },

    methods: {

        async Init() {
            try {
                NF.UI.Page.block();
                await Promise.all([this.CargarTablaUsuarios(), this.CargarComboPaises()]);
            } catch (e) {
                console.error(e);
            } finally {
               NF.UI.Page.unblock();
            } 
        },

        async CargarTablaUsuarios() {
            var self = this;
            await $.ajax({
                type: "GET",
                dataType: "json",
                url: _endpoint.Usuario.GetAll,
                success: function (data) {
                    self.userSource = data;
                }
            });
        },

        async CargarComboPaises() {
            var self = this;
            await $.ajax({
                type: "GET",
                dataType: "json",
                url: _endpoint.Pais.GetAll,
                success: function (data) {
                    console.table(data);
                    self.paisSource = self.ModificacionVariables(data);
                }
            });
        },

        //Metodos de la botonera
        Nuevo() {
            this.Component.find('nf-table')[0].deselectAll();

            //Limpiamos todas las selecciones
            this.selection = null;
            this.selectedLocality = null;
            this.selectedProvince = null;
            this.selectedPais = null;

            //Limpiamos los source de los combos
            this.provinciaSource = [];
            this.localidadSource = [];

            //Reinstanciamos el modal
            this.modal = {
                Id: 0,
                Nombre: '',
                Ciudad: {
                    Id: 0,
                    Nombre: '',
                    Provincia: {
                        Id: 0,
                        Nombre: '',
                        Pais: {
                            Id: 0,
                            Nombre: ''
                        }
                    }
                }
            };

            this.isEditing = false;

            this.$refs.modal.open();
        },

        Detalle() {
            this.$refs.detailModal.open();
        },

        async Editar() {
            try {
                NF.UI.Page.block();

                this.isEditing = true;

                var selectedEntity = this.modal;

                this.selectedPais = { id: selectedEntity.Ciudad.Provincia.Pais.Id, text: selectedEntity.Ciudad.Provincia.Pais.Nombre };

                await this.FiltroProvinciaPorPais(this.selectedPais);

                this.selectedProvince = { id: selectedEntity.Ciudad.Provincia.Id, text: selectedEntity.Ciudad.Provincia.Nombre };

                await this.FiltroCiudadPorProvincia(this.selectedProvince);

                this.selectedLocality = { id: selectedEntity.Ciudad.Id, text: selectedEntity.Ciudad.Nombre };

                this.$refs.modal.open();

            } catch (e) {
                console.error(e);
            } finally {
                NF.UI.Page.unblock();
            }
        },

        async Eliminar() {

            try {
                NF.UI.Page.block();
                var self = this;
                await $.ajax({
                    url: _endpoint.Usuario.Delete + this.selection[0].Id,
                    type: "DELETE",
                    contentType: "application/json;chartset=utf-8"
                })
                    .done(function () {
                        var selectedIds = self.selection.map(function (user) {
                            return user.Id;
                        });

                        self.userSource = self.userSource.filter(function (user) {
                            return selectedIds.indexOf(user.Id) === -1;
                        });

                        self.UpdateSelectedRow();
                    });

            } catch (e) {
                console.error(e);
            } finally {
                NF.UI.Page.unblock();
            }
        },

        //Metodos para el componente tabla
        SelectedRow() {
            this.$nextTick(() => {
                this.selection = this.$refs.ABMtable.getSelected();

                if (this.selection.length === 0) {
                    this.modal = {
                        Id: '',
                        Nombre: '',
                        Ciudad: {
                            Id: '',
                            Nombre: '',
                            Provincia: {
                                Id: '',
                                Nombre: '',
                                Pais: {
                                    Id: '',
                                    Nombre: ''
                                }
                            }
                        }
                    }
                } else {
                    this.modal = this.selection[0];
                }
            });
        },

        UpdateSelectedRow() {
            this.$nextTick(() => {
                this.selection = this.$refs.ABMtable.getSelected();
            });
        },

        //Metodos auxiliares
        ModificacionVariables(data) {
            var entitys = [];
            for (var i = 0; i < data.length; i++) {
                var entity = { id: 0, text: "" };
                entity.id = data[i].Id;
                entity.text = data[i].Nombre;
                entitys.push(entity);
            }
            return entitys;
        },


        //Metodo para invocar otro view model
        CallViewModel() {
            this.Bus.$emit('OpenModalTransactionFail');
        }
    }
};

// Instanciamos el View Model
var vmU = new NF(ViewModel);