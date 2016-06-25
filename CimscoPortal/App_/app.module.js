(function () {
    var app = angular.module("app", [

        "app.core",
        "widget",

        /* Specific To Company Overview area - move */
        "customFilters",
        

        /* Feature areas*/
       
        "app.dashboard",
        "app.detailBySite",
        "app.siteOverview"

    ]);



    app.factory('commonTools', function () {
        var root = {};
        root.arrowType = function (value) {
            if (value < 0.00) {
                return 'fa-arrow-down';
            }
            else if (value > 0.00) {
                return 'fa-arrow-up';
            }
            else {
                return 'fa-arrows-h';
            }
        };
        return root;
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
