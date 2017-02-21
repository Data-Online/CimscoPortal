(function () {

    var inDataSource = function ($http) {

        var getInvSummary = function (invoiceId) {
            var dataApi = "/api/invoiceSummaryFor/" + invoiceId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };


        //var getInvDetail = function (invoiceId) {
        //    var dataApi = "/api/invoicedetailfor/" + invoiceId;
        //    return $http.get(dataApi)
        //                .then(function (response) {
        //                    return response.data;
        //                });
        //};

        var getInvDetail_ = function (invoiceId) {
            var dataApi = "/api/invoiceDetailFor_/" + invoiceId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };


        // This is a duplication - need to refactor GPA
        var getCompanyTree = function () {
            var dataApi = "/api/companyhierarchy"; // was companylistfor
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getSiteTree = function () {
            var dataApi = "/api/sitehierarchy"; 
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getSiteInvoices = function (siteId) {
            var dataApi = "/api/siteInvoiceDataFor/" + siteId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var postInvoiceApproval = function (invoiceId) {
            //var data = { "InvoiceId": 20, "Approved": true };
            var dataApi = "/api/invoiceapproval/" + invoiceId;
            return $http.post(dataApi);
        }
        
        return {
            getInvSummary: getInvSummary,
          //  getInvDetail: getInvDetail,
            getInvDetail_: getInvDetail_,
            getCompanyTree: getCompanyTree,
            getSiteTree: getSiteTree,
            getSiteInvoices: getSiteInvoices,
            postInvoiceApproval: postInvoiceApproval
        };

    };
    var module = angular.module("invoice");
    module.factory("inDataSource", inDataSource);

}());