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
        "easypiechart",

        "googlechart",

        "nvd3ChartDirectives",

        "sparkline",

        /* Tooltip for progress bars (.tooltip() or .popover() */
        "tooltip",

        /* Factory for Google Chart control */
        "googleChartControl",
        //"googleChartDirectives",

        /* Common Data control factory */
        "commonDataControl",

        /* Feedback Tab */
        "angular-send-feedback",

        "ngSanitize"
    ])
   // .constant("debugStatus", { "showMessages": false });
})();
