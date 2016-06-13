﻿(function () {

    var dbDataSource = function ($http) {

        //var getTotalCostsByMonth = function (monthSpan, companyId) {
            
        //    var dataApi = "/api/TotalCostsByMonth/" + monthSpan;
        //    if (companyId > 0)
        //        dataApi = dataApi + "/" + companyId;
        //    return $http.get(dataApi)
        //                .then(function (response) {
        //                    return response.data;
        //                });
        //};

        var getTotalCostAndConsumption = function (monthSpan, filter) {

            var dataApi = "/api/TotalCostAndConsumption/" + monthSpan + "/" + filter;
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

        var getWelcomeScreen = function () {

            var dataApi = "/api/WelcomeScreen";
            return $http.get(dataApi)
                .then(function (response) {
                    return response.data;
                });
        };

        return {
            //getTotalCostsByMonth: getTotalCostsByMonth,
            getTotalCostAndConsumption: getTotalCostAndConsumption,
            getAllFilters: getAllFilters,
            getWelcomeScreen: getWelcomeScreen
        };

    };
    var module = angular.module("app.dashboard");
    module.factory("dbDataSource", dbDataSource);

}());