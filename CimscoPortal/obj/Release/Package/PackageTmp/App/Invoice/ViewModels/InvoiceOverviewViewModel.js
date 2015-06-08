(function () {

    var module = angular.module("invoice");

    var invoiceOverviewViewModel = function ($scope, inDataSource) {

        $scope.updateTest = function (customerId) {
            console.log('refresh invoice data for customer Id:' + customerId);
            $scope.invoiceData = inDataSource.getCompanyInvoices(customerId)
                                    .then(onInvData, onError);
        }


        var onRepo = function (data) {
            $scope.hierarchyData = data;
        };

        var onInvData = function (data) {
            $scope.invoiceData = data;
        };

        var onError = function (reason) {
            $scope.reason = reason;
        };

        inDataSource.getCompanyTree()
            .then(onRepo, onError);

        inDataSource.getCompanyInvoices()
            .then(onInvData, onError);

    };

    module.controller("invoiceOverviewViewModel", invoiceOverviewViewModel);
}());