'use strict';

var TokenInformationService = function ($http, OidcService){
    var self = this;

    self.get = function () {
        return $http(
            {
                method: 'GET',
                url: '/api/info',
                headers: { 'Authorization': 'Bearer ' + OidcService.tokenManager.access_token }
            }).then(function (response) {
                return response.data;
            });
        }

        return self;
    }

angular
    .module('CartoonsApp')
    .factory('TokenInformationService', TokenInformationService);

module.exports = TokenInformationService;
