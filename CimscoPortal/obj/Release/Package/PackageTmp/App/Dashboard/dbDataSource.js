(function () {

    var dbDataSource = function ($http) {

        var getCompanyTree = function () {
            var dataApi = "/api/companylistfor/1";
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getCompanyInvoiceData = function () {
            var dataApi = "/api/companyinvoicedatafor/1";
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getSummaryData = function () {
            var dataApi = "/api/summarydata";
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var postInvoiceApproval = function (invoiceId)
        {
            //var data = { "InvoiceId": 20, "Approved": true };
            var dataApi = "/api/invoiceapproval/" + invoiceId;
            return $http.post(dataApi);
        }

        return {
            getCompanyTree: getCompanyTree,
            getCompanyInvoiceData: getCompanyInvoiceData,
            getSummaryData: getSummaryData,
            postInvoiceApproval: postInvoiceApproval
        };

    };
    var module = angular.module("dashboard");
    module.factory("dbDataSource", dbDataSource);

}());