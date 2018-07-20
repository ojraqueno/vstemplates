import Vue from 'vue';
import VeeValidate from 'vee-validate';
import axios from 'axios';
import _ from 'lodash';
import redirect from '../shared/redirect';
import * as $ from 'jquery';

Vue.use(VeeValidate);

var vm = new Vue({
    el: '#accounts-login',
    data: {
        isBusy: false,
        error: '',
        user: {
            email: '',
            password: '',
            returnUrl: $('#ReturnUrl').val()
        }
    },
    methods: {
        onDeleteNotification: function () {
            this.error = '';
        },
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
                                if (error.response && error.response.status === 400) {
                                    var serverValidationErrors = error.response.data;

                                    this.error = _.first(serverValidationErrors[Object.keys(serverValidationErrors)[0]]);
                                }
                                else {
                                    this.error = 'Unable to complete action. Please try again later.';
                                }

                                this.isBusy = false;
                            }.bind(this));
                    }
                }.bind(this))
        }
    }
});