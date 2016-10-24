(function () {

    var module = angular.module("app.invoiceDetail");

    var invoiceDetail = function ($scope, idDataSource, icDataSource, commonTools, dataFormatting) {

        $scope.bcPageElement = 'invHistoryChart';
        $scope.doPageElement = 'donutChart';
        $scope.pctBoxStyle = dataFormatting.pctBoxStyle;
        $scope.negativeValue = dataFormatting.negativeValue;

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
            console.log('Creating invoice detail donut (% based) new for invoice ID: ' + $scope.invoiceId);
            morris = Morris.Donut({
                element: $scope.doPageElement,
                data: $scope.donutChartData,
                colors: [themeprimary, themethirdcolor, themesecondary, themefourthcolor],
                //formatter: function (y) { return "$" + y }
                formatter: function (y) { return y + "%" }
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
            console.log('Got invoice detail data new V2');
            //$scope.zztest = themeprimary;
            $scope.tools = commonTools;
            $scope.donutChartData = data.donutChartData;
            $scope.invoiceDetail = data.invoiceDetail;
            //$scope.doHeader = data.chartData.headerData;
            //$scope.doSummary = data.chartData.summaryData;
            //$scope.doApproval = data.chartData.approvalData;
            $scope.slModel = data.energyCosts;
            $scope.otherCharges = data.otherCharges;
            $scope.networkCharges = data.networkCharges;
            $scope.ncTotal = data.networkCharges[0] + data.networkCharges[1] + data.networkCharges[2] + data.networkCharges[3] + data.networkCharges[4];
            $scope.ocTotal = data.otherCharges[0] + data.otherCharges[1] + data.otherCharges[2] + data.otherCharges[3];
            $scope.slModel.maxYbar = Math.max(data.energyCosts.energyCostSeries[0].maxCharge, data.energyCosts.energyCostSeries[1].maxCharge);
            $scope.slModel.minYbar = $scope.slModel.maxYbar * 0.1;
            $scope.slModel.maxYgraph = Math.max(data.energyCosts.energyCostSeries[0].maxRate, data.energyCosts.energyCostSeries[1].maxRate);
            $scope.slModel.minYgraph = $scope.slModel.maxYgraph * 0.1;
            //$scope.slModel.lossRate = $scope.invoiceDetail.lossRate;
            $scope.invoiceDetail.arrow = $scope.tools.arrowType(data.invoiceDetail.percentageChange);

            //console.log('Max bar : ' + $scope.slModel.maxYbar + ' max graph : ' + $scope.slModel.maxYgraph);
            console.log("MODEL");
            console.log($scope.slModel);

            if ((data.invoiceDetail.invoiceNumber).charAt(0) == 't') {
                console.log('sample invoice!');
                $scope.invoiceDetail.invoiceIcon = 'fa-magic';
            }

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

        var onHistoryData = function (data) {
            console.log('Got history data');
            var ticks = [];
            var totalCost = [];
            var totalConsumption = [];
            for (var i in data) {
                ticks.push([i, data[i].month]);
                totalCost.push([i, data[i].avgCost]);
                totalConsumption.push([i, data[i].avgConsumption]);
            }
            historyChart(ticks, totalCost, totalConsumption);
        };

        var onError = function (reason) {
            $scope.reason = reason;
        };

        $scope.$watch(function (scope) { return scope.invoiceId },
            function () {
                idDataSource.getInvSummary($scope.invoiceId)
                    .then(onSummaryData, onError);
                idDataSource.getInvDetail_($scope.invoiceId)
                    .then(onDetailData_, onError);
                idDataSource.getHistoryData($scope.invoiceId)
                    .then(onHistoryData, onError);
            }
                );

        var onDetailRefresh = function () {
            idDataSource.getInvDetail_($scope.invoiceId)
              .then(onDetailData_pt, onError);
        };

        $scope.approveInvoice = function (setting, invoiceId) {
            console.log('setting change: ' + setting + ' for ' + invoiceId);
            icDataSource.postInvoiceApproval(invoiceId).then(onDetailRefresh, onError);
        };

        //$scope.Customers = "4,6,8,10,5";
        //$scope.Customers2 = "4,12,9,12,5";

        var historyChart = function (ticks, totalCost, totalConsumption) {
            $scope.SelectableChartData = [
                {
                    //color: $rootScope.settings.color.themeprimary,
                    label: "Cost",
                    data: totalCost,
                    bars: {
                        show: true,
                        align: "right",
                        fill: true,
                        lineWidth: 1,
                        barWidth: 0.25
                    },
                    //lines: { show: false },
                    yaxis: 1
                },
                {
                    //color: $rootScope.settings.color.themefourthcolor,
                    label: "Consumption",
                    data: totalConsumption,
                    //lines: {
                    //    show: true,
                    //    fill: false
                    //},
                    bars: {
                        show: true,
                        align: "left",
                        fill: true,
                        lineWidth: 1,
                        barWidth: 0.25
                    },
                    points: { show: true },
                    yaxis: 2
                }
            ];

            $scope.SelectableChartOptions = {
                //series: {
                //    lines: {
                //        show: true
                //    },
                //    points: {
                //        show: true
                //    }
                //},
                legend: {
                    noColumns: 2
                },
                xaxes: [{
                    tickDecimals: 0,
                    color: '#eee',
                    ticks: ticks
                }],
                yaxes: [
                {
                    tickFormatter: function (val, axis) {
                        return "$" + val;
                    },
                    position: "right",
                    axisLabel: "Cost",
                    axisLabelUseCanvas: true,
                    axisLabelFontSizePixels: 12,
                    axisLabelFontFamily: "Verdana, Arial, Helvetica, Tahoma, sans-serif",
                    axisLabelPadding: 25
                },
                {
                    tickFormatter: function (val, axis) {
                        return val;
                    },
                    min: 0,
                    position: "left",
                    axisLabel: "Consumption",
                    axisLabelUseCanvas: true,
                    axisLabelFontSizePixels: 12,
                    axisLabelFontFamily: "Verdana, Arial, Helvetica, Tahoma, sans-serif",
                    axisLabelPadding: 25
                }],
                selection: {
                    mode: "x"
                },
                grid: {
                    hoverable: true,
                    clickable: false,
                    borderWidth: 0,
                    aboveData: false
                },
                tooltip: true,
                tooltipOpts: {
                    defaultTheme: false,
                    content: "<b>%s</b> : <span>%y</span>",
                },
                crosshair: {
                    mode: "x"
                }
            };
        };

        $scope.randomStacked = function () {
            var values = [];
            var ztest = [22, 45, 47]; // low, this, max
            var units = (ztest[2] - ztest[0] )/ 100;
            values[0] = ((ztest[1] - ztest[0]) / units) - 1;
            values[1] = 2.0;
            values[2] = 100 - values[0] - values[1];
            //var values = [91, 2, 7];

            $scope.stacked = [];
            var types = ['info','success', 'info']; //, 'warning', 'danger'];
            values.forEach(setValues);
            
            //for (var i = 0, n = Math.floor((Math.random() * 4) + 1) ; i < n; i++) {
            //    var index = Math.floor((Math.random() * 4));
            //    $scope.stacked.push({
            //        value: Math.floor((Math.random() * 30) + 1),
            //        type: types[index]
            //    });
            //}
            function setValues(value, index)
            {
                console.log(index);
                $scope.stacked.push({
                    value: value,
                    type: types[index]
                });
            }
            console.log("Stack values:");
            console.log($scope.stacked);
        };

        $scope.randomStacked();

    ////    $scope.SelectableChartData = [
    ////{
    ////    //color: $rootScope.settings.color.themeprimary,
    ////    label: "Windows",
    ////    data: [[1990, 18.9], [1991, 18.7], [1992, 18.4], [1993, 19.3], [1994, 19.5], [1995, 19.3], [1996, 19.4], [1997, 20.2], [1998, 19.8], [1999, 19.9], [2000, 20.4], [2001, 20.1], [2002, 20.0], [2003, 19.8], [2004, 20.4]],
    ////    bars: {
    ////        show: true,
    ////        align: "center",
    ////        fill: true,
    ////        lineWidth: 1
    ////    },
    ////    //lines: { show: false },
    ////    yaxis: 1
    ////},
    ////{
    ////    //color: $rootScope.settings.color.themefourthcolor,
    ////    label: "DOS",
    ////    data: [[1990, 8.3], [1991, 8.3], [1992, 7.8], [1993, 8.3], [1994, 8.4], [1995, 5.9], [1996, 6.4], [1997, 6.7], [1998, 6.9], [1999, 7.6], [2000, 7.4], [2001, 8.1], [2002, 12.5], [2003, 9.9], [2004, 19.0]],
    ////    lines: {
    ////        show: true,
    ////        fill: false
    ////    },
    ////    points: { show: true },
    ////    yaxis: 2
    ////}
    ////    ];

    ////    $scope.SelectableChartOptions = {
    ////        //series: {
    ////        //    lines: {
    ////        //        show: true
    ////        //    },
    ////        //    points: {
    ////        //        show: true
    ////        //    }
    ////        //},
    ////        legend: {
    ////            noColumns: 4
    ////        },
    ////        xaxis: {
    ////            tickDecimals: 0,
    ////            color: '#eee'
    ////        },
    ////        yaxes: [
    ////        {
    ////            min: 0,
    ////            color: '#eee'
    ////        },
    ////        {
    ////            position: 0,
    ////            axisLabel: "Temperature",
    ////            axisLabelUseCanvas: true,
    ////            axisLabelFontSizePixels: 12,
    ////            axisLabelFontFamily: "Verdana, Arial, Helvetica, Tahoma, sans-serif",
    ////            axisLabelPadding: 5
    ////        }],
    ////        selection: {
    ////            mode: "x"
    ////        },
    ////        grid: {
    ////            hoverable: true,
    ////            clickable: false,
    ////            borderWidth: 0,
    ////            aboveData: false
    ////        },
    ////        tooltip: true,
    ////        tooltipOpts: {
    ////            defaultTheme: false,
    ////            content: "<b>%s</b> : <span>%x</span> : <span>%y</span>",
    ////        },
    ////        crosshair: {
    ////            mode: "x"
    ////        }
    ////    };


    };

    module.controller("app.core.invoiceDetail", invoiceDetail);


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