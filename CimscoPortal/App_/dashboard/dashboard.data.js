(function () {

    var dbDataSource = function ($http) {

        var getTotalCostsByMonth = function (monthSpan, companyId) {
            
            var dataApi = "/api/TotalCostsByMonth/" + monthSpan;
            if (companyId > 0)
                dataApi = dataApi + "/" + companyId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getTotalConsumptionByMonth = function (monthSpan) {

            var dataApi = "/api/TotalConsumptionByMonth/" + monthSpan;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getAllFilters = function () {

            var dataApi = "/api/Filters";
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        return {
            getTotalCostsByMonth: getTotalCostsByMonth,
            getTotalConsumptionByMonth: getTotalConsumptionByMonth,
            getAllFilters: getAllFilters
        };

    };
    var module = angular.module("app.dashboard");
    module.factory("dbDataSource", dbDataSource);

}());