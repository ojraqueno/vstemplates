import Vue from 'vue';

var vm = new Vue({
    el: '#navbar',
    data: {
        isActive: false
    },
    methods: {
        toggleActive: function () {
            this.isActive = !this.isActive;
        }
    }
});