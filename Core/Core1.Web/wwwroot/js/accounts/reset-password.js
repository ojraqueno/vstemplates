import Vue from 'vue';
import * as VeeValidate from 'vee-validate';
import axios from 'axios';
import _ from 'lodash';

Vue.use(VeeValidate);
Vue.config.devtools = true;

var vm = new Vue({
    el: '#accounts-reset-password',
    data: {
        isBusy: false,
        error: '',
        message: '',
        succeeded: false,
        user: {
            code: '',
            email: '',
            password: ''
        }
    },
    methods: {
        onSubmit: function () {
            this.$validator.validateAll()
                .then(function (result) {
                    if (result) {
                        this.error = '';
                        this.isBusy = true;

                        axios.post('/api/accounts/resetPassword', this.user)
                            .then(function (response) {
                                if (response.data && response.data.succeeded === true) {
                                    this.message = 'Password reset successfully.';
                                    this.succeeded = true;
                                }
                                else {
                                    this.error = 'Something went wrong. Please try again later.';
                                }

                                this.isBusy = false;
                            }.bind(this))
                            .catch(function (error) {
                                this.__.handleHttpError(this, error);

                                this.isBusy = false;
                            }.bind(this));
                    }
                }.bind(this));
        }
    },
    created: function () {
        this.__ = {
            handleHttpError: function (vm, error) {
                if (error.response && error.response.status === 400) {
                    var serverValidationErrors = error.response.data;

                    vm.error = _.first(serverValidationErrors[Object.keys(serverValidationErrors)[0]]);
                }
                else {
                    vm.error = 'Unable to complete action. Please try again later.';
                }
            }
        };
    },
    mounted() {
        var model = JSON.parse(document.getElementById('Model').innerHTML);
        this.user.code = model.code;
    }
});