(function () {

    var msgsource = function ($http) {

        var getMessages = function () {
            var msgApi = "/api/messagesZ/1";
            return $http.get(msgApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getCommon = function () {
            var msgApi = "/api/common";
            return $http.get(msgApi)
                .then(function (response) {
                    return response.data;
                });
        };

        return {
            getMessages: getMessages,
            getCommon: getCommon
        };

    };
    var module = angular.module("message");
    module.factory("msgsource", msgsource);

}());