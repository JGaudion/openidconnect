'use strict';

var CartoonsService = function ($http, OidcService) {
    var self = this;

    self.get = function () {
        return $http(
            {
                method: 'GET',
                url: '/api/cartoons',
                headers: { 'Authorization': 'Bearer ' + OidcService.tokenManager.access_token }
            }).then(function (response) {
                return response.data;
            });
    }

    return self;
};

angular
    .module('CartoonsApp')
    .factory('CartoonsService', CartoonsService);

module.exports = CartoonsService;
