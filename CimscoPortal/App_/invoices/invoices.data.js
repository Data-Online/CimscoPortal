(function () {

    var inDataSource = function ($http, $q) {

        //var getInvoiceTally = function (monthSpan, companyId) {
            
        //    //var dataApi = "/api/invoiceTally/" + monthSpan;
        //    var dataApi = "/api/detailbysite/" + monthSpan;
        //    if (companyId > 0)
        //        dataApi = dataApi + "/" + companyId;
        //    return $http.get(dataApi)
        //                .then(function (response) {
        //                    return response.data;
        //                });
        //};

        //var getAllInvoiceData_old = function (monthSpan, filter, pageNo) {
        //    //console.log("Getting data for Site ID = " + siteId + " for " + mounthsToDisplay + " months");
        //    pageNo = (pageNo == 0) ? 1 : pageNo;

        //    var canceler = $q.defer();

        //    var dataApi = "/api/invoiceAllOverview/" +  monthSpan + "/" + filter + "/" + pageNo;
        //    return $http.get(dataApi, {timeout: canceler.promise})
        //                .then(function (response) {
        //                    return response.data;
        //                });
        //};

        //var getSiteInvoiceData_old = function (siteId, monthsToDisplay) {
        //    //console.log("Getting data for Site ID = " + siteId + " for " + mounthsToDisplay + " months");
        //    var dataApi = "/api/invoiceOverviewFor/" + siteId + "/" + monthsToDisplay;
        //    return $http.get(dataApi)
        //                .then(function (response) {
        //                    return response.data;
        //                });
        //};

        var getAllInvoiceData = function (monthSpan, filter, pageNo) {
            //console.log("Getting data for Site ID = " + siteId + " for " + mounthsToDisplay + " months");
            //console.log(scope);
            pageNo = (pageNo == 0) ? 1 : pageNo;
            var dataApi = "/api/invoiceAllOverview/" + monthSpan  + "/" + filter + "/" + pageNo;

            var canceller = $q.defer();

            var cancel = function (reason) {
                canceller.resolve(reason);
            };
            
            var promise = $http.get(dataApi, { timeout: canceller.promise })
                        .then(function (response) {
                            return response.data;
                        });

            return {
                promise: promise,
                cancel: cancel
            };
        };

        var getSiteInvoiceData = function (siteId, monthSpan) {

            var dataApi = "/api/invoiceOverviewFor/" + siteId + "/" + monthSpan;

            var canceller = $q.defer();

            var cancel = function (reason) {
                canceller.resolve(reason);
            };

            var promise = $http.get(dataApi, { timeout: canceller.promise })
                        .then(function (response) {
                            return response.data;
                        });
            return {
                promise: promise,
                cancel: cancel
            };
        };

        var getInvoiceStatsBySite = function (monthSpan, filter) {
            var dataApi = "/api/invoiceStatsBySite/" + monthSpan + "/" + filter;

            var canceller = $q.defer();

            var cancel = function (reason) {
                canceller.resolve(reason);
            };

            var promise = $http.get(dataApi, { timeout: canceller.promise })
                        .then(function (response) {
                            return response.data;
                        });
            return {
                promise: promise,
                cancel: cancel
            };
        };

        var postInvoiceApproval = function (invoiceId) {
            //var data = { "InvoiceId": 20, "Approved": true };
            var dataApi = "/api/invoiceApproval/" + invoiceId;
            return $http.post(dataApi);
        };

        //var cancel = function(promise) {
        //    // If the promise does not contain a hook into the deferred timeout,
        //    // the simply ignore the cancel request.
        //   //// promise.canceler.resolve();
        //    if (
        //        promise &&
        //        promise._httpTimeout &&
        //        promise._httpTimeout.resolve
        //        ) {
        //        promise._httpTimeout.resolve();
        //    }
        //}

        return {
            //getInvoiceTally: getInvoiceTally,
            getAllInvoiceData: getAllInvoiceData,
            getSiteInvoiceData: getSiteInvoiceData,
            getInvoiceStatsBySite: getInvoiceStatsBySite,
            postInvoiceApproval: postInvoiceApproval
            //cancel: cancel,
          //  test: test
        };

    };
    var module = angular.module("app.detailBySite");
    module.factory("inDataSource", inDataSource);

}());