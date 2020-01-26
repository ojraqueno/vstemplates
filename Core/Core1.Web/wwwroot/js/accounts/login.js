import Vue from 'vue';
import * as VeeValidate from 'vee-validate';
import axios from 'axios';
import _ from 'lodash';
import redirect from '../shared/redirect';
import * as $ from 'jquery';

Vue.use(VeeValidate);
Vue.config.devtools = true;

var vm = new Vue({
    el: '#accounts-login',
    data: {
        isBusy: false,
        error: '',
        shoudShowRegisterSuccessMessage: true,
        user: {
            email: '',
            password: '',
            rememberMe: false,
            returnUrl: $('#ReturnUrl').val()
        }
    },
    methods: {
        onSubmit: function () {
            this.$validator.validateAll()
                .then(function (result) {
                    if (result) {
                        this.error = '';
                        this.isBusy = true;

                        axios.post('/api/accounts/login', this.user)
                            .then(function (response) {
                                redirect.redirectToPath(response.data.returnUrl);
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
    }
});