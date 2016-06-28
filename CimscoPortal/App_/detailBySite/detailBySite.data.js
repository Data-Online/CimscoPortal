(function () {

    var soDataSource = function ($http) {

        var getInvoiceTally = function (monthSpan, companyId) {
            
            //var dataApi = "/api/invoiceTally/" + monthSpan;
            var dataApi = "/api/detailbysite/" + monthSpan;
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
    var module = angular.module("app.detailBySite");
    module.factory("soDataSource", soDataSource);

}());