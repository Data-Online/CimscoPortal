(function () {

    var module = angular.module("invoice");

    var invoiceOverviewViewModel = function ($scope, inDataSource) {

        $scope.updateTest = function (indexId) {
            // Currently siteId, will move to indexId
            console.log('Refresh invoice data for index Id:' + indexId);
            $scope.invoiceData = inDataSource.getSiteInvoices(indexId)
                                    .then(onInvData, onError);
        }

        $scope.order = function (predicate) {
            $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : true;
            $scope.predicate = predicate;
        };

        $scope.pctBoxStyle = function (myValue) {
            var num = parseInt(myValue);
            var style = 'databox-stat radius-bordered';
            if (num > 0) {
                style = style + ' bg-warning';
            }
            else if (num < 0) {
                style = style + ' bg-green';
            }
            else {
                style = style + ' bg-sky';
            }
            return style;
        };

        $scope.negativeValue = function (myValue) {
            var num = parseInt(myValue);
            var style = 'stat-icon';
            if (num > 0) {
                style = style + ' fa fa-long-arrow-up';
            }
            else if (num < 0) {
                style = style + ' fa fa-long-arrow-down';
            }
            //console.log('Style for value ' + num +' set to ' + style);
            return style;
        };

        var onRepo = function (data) {
            $scope.hierarchyData = data;
        };

        var onSiteRepo = function (data) {
            $scope.siteHierarchyData = data;
        };

        var onInvData = function (data) {
            $scope.invoiceData = data;
        };

        var onError = function (reason) {
            $scope.reason = reason;
        };

        inDataSource.getCompanyTree()
            .then(onRepo, onError);

        inDataSource.getSiteTree()
            .then(onSiteRepo, onError);

        inDataSource.getSiteInvoices(2)
            .then(onInvData, onError);

    };

    module.controller("invoiceOverviewViewModel", invoiceOverviewViewModel);
}());