(function () {

    var icDataSource = function ($http) {

        var postInvoiceApproval = function (invoiceId) {
            var dataApi = "/api/invoiceApproval/" + invoiceId;
            return $http.post(dataApi);
        }

        return {
            postInvoiceApproval: postInvoiceApproval
        };
    };
    var module = angular.module("app");
    module.factory("icDataSource", icDataSource);

}());