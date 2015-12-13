(function () {
    var app = angular.module("app", ["ngAnimate", "angular-flot", "widget", "customFilters"]);

    app.service('sharedProperties', function () {
        var property = '0';
        
        return {
            getSiteId: function () {
                return property;
            },
            setSiteId: function (value) {
                console.log("Set site Id" + value);
                property = value;
            }
        };
    })

    app.factory('sharedFunctions', function () {

        return {
            foo: function ($scope) {
                console.log($scope.monthSpanOptions);
            }
        }
    });

    app.filter('padNumber', function () {
        return function (n, len) {
            var num = parseInt(n, 10);
            len = parseInt(len, 10);
            if (isNaN(num) || isNaN(len)) {
                return n;
            }
            num = '' + num;
            while (num.length < len) {
                num = '0' + num;
            }
            return num;
        };
    });

}());
