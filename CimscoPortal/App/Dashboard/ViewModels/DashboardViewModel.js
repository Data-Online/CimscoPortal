(function () {

    var module = angular.module("dashboard");

    var dashboardViewModel = function ($scope, dbDataSource) {
        $scope.pageElement = 'inv-hist-bar-chart2';
        var barchart = function () {
            console.log('creating morris chart 2');
            morris = Morris.Bar({
                element: $scope.pageElement,
                data: $scope.myModel,
                xkey: $scope.xkey,
                ykeys: $scope.ykeys,
                labels: $scope.labels,
                hideHover: 'auto',
                barColors: [themeprimary, themesecondary, themethirdcolor]
                // ,ymin: 0
               , ymax: $scope.maxValue
            });

            $scope.updateTest = function (siteIndex) {
                var offSet = 3;
                console.log('refresh morris chart 2 for site Id:' + (siteIndex - offSet));
                morris.setData($scope.InvoiceSummary[siteIndex - offSet].invoiceHistory);
                $scope.customerName = $scope.siteHierarchyData.siteData[siteIndex - offSet].siteName;
                $scope.invoiceData = $scope.InvoiceSummary[siteIndex - offSet].invoicesDue;
            }

        };

        var onRepo2 = function (data) {
            $scope.InvoiceSummary = data.summaryData;
            //$scope.hierarchyData = data.customerHierarchy;
            $scope.siteHierarchyData = data.siteHierarchy;
            //$scope.invoicesDue = data.invoicesDue;
            $scope.invoiceData = data.summaryData[0].invoicesDue;
            console.log('company data summary call 2');
            $scope.xkey = 'month';
            $scope.ykeys = ['yearA', 'yearB'];
            $scope.labels = ["$", "$"];// [$scope.InvoiceSummary[0].year, $scope.InvoiceSummary[1].year];
            $scope.myModel = $scope.InvoiceSummary[0].invoiceHistory;//data.summaryData[0].invoiceHistory;
            $scope.customerName = $scope.siteHierarchyData.siteData[0].siteName;
            $scope.maxValue = data.maxValue
            $scope.ldgInvHistory = false;
            $scope.loadingOpacity = "1.0";
            barchart();
        };

        $scope.testclick = function (setting, invoiceId, index) {
            console.log('setting change: ' + setting + ' for ' + invoiceId + ' site Index ' + index);
            dbDataSource.postInvoiceApproval(invoiceId);
            $scope.invoiceData.splice(index, 1);
        };

        $scope.removeInvoice = function (index) {
            console.log('item index = ' + index);
            $scope.invoiceData.splice(index, 1);
        };

        var onError = function (reason) {
            $scope.reason = reason;
        };

        $scope.ldgInvHistory = true;
        $scope.loadingOpacity = "0.5";
        dbDataSource.getSummaryData()
            .then(onRepo2, onError);

        //dbDataSource.postInvoiceApproval();
        $scope.setOpacity = function (myValue) {
           //console.log('Opacity set for ' + myValue);
            var opacity = 1;
            if ((myValue).charAt(0) == 't') {
                console.log('Opacity set');
                opacity = "0.5";
            }
            return { "opacity": opacity };
        };

        $scope.pctBoxStyle = function (myValue) {
            var num = parseInt(myValue);
            var style = 'databox-stat radius-bordered';
            if (num > 0) {
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
            if (num > 0) {
                style = style + ' fa fa-long-arrow-up';
            }
            else if (num < 0) {
                style = style + ' fa fa-long-arrow-down';
            }
            //console.log('Style for value ' + num +' set to ' + style);
            return style;
        };

    };

    module.controller("dashboardViewModel", dashboardViewModel);

}());