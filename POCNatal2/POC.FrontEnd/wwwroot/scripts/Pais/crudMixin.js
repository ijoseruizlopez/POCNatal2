var crudMixin = {
    methods: {
        Nuevo: function () {
            this.selection = null;
            vm.Component.find('nf-table')[0].deselectAll();

            this.modal = {
                Id: '',
                Nombre: ''
            };

            this.$refs.modal.open();
        },

        Detalle() {
            this.$refs.detailModal.open();
        },

        Editar() {
            if (this.selection !== null && this.selection !== undefined) {
                this.isEditing = false;
                this.$refs.modal.open();
            }
        },

        Eliminar() {
            $.ajax({
                url: _endpoint.Pais.Delete + this.selection[0].Id,
                type: "DELETE",
                contentType: "application/json;chartset=utf-8",
            }).done(function () {
                var selectedIds = vm.$data.selection.map(function (entity) {
                    return entity.Id;
                });

                vm.$data.data = vm.$data.data.filter(function (entity) {
                    return selectedIds.indexOf(entity.Id) === -1;
                });

                vm.updateSelectedRow();
            });
        }
    }
}