(function () {
    'use strict';

    angular
        .module("app.dashboard")
        .constant("dbConstants", {
            "costsDataElement": "costsBarChart",
            "consDataElement": "consBarChart",
            "filterSelectDelay": "15"
            //,"filters": ["divisions", "categories"]
        })
        .controller("app.dashboard.ctrl", dashboard);

    dashboard.$inject = ['$scope', '$parse', '$interval', '$timeout', 'dbDataSource', 'userDataSource', 'filterData', 'dbConstants', 'toaster', 'googleChart'];
    function dashboard($scope, $parse, $interval, $timeout, dbDataSource, userDataSource, filterData, dbConstants, toaster, googleChart) {

        var debugStatus_showMessages = true;
        $scope.showAsBar = false;  // Whether to show line or bar initiually. Dictates setting for toggle switch

        // elemenName == Page element where chart is to be reendered
        // columnNames == [Primary, Seconday] axes for chart. Primary axis is displayed by default. These names need to match columns returned from server.
        // title == Title to render on page (if required)
        // activeAxes == Which of the 2 axes are displayed. Current suppor is for Primary, !Secondary (true, false) OR Primary, Secondary (true, true)
        var _googleChartElements =
            [
                {
                    elementName: "energyChargesChart", columnNames: ["Invoice Total excl GST", "Previous Year Total"],
                    columns: [{ primary: [], all: [] }], title: "Energy Charges", activeAxes: [true, false]
                },
                {
                    elementName: "electricityConsumptionChart", columnNames: ["Total Kwh", "Previous Year Kwh"],
                    columns: [{ primary: [], all: [] }], title: "Electricity Consumption", activeAxes: [true, false]
                }
            ];

        // Directive required for (eg, where chart name = "energyChargesChart"):
        // <div class="col-lg-12" ng-if="energyChargesChart.data">
        //        <div google-chart chart="energyChargesChart" style="height:330px; width:100%; padding-right:5px;"></div>
        // </div>

        $scope.chartHelpText = {
            title: "Cost and Consumption Data", // Tootltip title is not handled correctly - hard coded in partial for now.
            detail: "Charts show total energy and invoice costs for all sites with data on file. All totals exclude GST."
        };

        $scope.includeBarChart = true;

        var divisions = [];
        var categories = [];
        var yearArray = {};
        var currentData = [];
        var prior12Data = [];
        $scope.loading = true;

        var onWelcomeMessage = function (data) {
            $scope.welcomeHeader = data.header;
            $scope.welcomeText = data.text;
        };

        var onUserData = function (data) {
            $scope.monthSpanOptions = data.monthSpanOptions;
            $scope.monthSpan = data.monthSpan;
        };

        var onFiltersOk = function (data) {
            filterData.createMultiDropdown('divisions', data.divisions, true, $scope);
            filterData.createMultiDropdown('categories', data.categories, true, $scope);
        };

        var plotAllGraphs = function (data) {
            if (debugStatus_showMessages) { toaster.pop('success', "All data loaded!", "Read histogram and stats data from database") };
            displayStats(data.invoiceStats);
            $scope.loading = false;
        };

        function onError(reason) {
            $scope.loading = false;
            toaster.pop('error', "Data Load Error", "Unable to load data from database! Status ID =" + reason.status);
           // console.log(reason);
        };

        var readAndShowStatistics = function () {
            dbDataSource.getDashboardStatistics($scope.monthSpan, getReturnIds())
                .then(displayStatistics, onError);
        };

        var getBusinessData = function () {
            $scope.loading = true;
            readAndPlotGoogleGraphData();
            readAndShowStatistics();
        };

        var displayStatistics = function (data) {
            displayStats(data);
        };

        $scope.$on("refreshData", function () {
            if (debugStatus_showMessages) { toaster.pop('success', "Event triggered", ""); }
            getBusinessData();
        });

        dbDataSource.getWelcomeScreen()
            .then(onWelcomeMessage, onError);

        userDataSource.getUserData()
           .then(onUserData, onError);

        filterData.getAllFilters()
                .then(onFiltersOk, onError);

        $scope.togglePreviousYearsData = function ($event) {
            $scope.loading = true;
            // Decide which axes are to be displayed.
            googleChart.toggleAxis2Display($scope.showPrevious12, _googleChartElements);
            // Refresh
            googleChart.refreshGoogleChart($scope, _googleChartElements[filterData.elementIndex(_googleChartElements, "Electricity Consumption")]);
            googleChart.refreshGoogleChart($scope, _googleChartElements[filterData.elementIndex(_googleChartElements, "Energy Charges")]);
            $scope.loading = false;
        };

        $scope.reviseMonths = function (newMonthSpan) {
            $scope.loading = true;
            $scope.monthSpan = newMonthSpan;
            readAndPlotGoogleGraphData();
        };

        $scope.toggleGraphType = function ($event) {
            $scope.loading = true;
            if ($scope.showAsBar) {
                var _type = 'column'
            }
            else
            {
                var _type = 'line'
            }
            googleChart.switchAllGoogleChartTypes($scope, _type, _googleChartElements);
            $scope.loading = false;
        };

        var displayStats = function (data) {
            $scope.totalSites = data.totalSites;
            // Percentage missing invoices
            $scope.missingPercent = data.percentMissingInvoices;
            $scope.missingInvoices = data.totalMissingInvoices;
            $scope.activeSites = data.totalActiveSites;

            $scope.missingOptions = {
                animate: {
                    duration: 10,
                    enabled: true
                },
                barColor: '#2C3E50',
                scaleColor: false,
                lineWidth: 5,
                lineCap: 'circle',
                size: 60
            };
            $scope.filedPercent = data.percentSitesWithData;
            $scope.filedOptions = {
                animate: {
                    duration: 5,
                    enabled: true
                },
                barColor: '#2C3E50',
                scaleColor: false,
                lineWidth: 5,
                lineCap: 'circle',
                size: 60
            }
        };

        var getReturnIds = function () {
            return filterData.createApiFilter($scope.categoriesModel, $scope.divisionsModel);
        }

        // Google chart control GPA: Refactor all code here, copied from SiteOverview
        var _siteSelect = 0;  // 0 == select all sites for current user
        var readAndPlotGoogleGraphData = function () {
            filterData.getCostConsumptionData($scope.monthSpan, getReturnIds(), _siteSelect)
                .then(onGoogleGraphData, onError);
        };


        var onGoogleGraphData = function (data) {
            googleChart.initializeGoogleChart($scope, data, _googleChartElements[filterData.elementIndex(_googleChartElements, "Electricity Consumption")]);
            googleChart.initializeGoogleChart($scope, data, _googleChartElements[filterData.elementIndex(_googleChartElements, "Energy Charges")]);
            if (debugStatus_showMessages) { toaster.pop('success', "Google graph data loaded!", ""); }
            
            $scope.loading = false;
        };

    };
})();
