(function () {

    var coDataSource = function ($http) {

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
            var dataApi = "/api/summarydatafor/1";
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        return {
            getCompanyTree: getCompanyTree,
            getCompanyInvoiceData: getCompanyInvoiceData,
            getSummaryData: getSummaryData
        };

    };
    var module = angular.module("portalEnvironment");
    module.factory("coDataSource", coDataSource);

}());