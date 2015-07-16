(function () {

    var module = angular.module("invoice");

    var invoiceDetailViewModel = function ($scope, inDataSource) {

        $scope.bcPageElement = 'invHistoryChart';
        $scope.doPageElement = 'donutChart';


        $scope.init = function (invoiceId) {
            $scope.invoiceId = invoiceId;
        }

        var stackedbarchart = function () {
            console.log('Creating invoice summary chart: ' + $scope.invoiceId);
            morris = Morris.Bar({
                element: $scope.bcPageElement,
                data: $scope.bcModel,
                xkey: $scope.xkey,
                ykeys: $scope.ykeys,
                labels: $scope.labels,
                hideHover: 'auto',
                stacked: true,
                barColors: [themeprimary, themesecondary, themethirdcolor]
                //,
                //hoverCallback: function (index, options, content) {
                //    var data = options.data[index];
                //    $(".morris-hover").html('<div>Custom label: ' + data.label + '</div>');
                //}
                // ,ymin: 0
                //, ymax: $scope.maxValue
            });
        };

        var donutchart = function () {
            console.log('Creating invoice detail donut: ' + $scope.invoiceId);
            morris = Morris.Donut({
                element: $scope.doPageElement,
                data: $scope.doModel,
                colors: [themeprimary, themethirdcolor, themesecondary, themefourthcolor],
                formatter: function (y) { return "$" + y }
            });
        }

        var donutchart_ = function () {
            console.log('Creating invoice detail donut new for invoice ID: ' + $scope.invoiceId);
            morris = Morris.Donut({
                element: $scope.doPageElement,
                data: $scope.donutChartData,
                colors: [themeprimary, themethirdcolor, themesecondary, themefourthcolor],
                formatter: function (y) { return "$" + y }
            });
        }

        var onSummaryData = function (data) {
            console.log('Got invoice summary data');
            $scope.xkey = 'month';
            $scope.ykeys = ['energy', 'line', 'other'];
            $scope.labels = ['Energy', 'Network', 'Other'];
            $scope.bcModel = data.monthlyData;
            $scope.summaryData = data.barChartSummaryData;
            console.log($scope.summaryData.percentChange);
            if ($scope.summaryData.percentChange < 0) {
                $scope.summaryData.percentChange = $scope.summaryData.percentChange * -1;
                console.log('negative change');
                $scope.summaryData.arrow = 'fa-arrow-down';
                $scope.summaryData.arrowColour = 'bg-palegreen';
            }
            else {
                $scope.summaryData.arrow = 'fa-arrow-up';
                $scope.summaryData.arrowColour = 'bg-warning';
            }
            stackedbarchart();
        };

        var onDetailData = function (data) {
            console.log('Got invoice detail data');
            $scope.doModel = data.chartData.donutChartData;
            $scope.doHeader = data.chartData.headerData;
            $scope.doSummary = data.chartData.summaryData;
            $scope.doApproval = data.chartData.approvalData;
            $scope.slModel = data.energyCosts.energyCostSeries;
            //$scope.summaryData = data.barChartSummaryData;
            donutchart();
        };

        var onDetailData_ = function (data) {
            console.log('Got invoice detail data new');
            //$scope.zztest = themeprimary;
            $scope.donutChartData = data.donutChartData;
            $scope.invoiceDetail = data.invoiceDetail;
            //$scope.doHeader = data.chartData.headerData;
            //$scope.doSummary = data.chartData.summaryData;
            //$scope.doApproval = data.chartData.approvalData;
            $scope.slModel = data.energyCosts;
            $scope.slModel.maxYbar = Math.max(data.energyCosts.energyCostSeries[0].maxCharge, data.energyCosts.energyCostSeries[1].maxCharge);
            $scope.slModel.minYbar = $scope.slModel.maxYbar * 0.1;
            $scope.slModel.maxYgraph = Math.max(data.energyCosts.energyCostSeries[0].maxRate, data.energyCosts.energyCostSeries[1].maxRate);
            $scope.slModel.minYgraph = $scope.slModel.maxYgraph * 0.1;
            console.log('Max bar : ' + $scope.slModel.maxYbar + ' max graph : ' + $scope.slModel.maxYgraph);
            donutchart_();
        };

        var onDetailData_pt = function (data) {
            console.log('Got refresh data');
            //$scope.donutChartData = data.donutChartData;
            $scope.invoiceDetail = data.invoiceDetail;
            //$scope.doHeader = data.chartData.headerData;
            //$scope.doSummary = data.chartData.summaryData;
            //$scope.doApproval = data.chartData.approvalData;
            //$scope.slModel = data.energyCosts;
            donutchart_();
        };

        var onError = function (reason) {
            $scope.reason = reason;
        };

        $scope.$watch(function (scope) { return scope.invoiceId },
            function () {
                inDataSource.getInvSummary($scope.invoiceId)
                .then(onSummaryData, onError);
                inDataSource.getInvDetail_($scope.invoiceId)
              .then(onDetailData_, onError);

            }
                );

        var onDetailRefresh = function () {
            inDataSource.getInvDetail_($scope.invoiceId)
              .then(onDetailData_pt, onError);
        };

        $scope.approveInvoice = function (setting, invoiceId) {
            console.log('setting change: ' + setting + ' for ' + invoiceId);
            inDataSource.postInvoiceApproval(invoiceId).then(onDetailRefresh, onError);
        };

        //$scope.Customers = "4,6,8,10,5";
        //$scope.Customers2 = "4,12,9,12,5";
    };

    module.controller("invoiceDetailViewModel", invoiceDetailViewModel);


    //module.directive("sparklinechart", function () {

    //    return {
    //        restrict: "E",
    //        scope: {
    //            data: "@"
    //        },
    //        compile: function (tElement, tAttrs, transclude) {
    //            tElement.replaceWith("<span>" + tAttrs.data + "</span>");
    //            return function (scope, element, attrs) {
    //                attrs.$observe("data", function (newValue) {
    //                    element.html(newValue);
    //                    console.log(scope.customers2);
    //                    element.sparkline('html', { type: 'bar', width: '96%', height: '80px', barWidth: 11, barColor: 'blue' });
    //                    element.sparkline(scope.customers2, { type: 'line', width: '96%', height: '80px', barWidth: 11, barColor: 'blue', composite: true });
    //                });

    //            };
    //        }
    //    };
    //});
    module.filter('percentage', ['$filter', function ($filter) {
        return function (input, decimals) {
            return $filter('number')(input * 100, decimals) + '%';
        };
    }]);


}());