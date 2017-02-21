(function () {

    var soDataSource = function ($http) {

        var getSiteInvoiceData = function (siteId, mounthsToDisplay) {
            //console.log("Getting data for Site ID = " + siteId + " for " + mounthsToDisplay + " months");
            var dataApi = "/api/invoiceOverviewFor/"+siteId+"/" + mounthsToDisplay;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getInvoiceData = function (siteId, invoiceId) {
            //console.log("Getting data for Site ID = " + siteId + " invoiceId = " + invoiceId);
            var dataApi = "/api/invoiceOverviewFor/" + siteId + "/" + invoiceId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var postInvoiceApproval = function (invoiceId) {
            //var data = { "InvoiceId": 20, "Approved": true };
            var dataApi = "/api/invoiceApproval/" + invoiceId;
            return $http.post(dataApi);
        };

        var getTestModel = function () {
            var dataApi = "/api/testmodel";
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getSiteDetails = function (siteId) {
            var dataApi = "/api/siteDetails/" + siteId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        //var getCostConsumptionData = function (siteId, monthSpan) {
        //    var dataApi = "/api/CostAndConsumption/" + siteId + "/" + monthSpan + "/__";
        //    return $http.get(dataApi)
        //                .then(function (response) {
        //                    return response.data;
        //                });
        //};

        
        return {
            getSiteInvoiceData: getSiteInvoiceData,
            getInvoiceData: getInvoiceData,
            postInvoiceApproval: postInvoiceApproval,
            getTestModel: getTestModel,
            //getCostConsumptionData: getCostConsumptionData,
            getSiteDetails: getSiteDetails

        };



    };
    angular.module("app.siteOverview")
        .factory("soDataSource", soDataSource);

}());