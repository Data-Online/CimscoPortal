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

    dashboard.$inject = ['$scope', '$parse', '$interval', '$timeout', 'dbDataSource', 'userDataSource', 'filterData', 'consumptionData', 'dbConstants', 'toaster', 'googleChart', 'sharedData'];
    function dashboard($scope, $parse, $interval, $timeout, dbDataSource, userDataSource, filterData, consumptionData, dbConstants, toaster, googleChart, sharedData) {

        var debugStatus_showMessages = false;
        $scope.showAsBar = false;  // Whether to show line or bar initiually. Dictates setting for toggle switch
        $scope.allowDisplayByDivision = true;
        $scope.allowShowEnergySavings = true;
        $scope.dpData = [];

        // Test info board
        //var getDatapointDetails = function () {

        //    var _testDate = new Date('01/10/2015');
        //    var _testDate2 = new Date('04/12/2015');
        //    var _testDate3 = new Date('02/12/2015');
        //    $scope.dpData = [
        //                        { status: "Attention", notes: "Missing invoices", date: _testDate },
        //                        { status: "Test", notes: "Some other details", date: _testDate2 },
        //                        { status: "Test2", notes: "Some more details", date: _testDate3 }
        //    ];
        //};

        var pushDataPointDetails = function (item) {
            $scope.dpData.push(item);
        };

        var onDatapointDetails = function (data) {
            pushDataPointDetails(data);
        };

        $scope.removeDatapoint = function (item) {
            var index = $scope.dpData.indexOf(item);
            $scope.dpData.splice(index, 1);
        };

        $scope.seriesSelected = function (selectedItem, text) {
            var _element = _googleChartElements[filterData.elementIndex(_googleChartElements, text, _chartElementId)];
            var _datapointIdentity = googleChart.lineNameFromChartNumber($scope, _element, selectedItem.column, selectedItem.row);
            console.log(_datapointIdentity);

            if (selectedItem) {
                //var zz = _googleChartElements[filterData.elementIndex(_googleChartElements, text, _chartElementId)].columnNames[selectedItem.column];
                consumptionData.getDatapointDetails(_datapointIdentity)
                    .then(onDatapointDetails, onError);
            };
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
                    columns: [[], [], []], colours: ['#009900', '#0000FF', '#DD9900'],
                    filter: ""
                },
                {
                    elementName: "electricityConsumptionChart", columnNames: ["Total Kwh", "Previous Year Kwh"],
                    title: "Electricity Consumption", activeAxes: [true, false],
                    columns: [[], []], colours: ['#009900', '#0000FF'],
                    filter: ""
            }
            ];
        //  ['#0000FF', '#009900', '#CC0000', '#DD9900'],
        var _chartElementId = 'elementName'; // Element used when finding entry by name from the above array.
        var _divisionDataStatus = { elementsAdded: false, updateRequired: true, basedUpon: ['energyChargesChart', 'electricityConsumptionChart'] };
        var _siteSelect = 0;  // 0 == select all sites for current user. Appropriate for the dashboard data display.

        var _userData = [];

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
            _userData = data;
            userDataSource.assignUserData($scope, _userData);
            $scope.updateUserData = updateUserData;
        };

        var updateUserData = function (setting, value) {
            _userData = userDataSource.updateUserData(_userData, setting, value);
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
        };

        var readAndShowStatistics = function () {
            dbDataSource.getDashboardStatistics($scope.monthSpan, getReturnIds())
                .then(displayStatistics, onError);
        };

        var getBusinessData = function () {
            $scope.loading = true;
            readAndPlotGoogleGraphData();
            readAndShowStatistics();
            readAndUpdateDivisonGraphs();
        };

        var displayStatistics = function (data) {
            displayStats(data);
        };

        var _triggerName = filterData.getEventName();
        $scope.$on(_triggerName, function () {
            if (debugStatus_showMessages) { toaster.pop('success', "Event triggered", "(end of count down)"); }
            _divisionDataStatus.updateRequired = true;

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
                        var _filter = filterData.createApiFilter($scope.categoriesModel, _element);
                        //console.log(filterData.createApiFilter($scope.categoriesModel, _element));
                        consumptionData.getCostConsumptionData($scope.monthSpan, _filter, _siteSelect)
                            .then(function success(data) { return onDivisionData(data, key, _filter) }, onError);
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
            googleChart.createButtonControls($scope, _googleChartElements);
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

        var onDivisionData = function (data, pairNo, filter) {
            var _target1 = "electricityConsumptionChart" + pairNo;
            var _target2 = "energyChargesChart" + pairNo;
            var _filterIconRequired = filterData.filterTypeActive('category', filter);

            var getter = $parse(_target1 + '.filtersActive');
            getter.assign($scope, _filterIconRequired);
            var getter = $parse(_target2 + '.filtersActive');
            getter.assign($scope, _filterIconRequired);

            // Required for html link, if selected
            var getter = $parse('divisionLinkFilter' + pairNo);
            getter.assign($scope, filter);
            var _element1 = _googleChartElements[filterData.elementIndex(_googleChartElements, _target1, _chartElementId)];
            var _element2 = _googleChartElements[filterData.elementIndex(_googleChartElements, _target2, _chartElementId)];

            _element1.filter = filter;
            _element2.filter = filter;
            googleChart.initializeGoogleChart($scope, data, _element1);
            googleChart.initializeGoogleChart($scope, data, _element2);
            $scope.loading = false;
        };

        // END

        googleChart.createButtonControls($scope, _googleChartElements);

        $scope.reviseMonths = function (newMonthSpan) {
            _divisionDataStatus.updateRequired = true;

            $scope.loading = true;
            $scope.monthSpan = newMonthSpan;
            readAndPlotGoogleGraphData();
            readAndUpdateDivisonGraphs();

            updateUserData("monthSpan", newMonthSpan);
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
            consumptionData.getCostConsumptionData($scope.monthSpan, getReturnIds(), _siteSelect)
                .then(onGoogleGraphData, onError);
        };

        var initializeGoogleCharts = function (charts, data, applyTitleDetail) {
            var _filtersActive = getReturnIds() != filterData.inactiveFilter();
            // console.log(_filtersActive);
            angular.forEach(charts, function (chart, index) {
                var _element = _googleChartElements[filterData.elementIndex(_googleChartElements, chart, _chartElementId)];
                googleChart.initializeGoogleChart($scope, data, _element);
                if (applyTitleDetail) {
                    var getter = $parse(chart + '.title');
                    var _currentText = getter($scope, chart + '.title');
                    getter.assign($scope, _currentText + ' for ' + sharedData.getSharedValues().topLevelName + ' Group');
                };
                var getter = $parse(chart + '.filtersActive');
                getter.assign($scope, _filtersActive);
                _element.filter = getReturnIds();
            });
        };

        var onGoogleGraphData = function (data) {
            //if (!_divisionDataStatus.elementsAdded) {
            //    _googleChartElements[filterData.elementIndex(_googleChartElements, "electricityConsumptionChart", _chartElementId)].title = "TEST UPDATE";
            //};
            // GPA ** --> Move to global and define only once. (divisions uses this also)
            var _primaryCharts = ["electricityConsumptionChart", "energyChargesChart"];

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



        ////TEST
        //var chart1 = {};
        //chart1.type = "LineChart";
        //chart1.displayed = false;
        //chart1.data = {
        //    "cols": [{
        //        id: "month",
        //        label: "Month",
        //        type: "string"
        //    }, {
        //        id: "laptop-id",
        //        label: "Laptop",
        //        type: "number"
        //    }, {
        //        id: "desktop-id",
        //        label: "Desktop",
        //        type: "number"
        //    }, {
        //        id: "server-id",
        //        label: "Server",
        //        type: "number"
        //    }, {
        //        id: "cost-id",
        //        label: "Shipping",
        //        type: "number"
        //    }],
        //    "rows": [{
        //        c: [{
        //            v: "January"
        //        }, {
        //            v: 19,
        //            f: "42 items"
        //        }, {
        //            v: 12,
        //            f: "Ony 12 items"
        //        }, {
        //            v: 7,
        //            f: "7 servers"
        //        }, {
        //            v: 4
        //        }]
        //    }, {
        //        c: [{
        //            v: "February"
        //        }, {
        //            v: 13
        //        }, {
        //            v: 1,
        //            f: "1 unit (Out of stock this month)"
        //        }, {
        //            v: 12
        //        }, {
        //            v: 2
        //        }]
        //    }, {
        //        c: [{
        //            v: "March"
        //        }, {
        //            v: 24
        //        }, {
        //            v: 5
        //        }, {
        //            v: 11
        //        }, {
        //            v: 6
        //        }

        //        ]
        //    }]
        //};
        //chart1.options = {
        //    "title": "Sales per month",
        //    "colors": ['#0000FF', '#009900', '#CC0000', '#DD9900'],
        //    "defaultColors": ['#0000FF', '#009900', '#CC0000', '#DD9900'],
        //    "isStacked": "true",
        //    "fill": 20,
        //    //"displayExactValues": true,
        //    "vAxis": {
        //        "title": "Sales unit",
        //        "gridlines": {
        //            "count": 10
        //        }
        //    },
        //    "hAxis": {
        //        "title": "Date"
        //    }
        //};
        //chart1.view = {
        //    columns: [0, 1, 2, 3, 4]
        //};
        //$scope.myChart = chart1;



        //TEST



    };
})();
