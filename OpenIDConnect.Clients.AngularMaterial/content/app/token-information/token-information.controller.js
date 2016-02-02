'use strict';

var TokenInformationController = function (TokenInformationService){
    var self = this;

    TokenInformationService.get()
    .then(function(data){
        self.tokenInformation = JSON.stringify(data, null, 4);
    })
    .catch(function(err){
        self.error = err;
    });

    return self;
};

angular
    .module('CartoonsApp')
    .controller('TokenInformationController', TokenInformationController);

module.exports = TokenInformationController;
