import Vue from 'vue';
import VeeValidate from 'vee-validate';
import axios from 'axios';
import _ from 'lodash';
import redirect from '../shared/redirect';

Vue.use(VeeValidate);

var vm = new Vue({
    el: '#accounts-register',
    data: {
        isBusy: false,
        error: '',
        user: {
            email: '',
            password: ''
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

                        axios.post('/api/accounts/register', this.user)
                            .then(function () {
                                redirect.redirectToAction('Login', 'Accounts');
                            }.bind(this))
                            .catch(function (error) {
                                alert('error');
                                console.log(error);
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