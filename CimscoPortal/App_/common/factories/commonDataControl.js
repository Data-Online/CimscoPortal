/// <reference path="P:\Projects\Cloud_Development\GitHub\CimscoPortal\CimscoPortal\Views/Portal/Templates/FilterDropdowns.html" />
(function () {
    angular.module("commonDataControl", [])
        .factory('filterData', filterData)
        .constant("cdcConstants", {
            "filterSelectDelay": "15",
            "triggerName": "refreshData",
            "template_url": '/App_/common/factories/templates/'
        })
        .directive('filterdropdowns', filterdropdowns);




    filterdropdowns.$inject = ['cdcConstants'];
    function filterdropdowns(cdcConstants) {
        return {
            restrict: 'A',
            templateUrl: cdcConstants.template_url + 'FilterDropdowns.html'
        }
    }

    filterData.$inject = ['$http', '$parse', '$interval', 'cdcConstants'];
    function filterData($http, $parse, $interval, cdcConstants) {
        String.prototype.capitalizeFirstLetter = function () {
            return this.charAt(0).toUpperCase() + this.slice(1);
        };

        var createMultiDropdown = function (baseName, selectionItemsList, createWatch, thisScope) {
            // Create variables on scope
            var getter = $parse(baseName + 'Model');
            getter.assign(thisScope, []);

            getter = $parse(baseName + 'Data');
            getter.assign(thisScope, selectionItemsList);
            getter = $parse(baseName + 'CustomTexts');

            var buttonText = 'All ' + baseName.capitalizeFirstLetter();
            var customTexts = { buttonDefaultText: buttonText, uncheckAll: 'Clear Filters' };
            getter.assign(thisScope, customTexts);
            if (createWatch) {
                thisScope.$watch(baseName + 'Model', function () {
                    //filterData(data, baseName);
                    filterData(thisScope);
                }, true);
            };

            var maxTextLength = buttonText.length;
            getter = $parse(baseName + 'Settings');
            getter.assign(thisScope, {
                smartButtonMaxItems: 1,
                externalIdProp: '',
                showCheckAll: false,
                smartButtonTextConverter: function (itemText, originalItem) {
                    if (itemText.length > maxTextLength) {
                        return itemText.substring(0, (maxTextLength - 2)) + '..';
                    }
                }
            });
        };

        var filterData = function (thisScope) {
            startDelay(thisScope);
        };

        var stop;
        var _counter = cdcConstants.filterSelectDelay;
        var startDelay = function (thisScope) {
            if (angular.isDefined(stop)) { _counter = cdcConstants.filterSelectDelay; return; }
            stop = $interval(function () {
                if (_counter > 0) {
                    _counter--;
                }
                else { stopCounter(thisScope); }
            }, 100)
        };

        var stopCounter = function (thisScope) {
            if (angular.isDefined(stop)) {
                $interval.cancel(stop);
                stop = undefined;
                _counter = cdcConstants.filterSelectDelay;
            };
            thisScope.$emit(cdcConstants.triggerName);
        };


        var getAllFilters = function () {
            var dataApi = "/api/Filters";
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getEventName = function () {
            return cdcConstants.triggerName;
        };

        var createApiFilter = function (categories, divisions) {
            var _returnIds = "_";
            angular.forEach(categories, function (value, key) {
                _returnIds += value.id + "-";
            });
            _returnIds += "_";
            angular.forEach(divisions, function (value, key) {
                _returnIds += value.id + "-";
            });
            console.log(_returnIds);
            return _returnIds;
        };
        elementIndex = function (elements, title) {
            // E
            // Return index for this title
            for (var i = 0; i < elements.length; i++) {
                if (elements[i].title == title) { return i }
            };
            return -1;
        };

        var getCostConsumptionData = function (monthSpan, filter, siteId) {
            var dataApi = "/api/CostAndConsumption_" + "/" + monthSpan + "/" + filter + "/" + siteId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        return {
            createMultiDropdown: createMultiDropdown,
            getAllFilters: getAllFilters,
            getEventName: getEventName,
            createApiFilter: createApiFilter,
            elementIndex: elementIndex,
            getCostConsumptionData: getCostConsumptionData
        };
    }

}());