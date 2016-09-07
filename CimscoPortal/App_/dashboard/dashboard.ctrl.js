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
        var _googleChartElements =
            [
                { elementName: "energyChargesChart", columnNames: ["Invoice Total excl GST", "Previous Year Total"], columns: [{ primary: [], all: [] }], title: "Energy Charges" },
                { elementName: "electricityConsumptionChart", columnNames: ["Total Kwh", "Previous Year Kwh"], columns: [{ primary: [], all: [] }], title: "Electricity Consumption" }
            ];
        var debugStatus_showMessages = false;
        //var showingPrior12 = false;
        $scope.pop = function () {
            toaster.pop('success', "title", "text");
        };

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
            //dbDataSource.getAllFilters()
            ////filterData.getAllFilters()
            //   .then(onFiltersOk, onError);
        };

        var onFiltersOk = function (data) {
            //createMultiDropdown('divisions', data.divisions, true);
            filterData.createMultiDropdown('divisions', data.divisions, true, $scope);
            //createMultiDropdown('categories', data.categories, true);
            filterData.createMultiDropdown('categories', data.categories, true, $scope);
        };

        var plotAllGraphs = function (data) {
            if (debugStatus_showMessages) { toaster.pop('success', "All data loaded!", "Read histogram and stats data from database") };
            onGraphData(data.cost, dbConstants.costsDataElement, 0);
            onGraphData(data.consumption, dbConstants.consDataElement, 1);
            displayStats(data.invoiceStats);
            $scope.loading = false;
        };

        var onGraphData = function (data, target, index) {
            // Assign data to graph and display
            yearArray = {
                years: data.years,
                months: data.months,
                invoices: data.totalInvoices,
                invoices12: data.totalInvoices12
            };
            var unit = ["$", "KWh"];
            currentData[index] = assignData(data.values, themeprimary, "current" + ":" + unit[index]);
            prior12Data[index] = assignData(data.values12, themesecondary, "minus12" + ":" + unit[index]);
            refreshData($scope.showPrevious12, currentData[index], prior12Data[index], target);
        }

        function onError(reason) {
            $scope.loading = false;
            toaster.pop('error', "Data Load Error", "Unable to load data from database! Status ID =" + reason.status);
           // console.log(reason);
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

        var getBusinessData = function () {
            $scope.loading = true;
            readAndPlotHistogram();
            readAndPlotGoogleGraphData();
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
            // var checkbox = $event.target;
            refreshData($scope.showPrevious12, currentData[0], prior12Data[0], dbConstants.costsDataElement);
            refreshData($scope.showPrevious12, currentData[1], prior12Data[1], dbConstants.consDataElement);

            refreshGoogleChart($scope.showPrevious12, _googleChartElements[googleChart.elementIndex(_googleChartElements, "Electricity Consumption")]);
            refreshGoogleChart($scope.showPrevious12, _googleChartElements[googleChart.elementIndex(_googleChartElements, "Energy Charges")]);
            //showingPrior12 = checkbox.checked;
            //$scope.loading = false;
        };


        $scope.reviseMonths = function (newMonthSpan) {
            $scope.loading = true;
            $scope.monthSpan = newMonthSpan;
            readAndPlotHistogram();
            readAndPlotGoogleGraphData();
            // $scope.loading = false;
        };

        $scope.toggleGraphType = function ($event) {
            $scope.loading = true;
            //window.dispatchEvent(new Event('resize'));
            _event();
            //var zoom = (window.outerWidth - 10) / window.innerWidth;
            //if (debugStatus_showMessages) { toaster.pop('warning', "Page zoom ", zoom); }
            $scope.loading = false;

            //var scale = 'scale(1)';
            //document.body.style.webkitTransform =       // Chrome, Opera, Safari
            // document.body.style.msTransform =          // IE 9
            // document.body.style.transform = scale;     // General

        };

        // GPA **--> common function
        var _event = function () {
           // console.log('event...');
            if (document.createEvent) { // W3C
                var ev = document.createEvent('Event');
                ev.initEvent('resize', true, true);
             //   console.log('dispatch event');
                window.dispatchEvent(ev);
            }
            else { // IE
              //  console.log('IE event');
                element = document.documentElement;
                var event = document.createEventObject();
                element.fireEvent("onresize", event);
            }
        };

        function refreshGoogleChart(showPreviousYear, chartElements) {
            var getter = $parse(chartElements.elementName + '.view');

            if (showPreviousYear) {
                var _columns = {
                    columns: chartElements.columns.all
                };
            }
            else {
                var _columns = {
                    columns: chartElements.columns.primary
                };
            }
            getter.assign($scope, _columns);
            $scope.loading = false;
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
        //var createMultiDropdown = function (baseName, selectionItemsList, createWatch) {
        //    // Create variables on scope
        //    var getter = $parse(baseName + 'Model');
        //    getter.assign($scope, []);

        //    getter = $parse(baseName + 'Data');
        //    getter.assign($scope, selectionItemsList);
        //    getter = $parse(baseName + 'CustomTexts');

        //    var buttonText = 'All ' + baseName.capitalizeFirstLetter();
        //    var customTexts = { buttonDefaultText: buttonText, uncheckAll: 'Clear Filters' };
        //    getter.assign($scope, customTexts);
        //    if (createWatch) {
        //        $scope.$watch(baseName + 'Model', function (data) {
        //            filterData(data, baseName);
        //        }, true);
        //    };

        //    var maxTextLength = buttonText.length;
        //    getter = $parse(baseName + 'Settings');
        //    getter.assign($scope, {
        //        smartButtonMaxItems: 1,
        //        externalIdProp: '',
        //        showCheckAll: false,
        //        smartButtonTextConverter: function (itemText, originalItem) {
        //            if (itemText.length > maxTextLength) {
        //                return itemText.substring(0, (maxTextLength - 2)) + '..';
        //            }
        //        }
        //    });

        //};

        //var filterData = function (data, baseName) {
        //    startDelay(data);
        //};

        //var stop;
        //var _counter = dbConstants.filterSelectDelay;//20;
        //var startDelay = function (data) {
        //    if (angular.isDefined(stop)) { _counter = dbConstants.filterSelectDelay; return; }
        //    stop = $interval(function () {
        //        if (_counter > 0) {
        //            _counter--;
        //        }
        //        else { stopCounter(data); }
        //    }, 100)
        //}
        //var stopCounter = function (data) {
        //    if (angular.isDefined(stop)) {
        //        $interval.cancel(stop);
        //        stop = undefined;
        //        _counter = dbConstants.filterSelectDelay;//20;
        //    };
        //    $scope.loading = true;
        //    readAndPlotHistogram();

        //    readAndPlotGoogleGraphData();
        //};

        //var getReturnIds = function () {
        //    var _returnIds = "_";
        //    angular.forEach($scope.categoriesModel, function (value, key) {
        //        _returnIds += value.id + "-";
        //    });
        //    _returnIds += "_";
        //    angular.forEach($scope.divisionsModel, function (value, key) {
        //        _returnIds += value.id + "-";
        //    });
        //    return _returnIds;
        //};

        var getReturnIds = function () {
            return filterData.createApiFilter($scope.categoriesModel, $scope.divisionsModel);
        }

        // Google chart control GPA: Refactor all code here, copied from SiteOverview
        var readAndPlotGoogleGraphData = function () {
            dbDataSource.getCostConsumptionData($scope.monthSpan, getReturnIds())
                .then(onGoogleGraphData, onError);
        };


        var onGoogleGraphData = function (data) {
            var _collatedData = googleChart.collateData(data);
            initializeGoogleChart(_collatedData, _googleChartElements[googleChart.elementIndex(_googleChartElements, "Electricity Consumption")]);
            initializeGoogleChart(_collatedData, _googleChartElements[googleChart.elementIndex(_googleChartElements, "Energy Charges")]);
            if (debugStatus_showMessages) { toaster.pop('success', "Google graph data loaded!", ""); }
            $scope.loading = false;
        };

        //console.log(googleChart.configureChart());

        function initializeGoogleChart(data, chartElements) {
            // data.axes defined the Axes labels and formats.
            // Axes 0 == X
            // Remaining Axes are Y

            var chartElementName = chartElements.elementName;

            var getter = $parse(chartElementName + '.type');
            getter.assign($scope, 'LineChart');

            getter = $parse(chartElementName + '.data');
            getter.assign($scope,
                {
                    "cols": data.cols,
                    "rows": data.rows
                }
                );

            // Restrict data displayed on this chart to specified Axes.
            var selectedColumnNames = [];
            selectedColumnNames.push(chartElements.columnNames[0]);

            var _axesToDisplay = googleChart.selectAxesByName(selectedColumnNames, data.axes);

            chartElements.columns.primary = _axesToDisplay.columns;

            getter = $parse(chartElementName + '.options');
            getter.assign($scope,
                googleChart.configureChart(chartElements.title, _axesToDisplay.axes, data.axes[0].title)
                );
            chartElements.columns.all = googleChart.displayAxesByName(chartElements.columnNames, data.axes);
            refreshGoogleChart($scope.showPrevious12, chartElements);
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
        //    console.log(pluralise(_invNumber));
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
        //   console.log(pluralise(_invNumber));
        if (unit == "$")
            var _format = _yearNumber + ' : ' + unit + numberWithCommas(v.value) + ' (' + _invNumber + ' invoice' + pluralise(_invNumber) + ')';
        else
            var _format = _yearNumber + ' : ' + numberWithCommas(v.value) + ' ' + unit + ' (' + _invNumber + ' invoice' + pluralise(_invNumber) + ')';
        return (_format);
    }

    function pluralise(count) {
        // Very simple at the moment
        //  console.log(count);
        if (count.length >= 0)
        { return "s"; }
        else
        { return ""; }
    };

    function numberWithCommas(x) {
        return x.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

})();


//data.cols = [];
//var colpart = { id: "2", label: "Month", title: "Month", type: "string" };
//data.cols.push(colpart);
//var colpart = { id: "3", title: "Invoice Total excl GST", label: "Invoice Total excl GST", type: "number" };
//data.cols.push(colpart);
//var colpart = { id: "8", type: "string", role: "tooltip", p: { 'html': true } };
//data.cols.push(colpart);
//var colpart = { id: "4", type: "number", title: "Test", label: "test" };
//data.cols.push(colpart);
//var colpart = { id: "7", type: "string", role: "tooltip" };
//data.cols.push(colpart);
//var colpart = { id: "5", title: "Cost / Sqm", label: "Cost / Sqm", type: "number" };
//data.cols.push(colpart);
//var colpart = { id: "6", title: "Kwh / SqM", label: "Kwh / SqM", type: "number" };
//data.cols.push(colpart);

//data.rows = [];
//var cpart = [{ v: "August", f: null }, { v: "$0.0", f: null }, { v: "$0.0", f: null }, { v: "$0.0", f: null }, { v: "$0.0", f: null }, { v: "$0.0", f: null }, { v: "$0.0", f: null }];
//data.rows.push({ c: cpart });
//var cpart = [{ v: "September", f: null }, { v: "32218.3900", f: "$32,218.39 (3 invoices)" }, { f: "<div style='width:150px; margin:10px;'>September</br><b>$32,218.39</br>(3 invoices)</b></div>" },
//                { v: "42218.3900", f: "$32,218.39 (3 invoices)" }, { f: "A label" },
//                { v: "52218.3900", f: "$32,218.39 (3 invoices)" }, { v: "62218.3900", f: "$32,218.39 (3 invoices)" }];
//data.rows.push({ c: cpart });