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

    dashboard.$inject = ['$scope', '$parse', '$interval', 'dbDataSource', 'userDataSource', 'dbConstants', 'toaster', 'googleChart'];
    function dashboard($scope, $parse, $interval, dbDataSource, userDataSource, dbConstants, toaster, googleChart) {

        $scope.pop = function () {
            toaster.pop('success', "title", "text");
        };

        var divisions = [];
        //[{ id: 1, label: 'MEGA Stores' },
        //   { id: 2, label: 'Mitre 10 Stores' }];
        var categories = [];
        //[{ id: 1, label: 'Hardware and Building Supplies Retailing' },
        //           { id: 2, label: 'Another A' },
        //           { id: 3, label: 'Another B' }];
        //var monthSpan;

        var yearArray = {};
        var currentData = [];
        var prior12Data = [];

        userDataSource.getUserData()
            .then(onUserData, onError);

        dbDataSource.getWelcomeScreen()
            .then(onWelcomeMessage, onError);

        function onWelcomeMessage(data) {
            $scope.welcomeHeader = data.header;
            $scope.welcomeText = data.text;
            console.log(data);
        };

        function onUserData(data) {
            $scope.monthSpanOptions = data.monthSpanOptions;
            //monthSpan = data.monthSpan;
            $scope.monthSpan = data.monthSpan;
            console.log('User Data ' + data.monthSpanOptions + " " + data.monthSpan);

            // $scope.topLevelName = data.topLevelName;
            var companyId = 0;
            //dbDataSource.getTotalCostsByMonth(data.monthSpan, companyId)
            //    .then(function success(data) { return onGraphData(data, dbConstants.costsDataElement, 0) }, onError);

            //dbDataSource.getTotalCostAndConsumption(data.monthSpan, "null")
            //    .then(plotAllGraphs, onError);
            //            .then(function success(data) {return onGraphData(data.cost, dbConstants.consDataElement, 1) }, onError);

            dbDataSource.getAllFilters()
                .then(onFiltersOk, onError);
        };

        var onFiltersOk = function (data) {
            //            console.log('Divisions : ' + divisions);
            createMultiDropdown('divisions', data.divisions, true);
            createMultiDropdown('categories', data.categories, true);
        };

        var plotAllGraphs = function (data) {
            // console.log('got data .. ');
            toaster.pop('success', "All data loaded!", "Read histogram and stats data from database");
            $scope.loading = false;
            
            onGraphData(data.cost, dbConstants.costsDataElement, 0);
            onGraphData(data.consumption, dbConstants.consDataElement, 1);
            displayStats(data.invoiceStats);
        };

        function onGraphData(data, target, index) {
            // Assign data to graph and display
            yearArray = {
                years: data.years,
                months: data.months,
                invoices: data.totalInvoices,
                invoices12: data.totalInvoices12
            };
            console.log(yearArray);
            var unit = ["$", "KWh"];
            currentData[index] = assignData(data.values, themeprimary, "current" + ":" + unit[index]);
            prior12Data[index] = assignData(data.values12, themesecondary, "minus12" + ":" + unit[index]);
            refreshData($scope.showPrevious12, currentData[index], prior12Data[index], target);
        }

        function onError(reason) {
            $scope.loading = false;
            toaster.pop('error', "Data Load Error", "Unable to load data from database!");
            console.log('Error reading user data');
            $scope.reason = reason;
        };

        var assignData = function (data, theme, label) {
            return {
                label: label,
                fillColor: theme,
                strokeColor: theme,
                highlightFill: theme,
                highlightStroke: 'rgba(0,0,0,0.5)',
                data: data
            };
        };

        var readAndPlotHistogram = function () {
            dbDataSource.getTotalCostAndConsumption($scope.monthSpan, getReturnIds())
                .then(plotAllGraphs, onError);
        };

        $scope.togglePreviousYearsData = function ($event) {
            var checkbox = $event.target;
            refreshData(checkbox.checked, currentData[0], prior12Data[0], dbConstants.costsDataElement);
            refreshData(checkbox.checked, currentData[1], prior12Data[1], dbConstants.consDataElement);
        };

        $scope.reviseMonths = function (newMonthSpan) {
            // console.log('revise data...' + newMonthSpan);
            $scope.loading = true;
            $scope.monthSpan = newMonthSpan;
            readAndPlotHistogram();
            readAndPlotGoogleGraphData();
            //dbDataSource.getTotalCostAndConsumption($scope.monthSpan, getReturnIds())
            //    .then(plotAllGraphs, onError);
        };

        function refreshData(showPreviousYear, firstDataset, secondDataset, target) {
            var getter = $parse(target);
            if (showPreviousYear) {
                getter.assign($scope, {
                    labels: yearArray.months,
                    datasets: [secondDataset, firstDataset]
                });
            }
            else {
                getter.assign($scope, {
                    labels: yearArray.months,
                    datasets: [firstDataset]
                });
            }
        };

        // Chart.js Options
        $scope.barChartOptions = {

            multiTooltipTemplate: function (v) { return multiTooltip(v, yearArray); },//" : $<%= value %>",//"<%=datasetLabel%> : $<%= value %>",

            tooltipTemplate: function (v) { return singleTooltip(v, yearArray); },
            //tooltipTemplate: "<%=label%> : $<%= value %>",
            // Sets the chart to be responsive
            responsive: true,

            //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
            scaleBeginAtZero: true,

            //Boolean - Whether grid lines are shown across the chart
            scaleShowGridLines: true,

            //String - Colour of the grid lines
            scaleGridLineColor: "rgba(0,0,0,.05)",

            //Number - Width of the grid lines
            scaleGridLineWidth: 1,

            //Boolean - If there is a stroke on each bar
            barShowStroke: true,

            //Number - Pixel width of the bar stroke
            barStrokeWidth: 2,

            //Number - Spacing between each of the X value sets
            barValueSpacing: 5,

            //Number - Spacing between data sets within X values
            barDatasetSpacing: 1,

            //String - A legend template
            //legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>'
            legendTemplate: ' '
        };

        var displayStats = function (data) {
            //console.log('Display stats...' + data.percentMissingInvoices);
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
        // Multi selects
        var createMultiDropdown = function (baseName, selectionItemsList, createWatch) {
            // Create variables on scope
            var getter = $parse(baseName + 'Model');
            getter.assign($scope, []);

            getter = $parse(baseName + 'Data');
            getter.assign($scope, selectionItemsList);
            getter = $parse(baseName + 'CustomTexts');

            var buttonText = 'All ' + baseName.capitalizeFirstLetter();
            var customTexts = { buttonDefaultText: buttonText, uncheckAll: 'Clear Filters' };
            getter.assign($scope, customTexts);
            if (createWatch) {
                $scope.$watch(baseName + 'Model', function (data) {
                    filterData(data, baseName);
                }, true);
            };

            var maxTextLength = buttonText.length;
            getter = $parse(baseName + 'Settings');
            getter.assign($scope, {
                smartButtonMaxItems: 1,
                externalIdProp: '',
                showCheckAll: false,
                smartButtonTextConverter: function (itemText, originalItem) {
                    if (itemText.length > maxTextLength) {
                        return itemText.substring(0, (maxTextLength - 2)) + '..';
                    }
                }
            });

        };

        //console.log('Categories : ' + categories);

        //        createMultiDropdown('divisions', divisions, true);
        //        createMultiDropdown('categories', categories, true);

        var filterData = function (data, baseName) {
            console.log('Start delay for data ' + baseName);
            //console.log($scope.divisionsModel);
            startDelay(data);
        };

        var stop;
        var _counter = dbConstants.filterSelectDelay;//20;
        var startDelay = function (data) {
            if (angular.isDefined(stop)) { _counter = dbConstants.filterSelectDelay; return; }
            stop = $interval(function () {
                if (_counter > 0) {
                    _counter--;
                }
                else { stopCounter(data); }
            }, 100)
        }
        var stopCounter = function (data) {
            if (angular.isDefined(stop)) {
                $interval.cancel(stop);
                stop = undefined;
                _counter = dbConstants.filterSelectDelay;//20;
            };
            //console.log('Return values ' + getReturnIds() + ' checkbox = ' + $scope.showPrevious12);
            $scope.loading = true;
            readAndPlotHistogram();
            //dbDataSource.getTotalCostAndConsumption($scope.monthSpan, getReturnIds())
            //    .then(plotAllGraphs, onError);

            readAndPlotGoogleGraphData();
        };

        var getReturnIds = function () {
            var _returnIds = "_";
            angular.forEach($scope.categoriesModel, function (value, key) {
                _returnIds += value.id + "-";
            });
            _returnIds += "_";
            angular.forEach($scope.divisionsModel, function (value, key) {
                _returnIds += value.id + "-";
            });
            return _returnIds;
        };

        // Google chart control GPA: Refactor all code here, copied from SiteOverview
        
        var readAndPlotGoogleGraphData = function () {
            console.log("Google chart plot...");
            dbDataSource.getCostConsumptionData($scope.monthSpan, getReturnIds())
                .then(onGoogleGraphData, onError);
        };

        var onGoogleGraphData = function (data) {
            var _collatedData = googleChart.collateData(data);
            initializeGoogleChart(_collatedData);
            toaster.pop('success', "Google graph data loaded!", "");
            $scope.loading = false;
        };

        $scope.myChartObject = {};
        function initializeGoogleChart(data) {
            $scope.myChartObject.type = "LineChart";//"BarChart";// 
            $scope.myChartObject.displayed = false;
            $scope.myChartObject.data = {
                "cols": data.cols,
                "rows": data.rows
            };

            $scope.myChartObject.options = {
                "interpolateNulls": true,
                "chartArea": { "height": "50%" },
                "title": "Charges and Consumption",
                "colors": ['#0000FF', '#009900', '#CC0000', '#DD9900'],
                "defaultColors": ['#0000FF', '#009900', '#CC0000', '#DD9900'],
                "isStacked": "true",
                "fill": 20,
                "displayExactValues": true,
                "pointSize": 5,
                "lineWidth": 3,
                "vAxes": {
                    0: {
                        "title": "Invoice Total excl GST",
                        "format": "currency"
                    }, 1: {
                        "title": "Total Kwh",
                        "format": "decimal"
                    }
                }
    ,
                "hAxis": {
                    "title": "Month", "direction": 1, "slantedText": true, "slantedTextAngle": 45
                },
                "animation": {
                    "duration": 1000,
                    "easing": 'out',
                }
                , "series": [{ targetAxisIndex: 0 }, { targetAxisIndex: 1 }]
            };
            $scope.myChartObject.view = {
                columns: [0,1,2]
            };
        }



    };

    String.prototype.capitalizeFirstLetter = function () {
        return this.charAt(0).toUpperCase() + this.slice(1);
    };
    var yearFromArray = function (v, yearArray) {
        return yearArray.years[yearArray.months.indexOf(v.label) + GetOffSet(yearArray.length, v.x)];
    };
    var totalInvoicesFromArray = function (v, yearArray, whichYear) {
        if (whichYear == 12) {
            return yearArray.invoices12[yearArray.months.indexOf(v.label) + GetOffSet(yearArray.length, v.x)];
        }
        else {
            return yearArray.invoices[yearArray.months.indexOf(v.label) + GetOffSet(yearArray.length, v.x)];
        }
    };

    function GetOffSet(length, x) {
        if (length > 12 && v.x > 400)
            return 12
        else
            return 0
    };
 

    function singleTooltip(v, yearArray) {
        //console.log(v);
        var unit = v.datasetLabel.split(":").pop();
        if (v.datasetLabel.split(":", 1) != 'current') {
            _yearNumber = _yearNumber - 1;
            var _invNumber = totalInvoicesFromArray(v, yearArray, 12);
        }
        else {
            var _invNumber = totalInvoicesFromArray(v, yearArray);
        }
        if (unit == "$")
            var _format = v.label + ' ' + yearFromArray(v, yearArray) + ' : ' + unit + numberWithCommas(v.value) + ' (' + _invNumber + ' invoice' + pluralise(_invNumber) + ')';
        else
            var _format = v.label + ' ' + yearFromArray(v, yearArray) + ' : ' + numberWithCommas(v.value) + ' ' + unit + ' (' + _invNumber + ' invoice' + pluralise(_invNumber) + ')';
        return (_format);
        // return v.label + ' ' + yearFromArray(v, yearArray) + ' : ' + '$' + v.value.toFixed(2);
    }

    function multiTooltip(v, yearArray) {
        var _yearNumber = yearFromArray(v, yearArray);
        
        var unit = v.datasetLabel.split(":").pop();
        if (v.datasetLabel.split(":", 1) != 'current') {
            _yearNumber = _yearNumber - 1;
            var _invNumber = totalInvoicesFromArray(v, yearArray, 12);
        }
        else {
            var _invNumber = totalInvoicesFromArray(v, yearArray);
        }
        if (unit == "$")
            var _format = _yearNumber + ' : ' + unit + numberWithCommas(v.value) + ' (' + _invNumber + ' invoice' + pluralise(_invNumber) +')';
        else
            var _format = _yearNumber + ' : ' + numberWithCommas(v.value) + ' ' + unit + ' (' + _invNumber + ' invoice' + pluralise(_invNumber) + ')';
        return (_format);
    }

    function pluralise(count) {
        // Very simple at the moment
        if (count.length >= 0)
            return 's';
        else
            return '';
    };

    function numberWithCommas(x) {
        return x.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

})();