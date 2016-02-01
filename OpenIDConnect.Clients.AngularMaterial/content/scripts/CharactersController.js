(function(){
    'use strict';

    angular
        .module('CartoonsApp')
        .controller('CharactersController', CharactersController);

        CharactersController.$inject = ['CharactersService'];

        function CharactersController(CharactersService){
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
})();
