var modalMixin = {
    mounted: function () {
        // Form validation espera callback globales
        window.checkPaisSelected = function () {
            return Boolean(this.selectedPais);
        }.bind(this);

        window.checkProvinceSelected = function () {
            return Boolean(this.selectedProvince);
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

            this.loadProvincesByCountry(newVal);
        },
        selectedProvince: function (newVal) {
            if (newVal === null)
                return;

            if (!this.isEditing) {
                this.selectedProvince = null;
                this.provinciaSource = [];
            }

            if (vm.provinciaSource != null && vm.provinciaSource.length > 0) {
                this.isEditing = true;
            }
        }
    },

    methods: {
        formSubmit() {
            if (NF.Validation.Form.validate('#modalForm')) {
                this.modal.Provincia.Id = this.selectedProvince.id;
                this.modal.Provincia.Nombre = this.selectedProvince.text;

                this.modal.Provincia.Pais.Id = this.selectedPais.id;
                this.modal.Provincia.Pais.Nombre = this.selectedPais.text;

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
                url: _endpoint.Ciudad.Update,
                type: "PUT",
                data: jsonData,
                processData: true,
                contentType: "application/json;chartset=utf-8",
            }).done(function () {
                var rowIndex = vm.selection[0].nfuuid;
                if (rowIndex !== null && rowIndex !== undefined && vm.modal !== null && vm.modal !== undefined) {
                    vm.data[rowIndex].Nombre = vm.modal.Nombre;
                    vm.data[rowIndex].Provincia.Nombre = vm.modal.Provincia.Nombre;
                    vm.data[rowIndex].Provincia.Pais.Nombre = vm.modal.Provincia.Pais.Nombre;
                }
            });
        },
        Create(jsonData) {
            $.ajax({
                url: _endpoint.Ciudad.Create,
                type: "POST",
                data: jsonData,
                processData: true,
                contentType: "application/json;chartset=utf-8",
                success: function (data) {
                    console.log(data);
                }
            });
        },
        loadProvincesByCountry(newVal) {
            console.log(newVal);
            $.ajax({
                type: "GET",
                dataType: "json",
                async: false,
                url: _endpoint.Provincia.GetByPais + newVal.id,
                success: function (data) {
                    vm.provinciaSource = vm.loadEntities(data);
                }
            });
        }
    }
}