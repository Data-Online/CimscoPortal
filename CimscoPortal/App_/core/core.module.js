(function () {
    'use strict';

    angular.module('app.core', [
        /* Angular modules*/
        "ngAnimate",

        "ui.bootstrap",

        /* User Data and Messaging */
         "app.messaging",

        /* 3rd party graphs: Angular directives */
        "angular-flot",
        "tc.chartjs",
        "angularjs-dropdown-multiselect",
        "toaster",
        "easypiechart"
    ]);
})();
