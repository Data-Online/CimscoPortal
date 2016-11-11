(function () {
    var userDataSource = function ($http, toaster) {

        var getUserData = function () {
            console.log("Getting user data");
            var dataApi = "/api/userdata";
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var saveUserData = function (userSettings) {
            console.log(userSettings);
            console.log(JSON.stringify(userSettings));
            var dataApi = "/api/saveUserData";
            var config = {
                headers: {
                    'Content-Type': 'application/json'
                }
            }
            return $http.post(dataApi, JSON.stringify(userSettings), config)
                        .then(function (response) {
                            return response; //.data;
                        });
        };

        var updateUserData = function (userData, setting, value) {
            var debugStatus_showMessages = true; // GPA - --> global variables
            userData = returnValueForSetting(userData, setting, value);
            if (userData) {
                saveUserData(userData)
                    .then(
                    function success() { if (debugStatus_showMessages) { toaster.pop('success', "User data saved", "") }; },
                    function error() { toaster.pop('error', "Unable to save data", " user settings");  }
                    );
            }
            else {
                if (debugStatus_showMessages) { toaster.pop('error', "No user data available", "") };
            }

            return userData;
        };

        var returnValueForSetting = function (userData, setting, value) {
            switch (setting) {
                case 'welcomeMsg':
                    userData.ShowWelcomeMessage = false;
                    break;
                case 'monthSpan':
                    userData.MonthSpan = value;
                    break;
                default:
            }
            console.log(userData);
            return userData;
        };

        var assignUserData = function (scope, userData) {
            scope.monthSpanOptions = userData.monthSpanOptions;
            scope.monthSpan = userData.monthSpan;
            scope.showWelcomeMessage = userData.showWelcomeMessage;
        };


        return {
            getUserData: getUserData,
            updateUserData: updateUserData,
            assignUserData: assignUserData
        };

    };
    var module = angular.module("app.core");
    module.factory("userDataSource", userDataSource);
    userDataSource.$inject = ['$http', 'toaster'];


}());