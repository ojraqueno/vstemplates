import Vue from 'vue';
import VeeValidate from 'vee-validate';
import axios from 'axios';
import _ from 'lodash';

Vue.use(VeeValidate);
Vue.config.devtools = true;

var vm = new Vue({
    el: '#accounts-forgot-password',
    data: {
        isBusy: false,
        error: '',
        message: '',
        succeeded: false,
        user: {
            email: ''
        }
    },
    methods: {
        onSubmit: function () {
            this.$validator.validateAll()
                .then(function (result) {
                    if (result) {
                        this.error = '';
                        this.isBusy = true;

                        axios.post('/api/accounts/forgotPassword', this.user)
                            .then(function (response) {
                                if (response.data && response.data.succeeded === true) {
                                    this.message = 'Please check your email to reset your password.';
                                    this.succeeded = true;
                                }
                                else {
                                    this.error = 'Something went wrong. Please try again later.';
                                }

                                this.isBusy = false;
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
                }.bind(this));
        }
    }
});