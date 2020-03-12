var tableMixin = {
    methods: {
        selectedRow() {
            this.$nextTick(() => {
                this.selection = this.$refs.ABMtable.getSelected();

                if (this.selection.length === 0) {
                    this.modal = {
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
                } else {
                    this.modal = this.selection[0];
                }
            });
        },

        updateSelectedRow() {
            this.$nextTick(() => {
                this.selection = this.$refs.ABMtable.getSelected();
            });
        },

        createdRow: function (row, data) {
            if (data.percentage >= 40 && data.percentage < 60) {
                $(row).addClass('success');
            }

            if (data.percentage >= 60 && data.percentage < 80) {
                $(row).addClass('warning');
            }

            if (data.percentage >= 80) {
                $(row).addClass('danger');
            }
        }
    }
}