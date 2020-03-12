var modalMixin = {

    mounted: function () {
        console.info("MOUNTED desde Mixin");
    },

    created() {
        console.info("CREATED desde Mixin");
    },

    watch: {
        async selectedPais (newVal) {
            if (newVal)
                await this.FiltroProvinciaPorPais(newVal);   
        },
        async selectedProvince (newVal) {
            if (!newVal) {
                this.localidadSource = [];
                return;
            }
            else
                await this.FiltroCiudadPorProvincia(newVal);
        }
    },

    methods: {

        async FiltroProvinciaPorPais(newVal) {

            var self = this;
            await $.ajax({
                type: "GET",
                dataType: "json",
                url: _endpoint.Provincia.GetByPais + newVal.id,
                success: function (data) {
                    self.provinciaSource = self.ModificacionVariables(data);
                }
            });
        },

        async FiltroCiudadPorProvincia(newVal) {
            var self = this;
            await $.ajax({
                type: "GET",
                dataType: "json",
                url: _endpoint.Ciudad.GetByProvincia + newVal.id,
                success: function (data) {
                    self.localidadSource = self.ModificacionVariables(data);
                }
            });
        },

        async SaveForm() {
            var self = this;

            try {
                if (NF.Validation.Form.validate('#modalForm')) {
                    NF.UI.Page.block();
                    //Cargamos el objeto modal con lo que tenemos en los combos
                    this.PopulateModal();
                    if (this.isEditing) {
                        //Recuperamos el objeto
                        var entityJson = JSON.stringify(this.modal);
                        await $.ajax({
                            url: _endpoint.Usuario.Update,
                            type: "PUT",
                            data: entityJson,
                            processData: true,
                            contentType: "application/json;chartset=utf-8",
                        })
                        .done(function () {
                            self.CargarTablaUsuarios();
                        });

                    } else {

                        //Seteamos el id en -1 para poder hacer el alta
                        this.modal.Id = -1;

                        //Convertimos a Json el objeto
                        var dataJson = JSON.stringify(this.modal);

                        //Consumimos API de de Guardado
                        await $.ajax({
                            url: _endpoint.Usuario.Create,
                            type: "POST",
                            data: dataJson,
                            processData: true,
                            contentType: "application/json;chartset=utf-8",
                            success: function (data) {
                                //Recargamos la tabla en caso de exito
                                self.CargarTablaUsuarios();
                            }
                        });

                    }
                    this.$refs.modal.close();
                }
            } catch (e) {
                console.error(e);

            } finally {
                NF.UI.Page.unblock();
            }
        },

        CloseModal() {
            NF.Validation.Form.reset("#modalForm");
        },

        PopulateModal() {

            this.modal.Ciudad.Id = this.selectedLocality.id;
            this.modal.Ciudad.Nombre = this.selectedLocality.text;

            this.modal.Ciudad.Provincia.Id = this.selectedProvince.id;
            this.modal.Ciudad.Provincia.Nombre = this.selectedProvince.text;

            this.modal.Ciudad.Provincia.Pais.Id = this.selectedPais.id;
            this.modal.Ciudad.Provincia.Pais.Nombre = this.selectedPais.text;
        }
    }
}