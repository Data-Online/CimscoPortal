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

    dashboard.$inject = ['$scope', '$parse', '$interval', '$timeout', 'dbDataSource', 'userDataSource', 'filterData', 'dbConstants', 'toaster', 'googleChart', 'sharedData'];
    function dashboard($scope, $parse, $interval, $timeout, dbDataSource, userDataSource, filterData, dbConstants, toaster, googleChart, sharedData) {

        var debugStatus_showMessages = false;
        $scope.showAsBar = false;  // Whether to show line or bar initiually. Dictates setting for toggle switch
        $scope.allowDisplayByDivision = true;
        $scope.allowShowEnergySavings = true;

        $scope.gczztest = gczztest;
        var gczztest = function (element) {
            console.log("Selected " + element);
        };
        

        // elemenName == Page element where chart is to be reendered
        // columnNames == [Primary, Seconday] axes for chart. Primary axis is displayed by default. These names need to match columns returned from server.
        // title == Title to render on page (if required)
        // activeAxes == Which of the 2 axes are displayed. Current suppor is for Primary, !Secondary (true, false) OR Primary, Secondary (true, true)

        var _googleChartElements =
            [
                {
                    elementName: "energyChargesChart", columnNames: ["Invoice Total excl GST", "Previous Year Total", "Project Saving Estimate"],
                    title: "Energy Charges", activeAxes: [true, false, false],
                    columns: [[], [], []], colours: ['#009900', '#0000FF', '#DD9900']
                },
                {
                    elementName: "electricityConsumptionChart", columnNames: ["Total Kwh", "Previous Year Kwh"],
                    title: "Electricity Consumption", activeAxes: [true, false],
                    columns: [[], []], colours: ['#009900', '#0000FF']
                }
            ];
        //  ['#0000FF', '#009900', '#CC0000', '#DD9900'],
        var _chartElementId = 'elementName'; // Element used when finding entry by name from the above array.
        var _divisionDataStatus = { elementsAdded: false, updateRequired: true, basedUpon: ['energyChargesChart', 'electricityConsumptionChart'] };
        var _siteSelect = 0;  // 0 == select all sites for current user. Appropriate for the dashboard data display.

        // Directive required for (eg, where chart name = "energyChargesChart"):
        // <div class="col-lg-12" ng-if="energyChargesChart.data">
        //        <div google-chart chart="energyChargesChart" style="height:330px; width:100%; padding-right:5px;"></div>
        // </div>

        $scope.chartHelpText = {
            title: "Cost and Consumption Data", // Tootltip title is not handled correctly - hard coded in partial for now.
            detail: "Charts show total energy and invoice costs for all sites with data on file. All totals exclude GST."
        };

        $scope.includeBarChart = true;
        $scope.loading = true;

        var onWelcomeMessage = function (data) {
            $scope.welcomeHeader = data.header;
            $scope.welcomeText = data.text;
        };

        var onUserData = function (data) {
            $scope.monthSpanOptions = data.monthSpanOptions;
            $scope.monthSpan = data.monthSpan;
        };

        var onFilterData = function (data) {
            var _createWatch = true;
            $scope.allowDisplayByDivision = (data.divisions != null);
            filterData.createMultiDropdown('divisions', data.divisions, _createWatch, $scope);
            filterData.createMultiDropdown('categories', data.categories, _createWatch, $scope);
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

        var _triggerName = filterData.getEventName();
        $scope.$on(_triggerName, function () {
            if (debugStatus_showMessages) { toaster.pop('success', "Event triggered", "(end of count down)"); }
            getBusinessData();
        });

        dbDataSource.getWelcomeScreen()
            .then(onWelcomeMessage, onError);

        userDataSource.getUserData()
           .then(onUserData, onError);

        filterData.getAllFilters()
                .then(onFilterData, onError);


        // BEGIN Split out divisions to separate charts - Test code
        var plotAllDivisions = function () {
            if (_divisionDataStatus.updateRequired) {
                var _siteSelect = 0;  // 0 == select all sites for current user
                angular.forEach($scope.divisionsData,
                    function (entry, key) {
                        var _element = [];
                        _element.push(entry);
                        //console.log(filterData.createApiFilter($scope.categoriesModel, _element));
                        filterData.getCostConsumptionData($scope.monthSpan, filterData.createApiFilter($scope.categoriesModel, _element), _siteSelect)
                            .then(function success(data) { return onDivisionData(data, key) }, onError);
                    }
            )
            }
        };

        var addDivisionElements = function () {
            if (_divisionDataStatus.elementsAdded) { return; }
            angular.forEach($scope.divisionsData, function (entry, key) {
                var _newArray = [JSON.parse(
                    JSON.stringify(_googleChartElements[filterData.elementIndex(_googleChartElements, _divisionDataStatus.basedUpon[0], _chartElementId)])),
                    JSON.parse(JSON.stringify(_googleChartElements[filterData.elementIndex(_googleChartElements, _divisionDataStatus.basedUpon[1], _chartElementId)]))
                ];
                // Add entry pairs for each division
                var _template1 = _newArray[0];
                var _template2 = _newArray[1];

                _template1.elementName = _template1.elementName + key;
                _template1.title = _template1.title + " for " + entry.label + " Division";
                _template2.elementName = _template2.elementName + key;
                _template2.title = _template2.title + " for " + entry.label + " Division";
                _googleChartElements = _googleChartElements.concat(_newArray);
            });
            _divisionDataStatus.elementsAdded = true;
        };

        $scope.splitOutDivisions = function () {
            // Create enries for all divisions in elements table, if not already done
            // Use pairNo ids to distinguish between each division output
            readAndUpdateDivisonGraphs();
        };

        var readAndUpdateDivisonGraphs = function () {
            if ($scope.splitDivisons) {
                $scope.loading = true;
                addDivisionElements();
                plotAllDivisions();
            }
        };

        var onDivisionData = function (data, pairNo) {
            $scope.loading = false;
            var _target1 = "electricityConsumptionChart" + pairNo;
            var _target2 = "energyChargesChart" + pairNo;
            // Read the data and plot the graphs
            googleChart.initializeGoogleChart($scope, data, _googleChartElements[filterData.elementIndex(_googleChartElements, _target1, _chartElementId)]);
            googleChart.initializeGoogleChart($scope, data, _googleChartElements[filterData.elementIndex(_googleChartElements, _target2, _chartElementId)]);

            //console.log(sharedData.getSharedValues().topLevelName);
        };

        // END

        googleChart.createButtonControls($scope, _googleChartElements);

        //$scope.togglePreviousYearsData = function ($event) {
        //    $scope.loading = true;
        //    // Decide which axes are to be displayed.
        //    googleChart.toggleAxis2Display($scope.showPrevious12, _googleChartElements);
        //    // Refresh
        //    googleChart.refreshAllGoogleCharts($scope, _googleChartElements);
        //    $scope.loading = false;
        //};

        //$scope.toggleEnergySavingData = function ($event) {
        //    $scope.loading = true;
        //    // Decide which axes are to be displayed.
        //    googleChart.ZtoggleAxis2Display($scope.showSavings, _googleChartElements)
        //    // Refresh
        //    googleChart.refreshAllGoogleCharts($scope, _googleChartElements);
        //    $scope.loading = false;
        //};

        $scope.reviseMonths = function (newMonthSpan) {
            _divisionDataStatus.updateRequired = true;

            $scope.loading = true;
            $scope.monthSpan = newMonthSpan;
            readAndPlotGoogleGraphData();
            readAndUpdateDivisonGraphs();
        };

        $scope.toggleGraphType = function ($event) {
            $scope.loading = true;
            if ($scope.showAsBar) {
                var _type = 'column'
            }
            else {
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

        // Google chart control 
        var readAndPlotGoogleGraphData = function () {
            filterData.getCostConsumptionData($scope.monthSpan, getReturnIds(), _siteSelect)
                .then(onGoogleGraphData, onError);
        };

        var initializeGoogleCharts = function (charts, data, applyTitleDetail) {
            var _filtersActive = getReturnIds() != filterData.inactiveFilter();
            console.log(_filtersActive);
            angular.forEach(charts, function (chart, index) {
                googleChart.initializeGoogleChart($scope, data, _googleChartElements[filterData.elementIndex(_googleChartElements, chart, _chartElementId)]);
                if (applyTitleDetail) {
                    var getter = $parse(chart + '.title');
                    var _currentText = getter($scope, chart + '.title');
                    getter.assign($scope, _currentText + ' for ' + sharedData.getSharedValues().topLevelName + ' Group');
                };
                var getter = $parse(chart + '.filtersActive');
                getter.assign($scope, _filtersActive)
            });
        };

        var onGoogleGraphData = function (data) {
            //if (!_divisionDataStatus.elementsAdded) {
            //    _googleChartElements[filterData.elementIndex(_googleChartElements, "electricityConsumptionChart", _chartElementId)].title = "TEST UPDATE";
            //};
            // GPA ** --> Move to global and define only once. (divisions uses this also)
            var _primaryCharts = [ "electricityConsumptionChart", "energyChargesChart" ];

            initializeGoogleCharts(_primaryCharts, data, $scope.allowDisplayByDivision);
           
            //googleChart.initializeGoogleChart($scope, data, _googleChartElements[filterData.elementIndex(_googleChartElements, "electricityConsumptionChart", _chartElementId)]);
            //googleChart.initializeGoogleChart($scope, data, _googleChartElements[filterData.elementIndex(_googleChartElements, "energyChargesChart", _chartElementId)]);

            //updateChartTitles("electricityConsumptionChart")
            //if (getReturnIds() != '__') {
            //    $scope.electricityConsumptionChart.title = $scope.electricityConsumptionChart.title + "TEST 2";
            //}
            //if (debugStatus_showMessages) { toaster.pop('success', "Google graph data loaded!", ""); }
            //$scope.electricityConsumptionChart.view = [0];
            //if ($scope.electricityConsumptionChart) {
            //    console.log($scope.electricityConsumptionChart.view);
            //    console.log($scope.electricityConsumptionChart.options.colors);
            //};
            $scope.loading = false;
        };



        //TEST
        var chart1 = {};
        chart1.type = "LineChart";
        chart1.displayed = false;
        chart1.data = {
            "cols": [{
                id: "month",
                label: "Month",
                type: "string"
            }, {
                id: "laptop-id",
                label: "Laptop",
                type: "number"
            }, {
                id: "desktop-id",
                label: "Desktop",
                type: "number"
            }, {
                id: "server-id",
                label: "Server",
                type: "number"
            }, {
                id: "cost-id",
                label: "Shipping",
                type: "number"
            }],
            "rows": [{
                c: [{
                    v: "January"
                }, {
                    v: 19,
                    f: "42 items"
                }, {
                    v: 12,
                    f: "Ony 12 items"
                }, {
                    v: 7,
                    f: "7 servers"
                }, {
                    v: 4
                }]
            }, {
                c: [{
                    v: "February"
                }, {
                    v: 13
                }, {
                    v: 1,
                    f: "1 unit (Out of stock this month)"
                }, {
                    v: 12
                }, {
                    v: 2
                }]
            }, {
                c: [{
                    v: "March"
                }, {
                    v: 24
                }, {
                    v: 5
                }, {
                    v: 11
                }, {
                    v: 6
                }

                ]
            }]
        };
        chart1.options = {
            "title": "Sales per month",
            "colors": ['#0000FF', '#009900', '#CC0000', '#DD9900'],
            "defaultColors": ['#0000FF', '#009900', '#CC0000', '#DD9900'],
            "isStacked": "true",
            "fill": 20,
            //"displayExactValues": true,
            "vAxis": {
                "title": "Sales unit",
                "gridlines": {
                    "count": 10
                }
            },
            "hAxis": {
                "title": "Date"
            }
        };
        chart1.view = {
            columns: [0, 1, 2, 3, 4]
        };
        $scope.myChart = chart1;

        $scope.seriesSelected = function (selectedItem) {
            if (selectedItem) {
                console.log("Selected point!" + selectedItem.row + " " + selectedItem.column);
            };
            //var chartData = $scope.myChart.data;
            //var value = chartData.rows[selectedItem.row].c[selectedItem.column].v;
            //var formattedValue = chartData.rows[selectedItem.row].c[selectedItem.column].f;
            //console.log(value + ":" + formattedValue);
        };

        //TEST



    };
})();
