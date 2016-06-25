(function () {

    var soDataSource = function ($http) {

        var getInvoiceTally = function (monthSpan, companyId) {
            
            var dataApi = "/api/invoicetally/" + monthSpan;
            if (companyId > 0)
                dataApi = dataApi + "/" + companyId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };




        return {
            getInvoiceTally: getInvoiceTally
        };

    };
    var module = angular.module("app");
    module.factory("soDataSource", soDataSource);

}());