(function() {
  'use strict';

  angular
    .module('src')
    .controller('HomeController', HomeController);

  /** @ngInject */
  function HomeController($timeout, toastr) {
    var vm = this;
  }
})();
