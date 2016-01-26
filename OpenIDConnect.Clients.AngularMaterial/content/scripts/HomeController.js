angular.module('CartoonsApp')
.controller('HomeController', ['$scope', '$location', function ($scope, $location) {
    $scope.go = function (destination) {
        $location.path(destination);
    }
    console.log("Loaded this con");

}]);