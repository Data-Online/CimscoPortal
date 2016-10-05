(function () {
    angular.module("googleChartControl", [])
    .factory('googleChart', googleChart)
    .directive('dualChartHeader', dualChartHeader);

    function googleChart($parse) {
        var reverseAxisOrder = true; // Use when 2nd axis is to be displayed as first bar (eg it is an ealier date, so makes sense to display in reverse order)

        var singleAxes = function (startIndex, axes) {
            // Starts at "index" entry in columns, and extracts elements for single axis graph. Can include Tooltip and / or Style
            var _displayAxes = [];
            var _columns = []; // Always display the first column (== x axis values)
            var _totalRows = 1;
            var _singleCount = 0;
            for (var i = startIndex; i < axes.length; i++) {
                // Only allow "tooltip" or "style" if "data" role has already been added
                if (_singleCount - _totalRows >= 0) {
                    if (axes[i].format != "tooltip" & axes[i].format != "style") {
                        break;
                    }
                }
                if (axes[i].format != "tooltip" & axes[i].format != "style") {
                    _singleCount++;
                    _displayAxes.push(axes[i]);
                }
                _columns.push(i);
            }
            var _data = { axes: _displayAxes, columns: _columns };
            return _data;
        };

        var pushColumns = function (array, newColumns) {
            for (var i = 0; i < newColumns.length; i++) {
                array.push(newColumns[i]);
            }
            return array;
        };

        var returnIndex = function (forName, data) {
            for (var j = 0; j < data.length; j++) {
                if (data[j].title == forName) {
                    return j;
                }
            }
            return -1;
        };

        selectAxesByName = function (axesNames, axesData) {
            var _displayAxes = [];
            var _columns = [0];
            for (index = 0; index < axesNames.length; index++) {
                var _startAtIndex = returnIndex(axesNames[index], axesData);
                var _singleAxes = singleAxes(_startAtIndex, axesData);
                _displayAxes = pushColumns(_displayAxes, _singleAxes.axes);
                _columns = pushColumns(_columns, _singleAxes.columns);
            }
            var _data = { axes: _displayAxes, columns: _columns };
            return _data;
        };

        addAxesByName = function (axesNames, axesData, elementName) {

            angular.forEach(axesNames, function (axesName, index) {
                var getter = $parse(elementName + '.title');
                getter.assign(scope, chartElements.title);
            })
        };

        collateData = function (data) {
            var _cols = [];
            var _rows = [];
            var _cpart = [];
            var _axes = [];

            for (var i = 0; i < data.columns.length; i++) {
                var _nextCol = data.columns[i];
                if (_nextCol.role == "tooltip" & _nextCol.format == "html") {
                    _nextCol.p = { 'html': true };
                }
                _cols.push(_nextCol);
                var _fmt = _nextCol.role == "tooltip" ? "tooltip" : (_nextCol.role == "style" ? "style" : data.columns[i].format);
                var _axis = { "title": data.columns[i].label, "format": _fmt }
                _axes.push(_axis);
            };
            for (var i = 0; i < data.rows.length; i++) {
                for (var j = 0; j < data.rows[i].cparts.length; j++) {
                    var c = data.rows[i].cparts[j];
                    _cpart.push(c);
                }
                _rows.push({ c: _cpart });
                _cpart = [];
            };

            var _data = { cols: _cols, rows: _rows, cpart: _cpart, axes: _axes };
            return _data;
        };
        //
        configureChart = function (title, vAxes, hAxisTitle) {
            angular.forEach(vAxes, function (obj, index) { console.log(obj); });
            var _right = 0;
            if (vAxes.length == 2) {
                var _series = [{ targetAxisIndex: 0 }, { targetAxisIndex: 1 }];
                _right = 100;
            }
            else { var _series = [{ targetAxisIndex: 0 }] }
            var options = {};
            options = {
                "interpolateNulls": true,
                "chartArea": { left: 100, top: 50, bottom: 100, right: _right, width: "100%", height: "100%" },//{ "width": "80%", "height": "60%" },
                "title": "", //title,
                "colors": ['#0000FF', '#009900', '#CC0000', '#DD9900'],
                "defaultColors": ['#0000FF', '#009900', '#CC0000', '#DD9900'],
                //     "isStacked": "true",
                "fill": 20,
                "displayExactValues": true,
                "pointSize": 5,
                "lineWidth": 3,
                "vAxes": vAxes,
                "hAxis": {
                    "title": hAxisTitle, "direction": 1, "slantedText": true, "slantedTextAngle": 45
                },
                "animation": {
                    "duration": 1000,
                    "easing": 'out',
                },
                "series": _series,
                "tooltip": { isHtml: true },
                "legend": { position: "top", maxlines: 1 }
            };
            return options;
        };

        displayAxesByName = function (axesNames, axesData, reverseOrder) {
            var _columns = [0];
            if (reverseOrder) {
                for (index = axesNames.length - 1; index >= 0; index--) {
                    var _entry = returnIndex(axesNames[index], axesData);
                    _columns = pushColumns(_columns, singleAxes(_entry, axesData).columns);
                }
            }
            else {
                for (index = 0; index < axesNames.length; index++) {
                    var _entry = returnIndex(axesNames[index], axesData);
                    _columns = pushColumns(_columns, singleAxes(_entry, axesData).columns);
                }
            }
            return _columns;
        };

        var displayAxesByName_ = function (axesNames, axesData) {
            var _columns = [];

            for (index = 0; index < axesNames.length; index++) {
                var _entry = returnIndex(axesNames[index], axesData);
                _columns = pushColumns(_columns, singleAxes(_entry, axesData).columns);
            }
            return _columns;
        };

        var initializeGoogleChart = function (scope, data, chartElements) {
            // data.axes defined the Axes labels and formats.
            // Axes 0 == X
            // Remaining Axes are Y
            var _firstDisplayAxis = 0; // Element in chartElements that should be treated as primary axis data (displayed initially)


            // Firstly assign all available data
            var _collatedData = collateData(data);
            //console.log(_collatedData);
            var _chartElementName = chartElements.elementName;
            var getter = $parse(_chartElementName + '.title');
            getter.assign(scope, chartElements.title);
            getter = $parse(_chartElementName + '.data');
            getter.assign(scope,
                {
                    "cols": _collatedData.cols,
                    "rows": _collatedData.rows
                }
            );

            // Establish which data and elements to display
            // Primary
            var selectedColumnNames = [];
            selectedColumnNames.push(chartElements.columnNames[_firstDisplayAxis]);
            var _axesToDisplay = selectAxesByName(selectedColumnNames, _collatedData.axes);
            console.log("Axes " + _axesToDisplay.columns);
            getter = $parse(_chartElementName + '.options');
            getter.assign(scope,
                configureChart(chartElements.title, _axesToDisplay.axes, _collatedData.axes[0].title)
                );

            angular.forEach(chartElements.columnNames, function (column, index) {
                var _col = [];
                _col.push(column);
                chartElements.columns[index] = displayAxesByName_(_col, _collatedData.axes);
            });

            if (scope.showAsBar) {
                var _type = 'column';
            }
            else {
                var _type = 'line';
            }

            switchGoogleChartType(scope, _type, chartElements);

            refreshGoogleChart(scope, chartElements, reverseAxisOrder);
        };

        var switchAllGoogleChartTypes = function (scope, chartType, allChartElements) {
            angular.forEach(allChartElements, function (element, key) {
                console.log("switching : " + element);
                switchGoogleChartType(scope, chartType, element);
            });
        };

        var switchGoogleChartType = function (scope, chartType, chartElements) {
            var _chartElementName = chartElements.elementName;

            switch (chartType) {
                case 'line':
                    var _type = 'LineChart';
                    var _stacked = true;
                    break;
                case 'column':
                    var _type = 'ColumnChart';
                    var _stacked = false;
                    break;
            }
            var getter = $parse(_chartElementName + '.type');
            getter.assign(scope, _type);
            var getter = $parse(_chartElementName + '.options.isStacked');
            getter.assign(scope, _stacked);
        };

        var refreshAllGoogleCharts = function (scope, allChartElements) {
            angular.forEach(allChartElements, function (element, key) {
                refreshGoogleChart(scope, element, reverseAxisOrder);
            });
        };

        var refreshGoogleChart = function (scope, chartElement, reverseOrder) {
            // First column is x Axis, so always display
            var _columns = [];
            var _colors = [];
            angular.forEach(chartElement.activeAxes, function (active, index) {
                if (active) {
                    if (chartElement.columns[index]) {
                        if (reverseOrder) {
                            _columns = chartElement.columns[index].concat(_columns);
                            _colors.unshift(chartElement.colours[index]);
                        }
                        else {
                            _columns = _columns.concat(chartElement.columns[index]);
                            _colors.push(chartElement.colours[index]);
                        }
                    }
                }
            });
            _columns.unshift(0);
            var getter = $parse(chartElement.elementName + '.view');
            getter.assign(scope, { columns: _columns });
            var getter = $parse(chartElement.elementName + '.options.colors');
            getter.assign(scope, _colors);
        };


        var toggleAxis2Display = function (displaySecondary, chartElements) {
            angular.forEach(chartElements, function (value, key) {
                value.activeAxes[1] = displaySecondary;
            });
        };
        var ZtoggleAxis2Display = function (display, chartElements) {
            angular.forEach(chartElements, function (value, key) {
                console.log(value + " " + (value.activeAxes[2]));
                value.activeAxes[2] = display;
            });
        };

        var createButtonControls = function (scope, chartElements) {
            scope.togglePreviousYearsData = function ($event) {
                scope.loading = true;
                // Decide which axes are to be displayed.
                toggleAxis2Display(scope.showPrevious12, chartElements);
                // Refresh
                refreshAllGoogleCharts(scope, chartElements);
                scope.loading = false;
            };

            scope.toggleEnergySavingData = function ($event) {
                scope.loading = true;
                // Decide which axes are to be displayed.
                ZtoggleAxis2Display(scope.showSavings, chartElements)
                // Refresh
                refreshAllGoogleCharts(scope, chartElements);
                scope.loading = false;
            };
        };

        return {
            initializeGoogleChart: initializeGoogleChart,
            //toggleAxis2Display: toggleAxis2Display,
            switchAllGoogleChartTypes: switchAllGoogleChartTypes,
            refreshAllGoogleCharts: refreshAllGoogleCharts,
            //ZtoggleAxis2Display: ZtoggleAxis2Display,
            createButtonControls: createButtonControls
        };

    }

    //dualchart.$inject = ['cdcConstants'];
    //function dualchart(cdcConstants) {
    //    return {
    //        restrict: 'E',
    //        templateUrl: cdcConstants.template_url + "googleDualChart.html",
    //        scope: {
    //            'chart': '=',
    //            'border': '@'
    //        }
    //    };
    //};

    dualChartHeader.$inject = ['cdcConstants'];
    function dualChartHeader(cdcConstants) {
        return {
            restrict: 'AE',
            templateUrl: cdcConstants.template_url + "googleDualChartHeader.html",
            scope: {
                'chart': '=',
                'border': '@'
            }
        };
    };

}());