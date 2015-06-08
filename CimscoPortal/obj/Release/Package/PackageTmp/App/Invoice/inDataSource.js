(function () {

    var inDataSource = function ($http) {

        var getInvSummary = function (invoiceId) {
            var dataApi = "/api/invoicesummaryfor/" + invoiceId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };


        var getInvDetail = function (invoiceId) {
            var dataApi = "/api/invoicedetailfor/" + invoiceId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        // This is a duplication - need to refactor GPA
        var getCompanyTree = function () {
            var dataApi = "/api/companylistfor/1";
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getCompanyInvoices = function (customerId) {
            var dataApi = "/api/companyinvoicedatafor/" + customerId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        
        return {
            getInvSummary: getInvSummary,
            getInvDetail: getInvDetail,
            getCompanyTree: getCompanyTree,
            getCompanyInvoices: getCompanyInvoices
        };

    };
    var module = angular.module("invoice");
    module.factory("inDataSource", inDataSource);

}());