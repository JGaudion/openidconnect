(function () {
    'use strict';

    angular.module("CartoonsApp")
        .controller("LoginController", LoginController);

    LoginController.$inject = ["ErrorService", "$location", "TokenManager"]; //dependencies we need inside the Login Controller
    function LoginController(ErrorService, $location, TokenManager) {
        var vm = this;
        
        vm.login = LoginMethod;
        vm.logout = LogoutMethod;

        //Login the user by redirecting to get the token
        function LoginMethod() {
            console.log("Hi!");
            ErrorService.clear();
            try{
                TokenManager.redirectForToken();
            }
            catch (err)
            {
                Console.log(err);
                ErrorService.show(err);
            }
        }
        //Logout the user by removing the token
        function LogoutMethod() {
            console.log("Bye5!");            
            ErrorService.clear();
            TokenManager.removeToken();
            TokenManager.redirectForLogout();
        }
    }

   
})();
