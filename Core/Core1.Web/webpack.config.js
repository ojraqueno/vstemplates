let path = require('path');
const VueLoaderPlugin = require('vue-loader/lib/plugin');
  
module.exports = {
    mode: 'development',
    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader'
            },
            // this will apply to both plain `.js` files
            // AND `<script>` blocks in `.vue` files
            {
                test: /\.js$/,
                loader: 'babel-loader'
            },
            // this will apply to both plain `.css` files
            // AND `<style>` blocks in `.vue` files
            {
                test: /\.css$/,
                use: [
                    'vue-style-loader',
                    'css-loader'
                ]
            },
            // the url-loader uses DataUrls.
            // the file-loader emits files.
            {
                test: /\.woff2?(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                // Limiting the size of the woff fonts breaks font-awesome ONLY for the extract text plugin
                // loader: "url?limit=10000"
                use: "url-loader"
            },
            {
                test: /\.(ttf|eot|svg)(\?[\s\S]+)?$/,
                use: 'file-loader'
            }
        ]
    },
    entry: {
        'filters': './wwwroot/js/shared/filters.js',
        'layout': './wwwroot/js/shared/layout.js',
        'accounts-login': './wwwroot/js/accounts/login.js',
        'accounts-register': './wwwroot/js/accounts/register.js',
        'accounts-forgot-password': './wwwroot/js/accounts/forgot-password.js',
        'accounts-reset-password': './wwwroot/js/accounts/reset-password.js',
    },
    output: {
        path: path.resolve('./wwwroot/dist'),
        filename: '[name].bundle.js'
    },
    plugins: [
        new VueLoaderPlugin()
    ],
    resolve: {
        alias: {
            'vue$': 'vue/dist/vue.esm.js'
        },
        extensions: ['*', '.js', '.vue', '.json']
    }
};
