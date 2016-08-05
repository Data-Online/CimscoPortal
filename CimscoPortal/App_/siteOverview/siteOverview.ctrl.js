(function () {
    'use strict';

    var module = angular.module("app.siteOverview")
        .controller("app.siteOverview.ctrl", siteOverview);

    siteOverview.$inject = ['$scope', '$timeout', '$filter', 'soDataSource', 'userDataSource', 'toaster'];
    function siteOverview($scope, $timeout, $filter, soDataSource, userDataSource, toaster) {

        var siteId = 0;
        var monthSpan = 12;

        // Determine correct siteId from from MVC model
        $scope.setSiteId = function (_siteId) {
            //console.log("Set site Id to" + _siteId);
            siteId = _siteId;
        };

        // Core Data
        var getGraphData = function () {
            soDataSource.getCostConsumptionData(siteId, monthSpan)
                .then(onGraphData, onError);
        };

        var getSiteInvoiceData = function () {
            soDataSource.getSiteInvoiceData(siteId, monthSpan)
                 .then(onInvoiceData, onError);
        };

        var getSiteData = function () {
            soDataSource.getSiteDetails(siteId)
                .then(onSiteData, onError);
        };

        var onUserData = function (data) {
            $scope.loading = true;
            $scope.monthSpanOptions = data.monthSpanOptions;
            $scope.monthSpan = data.monthSpan;
            monthSpan = data.monthSpan;
            toaster.pop('success', "User Data Loaded!", "User specific data loaded");
           // console.log('Get data');
            getSiteInvoiceData();
            getGraphData();

            getSiteData();
            

        };

        var onSiteData = function (data) {
            toaster.pop('success', "Site Date Loaded!", "");
            $scope.tabTableHeader = data.siteName;
            $scope.siteData = data;
           // console.log(data);
        };

        var onInvoiceData = function (data) {
            //console.log('Get data call');
            $scope.monthSpan = monthSpan;
            $scope.invoiceData = data;
            $scope.loading = false;
            toaster.pop('success', "Invoice Data Loaded!", "");
        };

        var onError = function (reason) {
            $scope.loading = false;
            toaster.pop('error', "Data Load Error", "Unable to load data from database!");
            console.log('An error occured : ' + reason.status);
            $scope.reason = "There was a problem (code:" + reason.status + ")";
        };

        // Get all data, starting with User Data
        userDataSource.getUserData()
            .then(onUserData, onError);


        // Common front end functions
        $scope.reviseMonths = function (newMonthSpan) {
            $scope.loading = true;
            monthSpan = newMonthSpan;

            getSiteInvoiceData();

            // Only refresh if this is the current tab
            var assignedClasses = document.getElementById("consumption").className;
            if (assignedClasses.indexOf("active") > 0)
                getGraphData();
        };

        // Invoice display control
        $scope.selectItems = {
            showAll: true,
            showApproved: true,
            showToApprove: true,
            showMissing: true
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.selectItems.showAll;
            angular.forEach($scope.selectItems, function (value, key) { $scope.selectItems[key] = toggleStatus });
        };

        // Front end actions - Invoices
        $scope.order = function (predicate) {
            if ($scope.predicate != predicate) { $scope.sortArrow = "fa-long-arrow-down" };
            $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : true;
            $scope.predicate = predicate;
            if ($scope.reverse) {
                $scope.sortArrow = "fa-long-arrow-up";
            }
            else {
                $scope.sortArrow = "fa-long-arrow-down";
            }
        };

        $scope.approveInv = function (setting, invoiceId) {
            soDataSource.postInvoiceApproval(invoiceId)
                .then(function success(data) { return invoiceApproved(data, invoiceId) }, onError);
        };

        var invoiceApproved = function (result, invoiceId) {
          //  console.log("Approved ok for invoiceId : " + invoiceId);
            var foundIndex = $filter('filter')($scope.invoiceData, { invoiceId: invoiceId }, true)[0].$id - 1;

            $scope.invoiceData[foundIndex].approversName = result.data.approversName;
            $scope.invoiceData[foundIndex].approvedDate = result.data.approvedDate;
            $scope.invoiceData[foundIndex].approved = result.data.approved;

            toaster.pop('info', "Approved!", "Notification email has been sent");
        };


        $scope.tabTableHeader = '*site name*';

        $scope.invTheme = [{ "theme": "success", "text": "Approved" }, { "theme": "info", "text": "To Approve" }, { "theme": "warning", "text": "Missing" }];

        $scope.removeInvoiceByIndex = function (index) {
            //console.log('Removing item index = ' + index);
            $scope.invoiceData.splice(index, 1);
        };

        $scope.pctBoxStyle = function (myValue) {
            var num = parseInt(myValue);
            var style = 'databox-stat radius-bordered';
            if (num <= -999) {
                style = style + ' hide-element';
            }
            else if (num > 0) {
                style = style + ' bg-warning';
            }
            else if (num < 0) {
                style = style + ' bg-green';
            }
            else {
                style = style + ' bg-sky';
            }
            return style;
        };

        $scope.negativeValue = function (myValue) {
            var num = parseInt(myValue);
            var style = 'stat-icon';
            if (num == -999) {

            }
            else if (num == 0) {
                style = style + ' fa fa-arrows-h';
            }
            else if (num > 0) {
                style = style + ' fa fa-long-arrow-up';
            }
            else if (num < 0) {
                style = style + ' fa fa-long-arrow-down';
            }
            // console.log('Style for value ' + num +' set to ' + style);
            return style;
        };


        // Google Chart control
        // Properties
        $scope.myChartObject = {};

        //Methods
        $scope.hideSeries = hideSeries;

        var _cols = [];
        var _rows = [];
        var _cpart = [];
        var onGraphData = function (data) {
            _cols = []; _rows = []; _cpart = [];
            for (var i = 0; i < data.columns.length; i++) {
                var _nextCol = { id: data.columns[i].$id, title: data.columns[i].label, label: data.columns[i].label, type: data.columns[i].type };
                _cols.push(_nextCol);
            }
            for (var i = 0; i < data.rows.length; i++) {
                for (var j = 0; j < data.rows[i].cparts.length; j++) {
                    var c = { v: data.rows[i].cparts[j].v, f: data.rows[i].cparts[j].f }
                    _cpart.push(c);
                }
                _rows.push({ c: _cpart });
                _cpart = [];
            }
            initializeChart();
            toaster.pop('success', "Graph data loaded!", "");
            $scope.loading = false;
        };


        function hideSeries(selectedItem) {
            //// Disabled for time being
            //var col = selectedItem.column;
            //console.log(col);
            //if (selectedItem.row === null) {
            //    if ($scope.myChartObject.view.columns[col] == col) {
            //        $scope.myChartObject.view.columns[col] = {
            //            label: $scope.myChartObject.data.cols[col].label,
            //            type: $scope.myChartObject.data.cols[col].type,
            //            calc: function () {
            //                return null;
            //            }
            //        };
            //        $scope.myChartObject.options.colors[col - 1] = '#CCCCCC';
            //    }
            //    else {
            //        $scope.myChartObject.view.columns[col] = col;
            //        $scope.myChartObject.options.colors[col - 1] = $scope.myChartObject.options.defaultColors[col - 1];
            //    }
            //}
        }

        var seriesTypes = [{ label: "By SqM", columns: [0, 3, 4] }, { label: "By Totals", columns: [0, 1, 2] }];
        $scope.toggleAxes = function (label) {
            console.log('toggle...');
            var index = findIndexFromLabel(label, seriesTypes);
            var columns = seriesTypes[index].columns;
            $scope.myChartObject.view = {
                columns: columns
            };
            for (var i = 1; i < seriesTypes[index].columns.length; i++) {
                $scope.myChartObject.options.vAxes[i - 1].title = _cols[seriesTypes[index].columns[i]].label;
            }
            $scope.unitTypeForGraph = seriesTypes[1 - index].label;
        };

        var findIndexFromLabel = function (label, array) {
            for (var i = 0; i < array.length; i++) {
                if (array[i].label == label) {
                    return i;
                    break;
                }
            }
            return 0;
        };

        $scope.updateCC = function () {
            window.dispatchEvent(new Event('resize'));
//            getGraphData();
        };

        function initializeChart() {
            $scope.myChartObject.type = "LineChart";//"BarChart";// 
            $scope.myChartObject.displayed = false;
            $scope.myChartObject.data = {
                "cols": _cols,
                "rows": _rows
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
            $scope.toggleAxes("By Totals");
        }
    };

}());