(function () {

    var module = angular.module("portalEnvironment");

    var dbCompanyDetails = function ($scope, coDataSource) {

        var onRepo = function (data) {
            $scope.data = data;
        };

        var onError = function (reason) {
            $scope.reason = reason;
        };

        coDataSource.getCompanyTree()
            .then(onRepo, onError);
    };

    module.controller("dbCompanyDetails", dbCompanyDetails);


}());