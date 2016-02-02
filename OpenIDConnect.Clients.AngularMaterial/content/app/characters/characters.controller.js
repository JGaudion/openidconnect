'use strict';

var CharactersController = function (CharactersService){
    var vm = this;

    CharactersService.get()
    .then(function(data){
        vm.characters = data;
    })
    .catch(function(err){
        vm.error = err.statusText;
    })

    return vm;
};

angular
    .module('CartoonsApp')
    .controller('CharactersController', CharactersController);

module.exports = CharactersController;
