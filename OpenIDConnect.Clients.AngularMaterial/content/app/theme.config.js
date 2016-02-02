'use strict';

var themeConfig = function ($mdThemingProvider) {
    $mdThemingProvider
        .theme('default') //defines this as the default one to use throughout my app
        .primaryPalette('teal') //my main colour scheme
        .accentPalette('indigo', {'default': '300'}); //For contrast
};

angular.module('CartoonsApp').config(themeConfig);

module.exports = themeConfig;
