(function(){
    'use strict';

    angular
        .module('CartoonsApp')
        .controller('TokenInformationController', TokenInformationController);

        TokenInformationController.$inject = ['TokenInformationService'];

        function TokenInformationController(TokenInformationService){
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
})();
