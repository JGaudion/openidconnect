(function() {
  'use strict';

  angular
    .module('src')
    .directive('acmeNavbar', acmeNavbar);

  /** @ngInject */
  function acmeNavbar() {
    var directive = {
      restrict: 'E',
      templateUrl: 'app/components/navbar/navbar.html',
      scope: {
      },
      controller: NavbarController,
      controllerAs: 'vm',
      bindToController: true
    };

    return directive;

    /** @ngInject */
    function NavbarController(moment, $log, oidc) {
      var vm = this;

      vm.signIn = function () {
          debugger;
          var mgr = oidc.get();

          if (mgr.expired) {
              mgr.redirectForToken();
          }
      }
    }
  }

})();
