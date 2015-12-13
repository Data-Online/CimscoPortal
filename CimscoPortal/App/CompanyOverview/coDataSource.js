(function () {

    var coDataSource = function ($http) {

        var getInvoiceTally = function (monthSpan) {
            var dataApi = "/api/invoicetally/"+monthSpan;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        return {
            getInvoiceTally: getInvoiceTally
        };

    };
    var module = angular.module("companyOverview");
    module.factory("coDataSource", coDataSource);

}());