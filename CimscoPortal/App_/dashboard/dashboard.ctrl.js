(function () {
    'use strict';

    angular
        .module("app.dashboard")
        .constant("graphElements", { "costsDataElement": "costsBarChart", "consDataElement": "consBarChart" })
        .controller("app.dashboard.ctrl", dashboard);

    dashboard.$inject = ['$scope', '$parse', '$interval', 'dbDataSource', 'userDataSource', 'graphElements'];
    function dashboard($scope, $parse, $interval, dbDataSource, userDataSource, graphElements) {

        $scope.topLevelName = '4 Sites';

        var divisions = [];
                    //[{ id: 1, label: 'MEGA Stores' },
                    //   { id: 2, label: 'Mitre 10 Stores' }];
        var categories = [];
            //[{ id: 1, label: 'Hardware and Building Supplies Retailing' },
            //           { id: 2, label: 'Another A' },
            //           { id: 3, label: 'Another B' }];


        var yearArray = {};
        var currentData = [];
        var prior12Data = [];

        userDataSource.getUserData()
            .then(onUserData, onError);

        function onUserData(data) {
            $scope.monthSpanOptions = data.monthSpanOptions;
            $scope.monthSpan = data.monthSpan;
            //console.log(data.monthSpanOptions + " " + data.monthSpan);

            // $scope.topLevelName = data.topLevelName;
            var companyId = 0;
            dbDataSource.getTotalCostsByMonth(data.monthSpan, companyId)
                .then(function success(data) { return onGraphData(data, graphElements.costsDataElement, 0) }, onError);

            dbDataSource.getTotalConsumptionByMonth(data.monthSpan)
                .then(function success(data) { return onGraphData(data, graphElements.consDataElement, 1) }, onError);

            dbDataSource.getAllFilters()
                .then(onFiltersOk, onError);
        };

        var onFiltersOk = function (data) {
//            console.log('Divisions : ' + divisions);
            createMultiDropdown('divisions', data.divisions, true);
            createMultiDropdown('categories', data.categories, true);
        };

        function onGraphData(data, target, index) {
            // console.log("Graph data " + (index + 1));

            // Assign data to graph and display
            yearArray = {
                years: data.years,
                months: data.months
            };

            currentData[index] = assignData(data.values, themeprimary, "current");
            prior12Data[index] = assignData(data.values12, themesecondary, "minus12");
            refreshData(false, currentData[index], prior12Data[index], target);
        }

        function onError(reason) {
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


        $scope.togglePreviousYearsData = function ($event) {
            var checkbox = $event.target;
            refreshData(checkbox.checked, currentData[0], prior12Data[0], graphElements.costsDataElement);
            refreshData(checkbox.checked, currentData[1], prior12Data[1], graphElements.consDataElement);
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
                    filterData(data);
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

        console.log('Categories : ' + categories);

//        createMultiDropdown('divisions', divisions, true);
//        createMultiDropdown('categories', categories, true);

        var filterData = function (data) {
            startDelay(data);
            //console.log('trigger');
           // console.log(data);
        };

        var stop;
        var _counter = 20;
        var startDelay = function (data) {
            if (angular.isDefined(stop)) { _counter = 20; return; }
            stop = $interval(function () {
                if (_counter > 0) {
                    _counter--;
                    //console.log(_counter);
                }
                else { stopCounter(data); }
            },100)
        }
        var stopCounter = function (data) {
           // console.log('stopping counter...');
            if (angular.isDefined(stop)) {
                $interval.cancel(stop);
                stop = undefined;
                _counter = 20;
            };
            console.log(data);

        };

    };

    String.prototype.capitalizeFirstLetter = function () {
        return this.charAt(0).toUpperCase() + this.slice(1);
    };
    var yearFromArray = function (v, yearArray) {
        return yearArray.years[yearArray.months.indexOf(v.label)];
    }

    function singleTooltip(v, yearArray) {
        //console.log(v);
        return v.label + ' ' + yearFromArray(v, yearArray) + ' : ' + '$' + v.value.toFixed(2);
    }
    function multiTooltip(v, yearArray) {
        var _yearNumber = yearFromArray(v, yearArray);
        //console.log(_yearNumber);
        if (v.datasetLabel != 'current') {
            _yearNumber = _yearNumber - 1;
        }
        return (_yearNumber + ' : ' + '$' + v.value.toFixed(2));
    }

})();