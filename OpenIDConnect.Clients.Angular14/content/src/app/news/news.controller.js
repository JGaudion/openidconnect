(function() {
  'use strict';

  angular.module('src')
    .controller('NewsController', NewsController);

  function NewsController() {
    var vm = this;

    vm.newsArticles = [ {
      id: "1",
      title: "First Article",
      body: "This is a fascinating news article"
    }, {
      id: "2",
      title: "Second Article",
      body: "This is a less interesting news article"
    }];
  }
})();
