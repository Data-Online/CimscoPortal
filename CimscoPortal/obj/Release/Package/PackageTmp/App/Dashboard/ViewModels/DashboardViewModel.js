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

            $scope.updateTest = function (customerId) {
                console.log('refresh morris chart 2 for customer Id:' + customerId);
                morris.setData($scope.InvoiceSummary[customerId - 1].invoiceHistory);
                $scope.customerName = $scope.hierarchyData.customerData[customerId - 1].customerName;
                $scope.invoiceData = $scope.InvoiceSummary[customerId - 1].invoicesDue;
            }
        };

        var onRepo2 = function (data) {
            $scope.InvoiceSummary = data.summaryData;
            $scope.hierarchyData = data.customerHierarchy;
            //$scope.invoicesDue = data.invoicesDue;
            $scope.invoiceData = data.summaryData[0].invoicesDue;
            console.log('company data summary call 2');
            $scope.xkey = 'month';
            $scope.ykeys = ['yearA', 'yearB'];
            $scope.labels = [$scope.InvoiceSummary[0].year, $scope.InvoiceSummary[1].year];
            $scope.myModel = $scope.InvoiceSummary[0].invoiceHistory;//data.summaryData[0].invoiceHistory;
            $scope.customerName = $scope.hierarchyData.customerData[0].customerName;
            $scope.maxValue = data.maxValue
            barchart();
        };

        var onError = function (reason) {
            $scope.reason = reason;
        };

        dbDataSource.getSummaryData()
            .then(onRepo2, onError);

    };

    module.controller("dashboardViewModel", dashboardViewModel);

}());