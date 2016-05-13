(function () {

    var userDataSource = function ($http) {

        var getUserData = function () {
            console.log("Getting user data");
            var dataApi = "/api/userdata";
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        }

        return {
            getUserData: getUserData
        };

    };
    var module = angular.module("app.core");
    module.factory("userDataSource", userDataSource);

}());