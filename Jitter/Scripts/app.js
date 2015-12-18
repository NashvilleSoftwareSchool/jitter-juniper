var app = angular.module('jitter', []);

app.controller('Controller', ["$scope", "$http", function ($scope, $http) {
    $scope.test = "test variable";

    $scope.deleteUsers = function () {
        $http.delete("/api/Test")
            .success(function (data) {
                alert("Deleting Users Yay!");
            })
            .error(function (error) { alert(error.error) });
    }

    $scope.hello = function () {
        $http.get("/api/Test")
            .success(function (data) {
                $scope.test = data;})
            .error(function (error) { alert(error.error) });
    }
}]);