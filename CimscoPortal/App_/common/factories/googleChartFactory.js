angular.module("googleChartControl", [])
    .factory('googleChart', function () {
        return {
            collateData: function (data) {
                var _cols = [];
                var _rows = [];
                var _cpart = [];

                for (var i = 0; i < data.columns.length; i++) {
                    var _nextCol = { id: data.columns[i].$id, title: data.columns[i].label, label: data.columns[i].label, type: data.columns[i].type };
                    _cols.push(_nextCol);
                }
                for (var i = 0; i < data.rows.length; i++) {
                    for (var j = 0; j < data.rows[i].cparts.length; j++) {
                        var c = { v: data.rows[i].cparts[j].v, f: data.rows[i].cparts[j].f }
                        _cpart.push(c);
                    }
                    _rows.push({ c: _cpart });
                    _cpart = [];
                }

                var _data = { cols: _cols, rows: _rows, cpart: _cpart };
                return _data;
            }
        };
    });