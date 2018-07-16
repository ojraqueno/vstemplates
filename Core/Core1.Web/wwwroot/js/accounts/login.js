import Vue from 'vue';
import VeeValidate from 'vee-validate';

Vue.use(VeeValidate);

var vm = new Vue({
    el: '#accounts-login',
    data: {
        //error: '',
        //values: []
    },
    //mounted: function () {
    //    console.log('mounted');

    //    axios.get('api/values/search')
    //        .then(function (response) {
    //            this.values = response.data;
    //        }.bind(this))
    //        .catch(function () {
    //            this.error = 'error';
    //        }.bind(this));
    //}
});