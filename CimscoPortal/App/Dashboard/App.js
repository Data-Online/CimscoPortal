(function () {
    var dashboardModule = angular.module("dashboard", ["ngAnimate"]);

    dashboardModule.service('sharedProperties', function () {
        var property = '0';
        return {
            getSiteId: function () {
                return property;
            },
            setSiteId: function (value) {
                property = value;
            }
        };
    })

}());
