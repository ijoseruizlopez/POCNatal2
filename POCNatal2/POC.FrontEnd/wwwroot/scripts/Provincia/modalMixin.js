var modalMixin = {
    mounted: function () {
        // Form validation espera callback globales
        window.checkPaisSelected = function () {
            return Boolean(this.selectedPais);
        }.bind(this);
    },

    watch: {
        selectedPais: function (newVal) {
            if (newVal === null)
                return;

            if (!this.isEditing) {
                this.selectedPais = null;
                this.paisSource = [];
            }

            if (vm.paisSource != null && vm.paisSource.length > 0) {
                this.isEditing = true;
            }
        }
    },

    methods: {
        formSubmit() {
            if (NF.Validation.Form.validate('#modalForm')) {
                this.modal.Pais.Id = this.selectedPais.id;
                this.modal.Pais.Nombre = this.selectedPais.text;

                if (this.selection[0] !== null && this.selection[0] !== undefined) {
                    this.Update(JSON.stringify(vm.modal));
                } else {
                    this.Create(JSON.stringify(vm.modal));
                }

                this.$refs.modal.close();
            }
        },
        Update(jsonData) {
            $.ajax({
                url: _endpoint.Provincia.Update,
                type: "PUT",
                data: jsonData,
                processData: true,
                contentType: "application/json;chartset=utf-8",
            }).done(function () {
                var rowIndex = vm.selection[0].nfuuid;
                if (rowIndex !== null && rowIndex !== undefined && vm.modal !== null && vm.modal !== undefined) {
                    vm.data[rowIndex].Nombre = vm.modal.Nombre;
                    vm.data[rowIndex].Pais.Nombre = vm.modal.Pais.Nombre;
                }
            });
        },
        Create(jsonData) {
            $.ajax({
                url: _endpoint.Provincia.Create,
                type: "POST",
                data: jsonData,
                processData: true,
                contentType: "application/json;chartset=utf-8",
                success: function (data) {
                    console.log(data);
                }
            });
        }
    }
}