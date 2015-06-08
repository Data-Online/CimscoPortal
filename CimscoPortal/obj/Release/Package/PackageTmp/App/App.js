(function () {
    var commonModule = angular.module("common", []);

    commonModule.controller('commonCtrl', [
        '$rootScope',
        function ($rootScope) {
            $rootScope.setttings = {
                skin: '',
                color: {
                    themeprimary: '#2dc3e8',
                    themesecondary: '#fb6e52',
                    themethirdcolor: '#ffce55',
                    themefourthcolor: '#a0d468',
                    themefifthcolor: '#e75b8d'
                }
            };
        }

    ]);
}());