import Vue from 'vue';
import VeeValidate from 'vee-validate';
import { Validator } from 'vee-validate';
import axios from 'axios';
import _ from 'lodash';
import redirect from '../shared/redirect';

Vue.use(VeeValidate);
Vue.config.devtools = true;

const dict = {
    custom: {
        agreedToTerms: {
            required: 'Please agree to the terms and conditions.'
        }
    }
};

Validator.localize('en', dict);

var vm = new Vue({
    el: '#accounts-register',
    data: {
        error: '',
        isBusy: false,
        plans: [],
        user: {
            agreedToTerms: false,
            email: '',
            name: '',
            password: '',
            timezoneOffsetMinutes: new Date().getTimezoneOffset()
        }
    },
    methods: {
        onSubmit: function () {
            this.$validator.validateAll()
                .then(function (result) {
                    if (result) {
                        this.error = '';
                        this.isBusy = true;

                        axios.post('/api/accounts/register', this.user)
                            .then(function () {
                                redirect.redirectToAction('Login', 'Accounts', { fromRegistration: true });
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