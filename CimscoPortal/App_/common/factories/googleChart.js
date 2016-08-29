angular.module("googleChartControl", [])
    .factory('googleChart', function () {
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
        //
        //
        return {
            collateData: function (data) {
                var _cols = [];
                var _rows = [];
                var _cpart = [];
                var _axes = [];

                for (var i = 0; i < data.columns.length; i++) {
                    var _nextCol = data.columns[i];
                    if (_nextCol.role == "tooltip" & _nextCol.format == "html") {
                        _nextCol.p = { 'html': true };
                    }
                    //{ id: data.columns[i].$id, title: data.columns[i].label+"T", label: data.columns[i].label+"L", type: data.columns[i].type };
                    _cols.push(_nextCol);
                    var _fmt = _nextCol.role == "tooltip" ? "tooltip" : data.columns[i].format;
                    var _axis = { "title": data.columns[i].label, "format": _fmt }
                    _axes.push(_axis);
                };
                //// Test annotation
                //var _nextCol = { type: 'string', role: 'tooltip' };
                //_cols.push(_nextCol);
                for (var i = 0; i < data.rows.length; i++) {
                    for (var j = 0; j < data.rows[i].cparts.length; j++) {
                        //var c = { v: data.rows[i].cparts[j].v, f: data.rows[i].cparts[j].f }
                        var c = data.rows[i].cparts[j];
                        _cpart.push(c);
                    }
                    //// Test annotation
                    //var c = { v: "test" };
                    //_cpart.push(c);

                    _rows.push({ c: _cpart });
                    _cpart = [];
                };

                var _data = { cols: _cols, rows: _rows, cpart: _cpart, axes: _axes };
                return _data;
            },
            configureChart: function (title, vAxes, hAxisTitle) {
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
                    "isStacked": "true",
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
                    "series": _series, //[{ targetAxisIndex: 0 }, { targetAxisIndex: 1 }],
                    "tooltip": { isHtml: true },
                    "legend": { position: "top", maxlines: 1 }
                };
                return options;
            },
            axesPair: function (pair, axes) {
                var skipPair = function (pair, axes) {
                    var _loopCount = 0;
                    for (var i = 1; i < axes.length; i++) {
                        if (axes[i].format != "tooltip") { _loopCount++ }
                        //console.log('I:' + i + " loop:" + _loopCount);
                        if (_loopCount == (2 * pair - 1)) { return i; }
                    };
                };
                if (pair == 0) {
                    var _allCols = true;
                    pair = 1;
                };
                // console.log(_allCols);
                var _displayAxes = [];
                var _columns = [0]; // Always display the first column (== x axis values)
                //var _index = (2 * pair) - 1;
                //var _index = (2 * pair);
                var _index = skipPair(pair, axes);
                //console.log("Index returned = " + _index);
                //var _Zindex = skipPair(2, axes);
                //console.log(_Zindex);

                var _totalRows = 2;
                var _pairCount = 0;
                for (var i = _index; i < axes.length; i++) {
                    if (_pairCount - _totalRows >= 0) {
                        //console.log("A" + i);
                        if (axes[i].format == "tooltip" & !_allCols) {
                            //console.log("B");
                            _columns.push(i);
                        }
                        break;
                    }
                    if (axes[i].format != "tooltip") {
                        _pairCount++;
                        _displayAxes.push(axes[i]);
                    }
                    //console.log('I count = ' + i);
                    if (!_allCols) { _columns.push(i) };
                }
                if (_allCols) {
                    for (var i = 1; i < axes.length; i++) {
                        _columns.push(i);
                    };
                }
                var _data = { axes: _displayAxes, columns: _columns };
                return _data;
            },
            allAxes: function (axes) {
                var _columns = [0]; // Always display the first column (== x axis values == Month Name)
                for (var i = 1; i < axes.length; i++) {
                    _columns.push(i);
                };
                return _columns;
            },
            addColumnsForSecondayAxes: function (axesName, axesData, columns) {
                var _columns = [0];
                for (var col = 1; col < columns.length; col++) {
                    _columns.push(columns[col]);
                }
                var _entry = returnIndex(axesName, axesData);
                if (_entry > 0) {
                    _columns = pushColumns(_columns, singleAxes(_entry, axesData).columns);
                }
                return _columns;
            },
            displayAxesByName: function (axesNames, axesData) {
                var _columns = [0];
                for (index = 0; index < axesNames.length; index++) {
                    var _entry = returnIndex(axesNames[index], axesData);
                    _columns = pushColumns(_columns, singleAxes(_entry, axesData).columns);
                }
                //_columns.push(9);
                return _columns;

            },
            selectAxesByName: function (axesNames, axesData) {
                var _displayAxes = [];
                var _columns = [0];
                //var returnIndex = function (forName, data) {
                //    for (var j = 0; j < data.length; j++) {
                //        if (data[j].title == forName) {
                //            return j;
                //        }
                //    }
                //    return -1;
                //};
                for (index = 0; index < axesNames.length; index++) {
                    var _entry = returnIndex(axesNames[index], axesData);
                    // console.log(_entry);
                    //  console.log(singleAxes(_entry, axesData));
                    //_displayAxes.push(singleAxes(_entry, axesData).axes);
                    _displayAxes = pushColumns(_displayAxes, singleAxes(_entry, axesData).axes);
                    _columns = pushColumns(_columns, singleAxes(_entry, axesData).columns);
                    //console.log(_columns);
                    //_columns.push(singleAxes(_entry, axesData).columns);
                }
                //console.log(_displayAxes);
                //console.log(_columns);
                var _data = { axes: _displayAxes, columns: _columns };
                return _data;
            },
            elementIndex: function (elements, title) {
                // Return index for this title
                for (var i = 0; i < elements.length; i++) {
                    if (elements[i].title == title) { return i }
                };
                return -1;
            }
        };

    });