(function () {

    var soDataSource = function ($http) {

        var getSiteInvoiceData = function (siteId) {
            console.log("Getting data for Site ID = " + siteId);
            var dataApi = "/api/invoiceOverviewFor/"+siteId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getInvoiceData = function (siteId, invoiceId) {
            console.log("Getting data for Site ID = " + siteId + " invoiceId = " + invoiceId);
            var dataApi = "/api/invoiceOverviewFor/" + siteId + "/" + invoiceId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var postInvoiceApproval = function (invoiceId) {
            //var data = { "InvoiceId": 20, "Approved": true };
            var dataApi = "/api/invoiceApproval/" + invoiceId;
            return $http.post(dataApi);
        }

        return {
            getSiteInvoiceData: getSiteInvoiceData,
            getInvoiceData: getInvoiceData,
            postInvoiceApproval: postInvoiceApproval
        };



    };
    var module = angular.module("app");
    module.factory("soDataSource", soDataSource);

}());