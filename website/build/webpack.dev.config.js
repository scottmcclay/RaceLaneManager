const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const WriteFileWebpackPlugin = require('write-file-webpack-plugin');
const webpack = require('webpack');

module.exports = {
    entry: {
        app: [
            'webpack-dev-server/client?http://localhost:8080',
            './src/rlm-app/rlm-app.index'
        ]
    },
    mode: 'development',
    devtool: 'inline-source-map',
    devServer: {
        contentBase: path.resolve(__dirname, '../../bin/Debug/website'),
        hot: true
    },
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, '../../bin/Debug/website')
    },
    watchOptions: {
        ignored: ['node_modules', /\.hot-update.json$/]
    },
    module: {
        rules: [
            {
                test: /\.html$/,
                use: {
                    loader: 'html-loader'
                }
            },
            {
                test: /\.ts$/,
                use: 'ts-loader',
                exclude: /node_modules/
            }
        ]
    },
    resolve: {
        extensions: ['.ts', '.js']
    },
    plugins: [
        new CleanWebpackPlugin(['website'], {verbose: true, root: path.resolve(__dirname, "../../bin/Debug")}),
        new webpack.ProvidePlugin({
            $: 'jquery',
            'window.jQuery': 'jquery',
            jQuery: 'jquery',
            jquery: 'jquery'
        }),
        new HtmlWebpackPlugin({
            template: './index.html'
        }),
        //new WriteFileWebpackPlugin(),
        new CopyWebpackPlugin([
            { from: path.resolve(__dirname, '../node_modules/@webcomponents/webcomponentsjs/webcomponents-loader.js') },
            { from: path.resolve(__dirname, '../images'), to: 'images' },
            { from: path.resolve(__dirname, '../manifest.json') }
        ]),
        new webpack.HotModuleReplacementPlugin()
    ]
};