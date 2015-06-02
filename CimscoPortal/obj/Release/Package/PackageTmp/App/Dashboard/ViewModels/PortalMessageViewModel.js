(function () {

    var module = angular.module("portalEnvironment");

    var PortalMessageViewModel = function ($scope, msgsource) {

        var onRepo = function (data) {
            $scope.msgs = data;
        };

        var onError = function (reason) {
            $scope.reason = reason;
        };

        var onCommonRepo = function (data) {
            $scope.common = data;
        };

        var onCommonError = function(reason) {
            $scope.reason = reason;
        };

        msgsource.getMessages()
            .then(onRepo, onError);

        msgsource.getCommon()
            .then(onCommonRepo, onCommonError);
    };

    module.controller("PortalMessageViewModel", PortalMessageViewModel);

}());