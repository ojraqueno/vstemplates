import Vue from 'vue';
import moment from 'moment';

Vue.filter('date', function (value, format) {
    if (value && format) {
        return moment(String(value)).format(format);
    }
});