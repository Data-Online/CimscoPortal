(function () {
    'use strict';

    angular
        .module("app.messaging")
        .controller("app.messaging.ctrl", messaging);

    messaging.$inject = ['$scope', 'msgsource'];
    function messaging ($scope, msgsource) {

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

}());