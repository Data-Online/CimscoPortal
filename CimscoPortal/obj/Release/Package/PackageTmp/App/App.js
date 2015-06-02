(function () {
    var app = angular.module("portalEnvironment", ["ngRoute"]);
    app.config(function ($routeProvider) {
        $routeProvider
            .when("/main", {
                templateUrl: "/App/Dashboard/TestPage.html"
                //,controller: "MainController"
            })
            .when("/main2", {
                //templateUrl: "/App/Dashboard/TestPage1.html",
                controller: "MorrisCtrl"
            })
            .otherwise({ redirectTo: "/main" });
    });


    // Test for Morris chart
    var dbInvoiceValues = function ($scope, coDataSource) {
        $scope.pageElement = 'inv-hist-bar-chart';
        var barchart = function () {
            console.log('creating morris chart');
            morris = Morris.Bar({
                element: $scope.pageElement,
                data: $scope.myModel,
                xkey: $scope.xkey,
                ykeys: $scope.ykeys,
                labels: $scope.labels,
                hideHover: 'auto',
                ymin: 0,
                ymax: 12000

            });
            $scope.zztest = function () {
                console.log('inside update code');
                $scope.myModel = [{ "$id": "1", "month": "Jan", "yearA": "9943", "yearB": "11981" }, { "$id": "2", "month": "Feb", "yearA": "10787", "yearB": "8625" }, { "$id": "3", "month": "Mar", "yearA": "9520", "yearB": "9818" }, { "$id": "4", "month": "Apr", "yearA": "9628", "yearB": "11137" }, { "$id": "5", "month": "May", "yearA": "10232", "yearB": "10297" }, { "$id": "6", "month": "Jun", "yearA": "10083", "yearB": "9246" }, { "$id": "7", "month": "July", "yearA": "9311", "yearB": "10613" }, { "$id": "8", "month": "Aug", "yearA": "10163", "yearB": "9770" }, { "$id": "9", "month": "Sept", "yearA": "9648", "yearB": "11888" }, { "$id": "10", "month": "Oct", "yearA": "10451", "yearB": "8417" }, { "$id": "11", "month": "Nov", "yearA": "8357", "yearB": "11180" }, { "$id": "12", "month": "Dec", "yearA": "8932", "yearB": "10894" }];
                Morris.Bar({
                    element: $scope.pageElement,
                    data: $scope.myModel,
                    xkey: $scope.xkey,
                    ykeys: $scope.ykeys,
                    labels: $scope.labels,
                    hideHover: 'auto',
                    ymin: 0,
                    ymax: 12000
                });
            };
            $scope.zzztest = function () {
                console.log('refresh morris chart');
                var newModel = [{ "$id": "1", "month": "Jan", "yearA": "9943", "yearB": "10981" }, { "$id": "2", "month": "Feb", "yearA": "10787", "yearB": "8625" }, { "$id": "3", "month": "Mar", "yearA": "9520", "yearB": "9818" }, { "$id": "4", "month": "Apr", "yearA": "9628", "yearB": "11137" }, { "$id": "5", "month": "May", "yearA": "10232", "yearB": "10297" }, { "$id": "6", "month": "Jun", "yearA": "10083", "yearB": "9246" }, { "$id": "7", "month": "July", "yearA": "9311", "yearB": "10613" }, { "$id": "8", "month": "Aug", "yearA": "10163", "yearB": "9770" }, { "$id": "9", "month": "Sept", "yearA": "9648", "yearB": "11888" }, { "$id": "10", "month": "Oct", "yearA": "10451", "yearB": "8417" }, { "$id": "11", "month": "Nov", "yearA": "8357", "yearB": "11180" }, { "$id": "12", "month": "Dec", "yearA": "8932", "yearB": "10894" }];
                morris.setData(newModel);
            }
        };


        var onRepo = function (data) {
            $scope.xkey = 'month';
            $scope.ykeys = ['yearA', 'yearB'];
            $scope.labels = ['2013', '2014'];
            $scope.myModel = data;
            barchart();
        };

        var onRepo2 = function (data) {
            console.log('company data summary call');
            $scope.xkey = 'month';
            $scope.ykeys = ['yearA', 'yearB'];
            $scope.labels = ['2013', '2014'];
            $scope.myModel = data.summaryData[0].invoiceHistory;
            barchart();
        };

        var onError = function (reason) {
            $scope.reason = reason;
        };

        coDataSource.getCompanyInvoiceData()
            .then(onRepo, onError);

        coDataSource.getSummaryData()
            .then(onRepo2, onError);
    }

    app.controller('MorrisCtrl', dbInvoiceValues);

    var dbInvoiceValues2 = function ($scope, coDataSource) {
        $scope.pageElement = 'inv-hist-bar-chart2';
        var barchart = function () {
            console.log('creating morris chart 2');
            morris = Morris.Bar({
                element: $scope.pageElement,
                data: $scope.myModel,
                xkey: $scope.xkey,
                ykeys: $scope.ykeys,
                labels: $scope.labels,
                hideHover: 'auto'
               // ,ymin: 0
               ,ymax: $scope.maxValue
            });

            $scope.updateTest = function (customerId) {
                console.log('refresh morris chart 2 for customer Id:' + customerId);
                morris.setData($scope.invoiceData[customerId - 1].invoiceHistory);
                $scope.customerName = $scope.hierarchyData.customerData[customerId - 1].customerName;
                $scope.invoicesDue = $scope.invoiceData[customerId - 1].invoicesDue;
            }
        };

        var onRepo2 = function (data) {
            $scope.invoiceData = data.summaryData;
            $scope.hierarchyData = data.customerHierarchy;
            //$scope.invoicesDue = data.invoicesDue;
            $scope.invoicesDue = data.summaryData[0].invoicesDue;
            console.log('company data summary call 2');
            $scope.xkey = 'month';
            $scope.ykeys = ['yearA', 'yearB'];
            $scope.labels = [$scope.invoiceData[0].year, $scope.invoiceData[1].year];
            $scope.myModel = $scope.invoiceData[0].invoiceHistory;//data.summaryData[0].invoiceHistory;
            $scope.customerName = $scope.hierarchyData.customerData[0].customerName;
            $scope.maxValue = data.maxValue
            barchart();
        };

        var onError = function (reason) {
            $scope.reason = reason;
        };

        coDataSource.getSummaryData()
            .then(onRepo2, onError);

    };

    app.controller('MorrisCtrl2', dbInvoiceValues2);


}());