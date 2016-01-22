(function() {
  'use strict';

  angular.module('src')
    .controller('NewsController', NewsController);

  function NewsController(api) {
    var vm = this;

    vm.newsArticles = [];
    vm.errorMessage = "";

    getNewsArticles();

    function getNewsArticles() {
      api.get('news').then(function(response) {
        vm.newsArticles = response.data;
      }, function(error) {
        vm.errorMessage = error;
      })
    }
  }
})();
