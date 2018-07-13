let path = require('path');
  
module.exports = {
    mode: 'development',
    entry: {
        'accounts-login': './wwwroot/js/accounts/login.js',
        'accounts-register': './wwwroot/js/accounts/register.js'
    },
    output: {
        path: path.resolve('./wwwroot/dist'),
        filename: '[name].bundle.js'
    },
    resolve: {
        alias: {
            'vue$': 'vue/dist/vue.esm.js'
        }
    }
}
