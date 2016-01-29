(function () {
    'use strict';

    angular.module('CustomView')
        .directive('antiForgeryToken', antiForgeryToken);

    function antiForgeryToken() {
        return {
            restrict: 'E',
            replace: true,
            scope: {
                token: "="
            },
            template: "<input type='hidden' name='{{token.name}}' value='{{token.value}}'>"
        };
    };
})();