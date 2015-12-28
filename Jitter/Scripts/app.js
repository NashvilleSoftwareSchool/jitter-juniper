var app = angular.module('jitter', []);

app.controller('Controller', ["$scope", "$http", function ($scope, $http) {
    $scope.test = "test variable";


    $scope.postJot = function () {
        $form = $("#myform").first();
        
        $jot = {
            "Content": $("#formContent").val(),
            "Date": $("#formDate").val()
        };
        console.log($jot);
        $config_obj = {
            'headers': {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
                'X-Jurnell': 'I am here'
            }
        };

        $http.post("/api/Test", $jot,$config_obj)
            .success(function (data) {
                alert("Posted! Hopefully");
            })
            .error(function (error) { alert(error.error) });
    }


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