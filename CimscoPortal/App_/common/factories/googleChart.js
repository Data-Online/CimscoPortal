(function () {
    angular.module("googleChartControl", [])
    .factory('googleChart', googleChart)
    .directive('dualchart', dualchart);

    function googleChart($parse) {
        var singleAxes = function (single, axes) {
            var _displayAxes = [];
            var _columns = []; // Always display the first column (== x axis values)
            var _index = single;
            var _totalRows = 1;
            var _singleCount = 0;
            for (var i = _index; i < axes.length; i++) {
                if (_singleCount - _totalRows >= 0) {
                    if (axes[i].format == "tooltip") {
                        _columns.push(i);
                    }
                    break;
                }
                if (axes[i].format != "tooltip") {
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
                var _entry = returnIndex(axesNames[index], axesData);
                _displayAxes = pushColumns(_displayAxes, singleAxes(_entry, axesData).axes);
                _columns = pushColumns(_columns, singleAxes(_entry, axesData).columns);
            }
            var _data = { axes: _displayAxes, columns: _columns };
            return _data;
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
                var _fmt = _nextCol.role == "tooltip" ? "tooltip" : data.columns[i].format;
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

        displayAxesByName = function (axesNames, axesData) {
            var _columns = [0];
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

            var _collatedData = collateData(data);
            //console.log(_collatedData);
            var _chartElementName = chartElements.elementName;
            //console.log(_chartElementName);
            //var getter = $parse(_chartElementName + '.type');
            //getter.assign(scope, 'LineChart');
            var getter = $parse(_chartElementName + '.title');
            getter.assign(scope, chartElements.title);
            getter = $parse(_chartElementName + '.data');
            getter.assign(scope,
                {
                    "cols": _collatedData.cols,
                    "rows": _collatedData.rows
                }
                );

            // Restrict data displayed on this chart to specified Axes.
            var selectedColumnNames = [];
            selectedColumnNames.push(chartElements.columnNames[0]);

            var _axesToDisplay = selectAxesByName(selectedColumnNames, _collatedData.axes);
            chartElements.columns.primary = _axesToDisplay.columns;

            getter = $parse(_chartElementName + '.options');
            getter.assign(scope,
                configureChart(chartElements.title, _axesToDisplay.axes, _collatedData.axes[0].title)
                );
            chartElements.columns.all = displayAxesByName(chartElements.columnNames, _collatedData.axes);

            if (scope.showAsBar) {
                var _type = 'column';
            }
            else {
                var _type = 'line';
            }

            switchGoogleChartType(scope, _type, chartElements);

            refreshGoogleChart(scope, chartElements);
        };

        var switchAllGoogleChartTypes = function (scope, chartType, allChartElements) {
            angular.forEach(allChartElements, function (element, key) { switchGoogleChartType(scope, chartType, element); });
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

        var refreshGoogleChart = function (scope, chartElements) {
            var getter = $parse(chartElements.elementName + '.view');
            if (chartElements.activeAxes[1]) {
                var _columns = {
                    columns: chartElements.columns.all
                };
            }
            else {
                var _columns = {
                    columns: chartElements.columns.primary
                };
            }
            getter.assign(scope, _columns);
        };

        var toggleAxis2Display = function (displaySecondary, chartElements) {
            angular.forEach(chartElements, function (value, key) {
                value.activeAxes[1] = displaySecondary;
            });
        };

        return {
            initializeGoogleChart: initializeGoogleChart,
            refreshGoogleChart: refreshGoogleChart,
            toggleAxis2Display: toggleAxis2Display,
            switchAllGoogleChartTypes: switchAllGoogleChartTypes
        };

    }

    dualchart.$inject = ['cdcConstants'];
    function dualchart (cdcConstants) {
        return {
            restrict: "E",
            templateUrl: cdcConstants.template_url + "googleDualChart.html",
            scope: {
                chart: "=",
            }
        };
    }

}());