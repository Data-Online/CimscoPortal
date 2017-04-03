/// <reference path="P:\Projects\Cloud_Development\GitHub\CimscoPortal\CimscoPortal\Views/Portal/Templates/FilterDropdowns.html" />
(function () {
    angular.module("commonDataControl", [])
        .factory('filterData', filterData)
        .constant("cdcConstants", {
            "filterSelectDelay": "15",
            "triggerName": "refreshData",
            "template_url": '/App_/common/templates/',
            "inactiveFilter": "_"
        })
        .directive('filterdropdowns', filterdropdowns)
        .service('sharedData', sharedData);


    function sharedData() {
        var sharedValues = { topLevelName: "", levelType: "" };
        return {
            getSharedValues: function () { return sharedValues; },
            setSharedValues: function (name, type) {
                sharedValues.topLevelName = name;
                sharedValues.levelType = type;
            }
        };
    }

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
            var dataApi = "/api/filters";
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getEventName = function () {
            return cdcConstants.triggerName;
        };

        var createApiFilter = function (categories, divisions) {
            var _returnIds = cdcConstants.inactiveFilter;
            angular.forEach(categories, function (value, key) {
                _returnIds += value.id + "-";
            });
            _returnIds += cdcConstants.inactiveFilter;
            angular.forEach(divisions, function (value, key) {
                _returnIds += value.id + "-";
            });
            //console.log(_returnIds);
            return _returnIds;
        };

        var inactiveFilter = function () {
            return cdcConstants.inactiveFilter + cdcConstants.inactiveFilter;
        };

        var filterTypeActive = function (type, filter) {
            var _filterParts = filter.split(cdcConstants.inactiveFilter);
            //console.log(_filterParts);
            switch (type) {
                case 'division':
                    return _filterParts[2] != "";
                    break;
                case 'category':
                    return _filterParts[1] != "";
                    break;
                case 'any':
                    return filter != cdcConstants.inactiveFilter + cdcConstants.inactiveFilter;
                    break;
                default:
                    return false;
            }
        };

        //elementIndex_ = function (elements, elementName) {
        //    //var zz = elementIndex_(elements, elementName, 'elementName');
        //    //console.log(zz);
        //    // Return index for this title
        //    for (var i = 0; i < elements.length; i++) {
        //        if (elements[i].elementName == elementName) { return i }
        //    };

        //    return -1;
        //};

        elementIndex = function (elements, elementName, keyElement) {
            // Return index for this title
            //var keyElement = 'elementName';
            for (var i = 0; i < elements.length; i++) {
                var _element = elements[i];
                getter = $parse(keyElement);
                if (getter(_element) == elementName) { return i };
            };
            //angular.forEach(elements, function (element, key) {
            //    getter = $parse(keyElement);
            //    var _currentElement = getter(element);
            //    console.log(_currentElement + " looking for : " + elementName + " key = " + key + " match = " + (_currentElement == elementName));

            //    if (_currentElement == elementName) { console.log("returning " + key); return key };
            //});
            return -1;
            //            for (var i = 0; i < elements.length; i++) {
            //                var _element = elements[i];
            //                //console.log(_element.elementName);
            //                getter = $parse('elementName');
            //                var ztest = getter(_element);
            //                console.log(ztest);
            ////                if (elements[i].elementName == elementName) { return i }
            //            };
            //          return -1;
        };


        return {
            createMultiDropdown: createMultiDropdown,
            getAllFilters: getAllFilters,
            getEventName: getEventName,
            createApiFilter: createApiFilter,
            elementIndex: elementIndex,
            inactiveFilter: inactiveFilter,
            filterTypeActive: filterTypeActive
        };
    }

})();

(function () {
    angular.module("commonDataControl")
        .factory('dataFormatting', dataFormatting);

    function dataFormatting() {
        return {
            pctBoxStyle: function (value) {
                var num = parseInt(value);
                var style = 'databox-stat radius-bordered';
                if (num <= -999) {
                    style = style + ' hide-element';
                }
                else if (num > 0) {
                    style = style + ' bg-warning';
                }
                else if (num < 0) {
                    style = style + ' bg-green';
                }
                else {
                    style = style + ' bg-sky';
                }
                return style;
            },
            negativeValue: function (value) {
                var num = parseInt(value);
                var style = 'stat-icon';
                if (num == -999) {

                }
                else if (num == 0) {
                    style = style + ' fa fa-arrows-h';
                }
                else if (num > 0) {
                    style = style + ' fa fa-long-arrow-up';
                }
                else if (num < 0) {
                    style = style + ' fa fa-long-arrow-down';
                }
                // console.log('Style for value ' + num +' set to ' + style);
                return style;
            }
        };
    }
})();

(function () {
    angular.module("commonDataControl")
        .factory('consumptionData', consumptionData);

    consumptionData.$inject = ['$http']
    function consumptionData($http) {

        var getCostConsumptionData = function (monthSpan, filter, siteId) {
            var dataApi = "/api/costAndConsumption" + "/" + monthSpan + "/" + filter + "/" + siteId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        var getDatapointDetails = function (datapointIdentity, siteId) {
            //console.log(datapointIdentity);
            //console.log(JSON.stringify(datapointIdentity));
            var dataApi = "/api/DatapointDetails/" + siteId;
            var config = {
                headers: {
                    'Content-Type': 'application/json'
                }
            };
            //var datapointIdentity = [];

            return $http.post(dataApi, JSON.stringify(datapointIdentity), config)
                        .then(function (response) {
                            return response.data;
                        });


            //console.log(datapointIdentity);
            //return { status: "Attention", notes: "Missing invoices", date: new Date('01/10/2015') };
        };

        var getComparisonData = function (monthSpan, filter, siteId) {
            var dataApi = "/api/comparisonData" + "/" + monthSpan + "/" + filter + "/" + siteId;
            return $http.get(dataApi)
                        .then(function (response) {
                            return response.data;
                        });
        };

        return {
            getCostConsumptionData: getCostConsumptionData,
            getDatapointDetails: getDatapointDetails,
            getComparisonData: getComparisonData
        };
    };
})();

(function () {
    angular.module("commonDataControl")
    .factory('datapointDetails', datapointDetails);
    
    datapointDetails.$inject = ['$parse', 'googleChart', 'consumptionData'];
    function datapointDetails($parse, googleChart, consumptionData) {

        var removeDatapoint = function (item, thisScope, dataElementName) {
            var getter = $parse(dataElementName);
            if (item == 0) {
                getter.assign(thisScope, []);
            }
            else {
                var index = getter(thisScope).indexOf(item);
                getter(thisScope).splice(index, 1);
            }
        };
        var onDatapointDetails = function (data, colour, thisScope, dataElementName) {
            data.colour = colour;
            var getter = $parse(dataElementName);
            getter(thisScope).push(data);
        };
        function onError(reason) {
            toaster.pop('error', "Data Load Error", "Unable to load datapoint details. Status ID =" + reason.status);
        };
        var selectedDatapoint = function (selectedItem, element, thisScope, configData, siteId) {
            if (selectedItem) {
                //var loadIconGetter = $parse(loadingIconActive);
                var loadIconGetter = $parse(configData.loadIconElementName);
                loadIconGetter.assign(thisScope, true);
                var _datapointIdentity = googleChart.lineNameFromChartNumber(thisScope, element, selectedItem.column, selectedItem.row);
                //console.log(_datapointIdentity);
                consumptionData.getDatapointDetails(_datapointIdentity, siteId)
                    .then(function success(data) { loadIconGetter.assign(thisScope, false); return onDatapointDetails(data, _datapointIdentity.colour, thisScope, configData.dataElementName) },
                            function error() { loadIconGetter.assign(thisScope, false); return onError });
            };
        };

        return {
            selectedDatapoint: selectedDatapoint,
            removeDatapoint: removeDatapoint
        };
    };
})();