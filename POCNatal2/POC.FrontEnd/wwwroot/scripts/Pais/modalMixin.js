var modalMixin = {
    watch: {

    },

    methods: {
        formSubmit() {
            if(NF.Validation.Form.validate('#modalForm')) {
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
                url: _endpoint.Pais.Update,
                type: "PUT",
                data: jsonData,
                processData: true,
                contentType: "application/json;chartset=utf-8",
            }).done(function () {
                var rowIndex = vm.selection[0].nfuuid;
                if (rowIndex !== null && rowIndex !== undefined && vm.modal !== null && vm.modal !== undefined) {
                    vm.data[rowIndex].Nombre = vm.modal.Nombre;
                }
            });
        },
        Create(jsonData) {
            $.ajax({
                url: _endpoint.Pais.Create,
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