(function () {

    var soDataSource = function ($http) {

        var getSummaryData = function () {
            var dataApi = "/api/summarydata";
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        return {
            getSummaryData: getSummaryData
        };

    };
    var module = angular.module("siteOverview");
    module.factory("soDataSource", soDataSource);

}());