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

    dashboard.$inject = ['$scope', '$parse', '$interval', '$timeout', 'dbDataSource', 'userDataSource', 'filterData', 'consumptionData', 'datapointDetails', 'dbConstants', 'toaster', 'googleChart', 'sharedData', 'CONFIG'];
    function dashboard($scope, $parse, $interval, $timeout, dbDataSource, userDataSource, filterData, consumptionData, datapointDetails, dbConstants, toaster, googleChart, sharedData, CONFIG) {

        // Option buttons
        $scope.allowDisplayByDivision = true;
        $scope.allowShowEnergySavings = true;
        $scope.includeBarChart = true; // Include bar chart seletion option ?
        $scope.showAsBar = false;  // Whether to show line or bar initiually. Dictates setting for toggle switch

        // Test stacked 100% bar

        //consumptionData.getComparisonData()
        //                    .then(function success(data) { return onComparisonData(data) }, onError);


        // Google chart control 
        var readAndPlotComparisonData = function () {
            consumptionData.getComparisonData($scope.monthSpan, getReturnIds(), _siteSelect)
                .then(onComparisonData, onError);
        };
        ////consumptionData.getComparisonData($scope.monthSpan, _filter, _siteSelect)
        ////                    .then(function success(data) { return onComparisonData(data, key, _filter) }, onError);


        $scope.myChartObject = {};
        $scope.myChartObject.type = "BarChart";

        var onComparisonData = function (data) {
            //console.log(pairNo);
            var _zData = googleChart.zTestGoogle(data);

            $scope.myChartObject.data =
                {
                    "cols": _zData.cols,
                    "rows": _zData.rows
                }

            var _legend = { position: 'top', maxLines: 3 };
            var _width = { groupWidth: '60%' };
            var _isHtml = { isHtml: true };
            //var _haxis = { minValue: 4.0, viewWindow: { min: 4, max: 7 }, maxValue: 7.0, gridlines: { count: 10 } };
            var _haxis = { minValue: data.startEnd[0], viewWindow: { min: data.startEnd[0], max: data.startEnd[1] }, maxValue: data.startEnd[1], gridlines: { count: 10 } };
            $scope.myChartObject.options = {
                'title': 'Average energy consumption per SqM',
                'isStacked': 'true',
                'legend': 'none',
                'bar': _width,
                'height': 150,
                'tooltip': _isHtml,
                'hAxis': _haxis,
                "colors": CONFIG.googleComparisonBarTheme, // ['#0000FF', '#009900', '#0000FF', '#CC0000', '#0000FF', '#DD9900', '#0000FF']
                "series": { 0: { tooltip: false }, 2: { tooltip: false }, 4: { tooltip: false }, 6: { tooltip: false } }
                //, 1: { tooltip: true }, 1: { tooltip: false}, 2: { tooltip: true }, 3: { tooltip: false }, 4: { tooltip: true }, 5: { tooltip: false } }
            };
            $scope.testDelta = data.analysisFigures[0];
        };

        //  $scope.myChartObject = {};

        //  $scope.myChartObject.type = "BarChart";
        //  var _html = "<div><p><b>Test text</b></p></div>";
        //  $scope.kwh = [
        //      { v: "KWh / SqM" },

        //      {
        //          v: 4.5439801
        //      },  // Pad
        //      { f: _html },
        //      { v: "lightblue" },

        //      { v: 0.02 },             // Low
        //      { f: "Low" },

        //      {
        //          v: 0.964878575
        //      },     // Pad
        //      { f: "pad" },
        //      { v: "lightblue" },

        //      { v: 0.02 },      // Avg
        //      { f: "avg" },

        //      {
        //          v: 1.173357222
        //      },       // Pad
        //      { f: "pad" },
        //      { v: "lightblue" },

        //      { v: 0.02 },      // Avg
        //      { f: "max" },

        //      {
        //          v: 0.257784103

        //      },          // Pad
        //      { f: "endpad" },
        //      { v: "lightblue" },

        //  ];

        //  var _columnData = [
        //          { id: "t", label: "Measurement", type: "string" },

        //          { id: "s", label: "% Sites using less energy", type: "number" },
        //          { id: "tt", type: "number", role: "tooltip", p: { "html": true } },
        //          { id: "s1", type: "string", role: "style" },

        //          { id: "x", label: "Selected site(s)", type: "number" },
        //          { id: "tt2", label: "Title", type: "number", role: "tooltip" },

        //          { id: "y", label: "% Sites using more energy", type: "number" },
        //          { id: "tt3", label: "Title", type: "number", role: "tooltip" },
        //          { id: "s1", type: "string", role: "style" },

        //          { id: "y", label: "% Sites using more energy", type: "number" },
        //          { id: "tt4", label: "Title", type: "number", role: "tooltip" },

        //          { id: "y", label: "% Sites using more energy", type: "number" },
        //          { id: "tt5", label: "Title", type: "number", role: "tooltip" },
        //          { id: "s1", type: "string", role: "style" },

        //          { id: "y", label: "% Sites using more energy", type: "number", color: "yellow" },
        //          { id: "tt6", label: "Title", type: "number", role: "tooltip" },

        //          { id: "y", label: "% Sites using more energy", type: "number" },
        //          { id: "tt7", label: "Title", type: "number", role: "tooltip" },
        //          { id: "s1", type: "string", role: "style" }

        //  ];
        //  var _rowData =  [ { c: $scope.kwh } ];
        //  var ztest = { "columns": _columnData, "rows": _rowData };
        ////  googleChart.zTestGoogle(ztest);

        //  $scope.myChartObject.data = {
        //      "cols": [
        //          { id: "t", label: "Measurement", type: "string" },

        //          { id: "s", label: "% Sites using less energy", type: "number" },
        //          { id: "tt", type: "number", role: "tooltip", p: { "html": true } },
        //          { id: "s1", type: "string", role: "style" },

        //          { id: "x", label: "Selected site(s)", type: "number" },
        //          { id: "tt2", label: "Title", type: "number", role: "tooltip" },

        //          { id: "y", label: "% Sites using more energy", type: "number" },
        //          { id: "tt3", label: "Title", type: "number", role: "tooltip" },
        //          { id: "s1", type: "string", role: "style" },

        //          { id: "y", label: "% Sites using more energy", type: "number" },
        //          { id: "tt4", label: "Title", type: "number", role: "tooltip" },

        //          { id: "y", label: "% Sites using more energy", type: "number" },
        //          { id: "tt5", label: "Title", type: "number", role: "tooltip" },
        //          { id: "s1", type: "string", role: "style" },

        //          { id: "y", label: "% Sites using more energy", type: "number", color: "yellow" },
        //          { id: "tt6", label: "Title", type: "number", role: "tooltip" },

        //          { id: "y", label: "% Sites using more energy", type: "number" },
        //          { id: "tt7", label: "Title", type: "number", role: "tooltip" },
        //          { id: "s1", type: "string", role: "style" }

        //      ], "rows": [
        //          { c: $scope.kwh }
        //      ]
        //  };

        //  $scope.myChartObject.data = {
        //      "cols": _columnData,
        //      "rows": [
        //          { c: $scope.kwh }
        //      ]
        //  };



        //


        // elemenName == Page element where chart is to be reendered
        // columnNames == [Primary, Seconday] axes for chart. Primary axis is displayed by default. These names need to match columns returned from server.
        // title == Title to render on page (if required)
        // activeAxes == Which of the 2 axes are displayed. Current suppor is for Primary, !Secondary (true, false) OR Primary, Secondary (true, true)
        //console.log(CONFIG.googleChartTheme);
        var _googleChartElements =
            [
                {
                    elementName: "energyChargesChart", columnNames: ["Invoice Total excl GST", "Previous Year Total", "Project Saving Estimate"],
                    title: "Energy Charges", activeAxes: [true, false, false],
                    columns: [[], [], []], colours: CONFIG.googleChartTheme,//['#009900', '#0000FF', '#DD9900'],
                    filter: ""
                },
                {
                    elementName: "electricityConsumptionChart", columnNames: ["Total Kwh", "Previous Year Kwh"],
                    title: "Electricity Consumption", activeAxes: [true, false],
                    columns: [[], []], colours: CONFIG.googleChartTheme, //['#009900', '#0000FF'],
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
            detail: "Charts show total energy and invoice costs for all sites with data on file. All totals exclude GST. Values shown are for the month the costs were incurred, not the Invoice date"
        };

        $scope.loading = true;

        // Datapoint detail display functions
        var dpConfig = { dataElementName: 'dpData', loadIconElementName: 'dpLoading' };
        $scope.dpData = [];
        $scope.seriesSelected = function (selectedItem, text) {
            //$scope.dpLoading = true;
            var _siteId = 0; // i.e all sites
            var _element = _googleChartElements[filterData.elementIndex(_googleChartElements, text, _chartElementId)];
            datapointDetails.selectedDatapoint(selectedItem, _element, $scope, dpConfig, _siteId);
        };
        $scope.removeDatapoint = function (item) {
            datapointDetails.removeDatapoint(item, $scope, dpConfig.dataElementName);
        }
        // End


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
            readAndPlotComparisonData();
        };

        var displayStatistics = function (data) {
            displayStats(data);
        };

        var _triggerName = filterData.getEventName();
        $scope.$on(_triggerName, function () {
            if (CONFIG.debug) { toaster.pop('success', "Event triggered", "(end of count down)"); }
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
            // GPA ** --> Move to global and define only once. (divisions uses this also)
            var _primaryCharts = ["electricityConsumptionChart", "energyChargesChart"];
            initializeGoogleCharts(_primaryCharts, data, $scope.allowDisplayByDivision);
            $scope.loading = false;
        };
    };
})();
