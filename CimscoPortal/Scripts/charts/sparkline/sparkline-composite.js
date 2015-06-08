var createSparklineCharts = function () {
    return {
        composite: function () {
            var sparklineCharts = $('[data-display=sparklineComposite]');
            var AJAXdata = [];
            AJAXdata.push({});
            $.each(sparklineCharts, function () {
                var dataFor = $(this).data('datafor');
                AJAXdata.push(getJsonData('GetSparklineDataFor', dataFor));
            });
            $.when.apply($, AJAXdata).done(function (data) {
                var obj = []; obj.push({});
                for (var i = 1, len = arguments.length; i < len; i++) {
                    obj.push(arguments[i][0]);
                }
                for (var i = 1, len = obj.length; i < len; i++) {
                    var elementId = i + 'SlineComposite';
                    InitiateSparklineCharts.init(obj[i], elementId);
                }
            });
        }
    }
}();



var InitiateSparklineCharts = function () {
    return {
        init: function (json, elementId) {
            /*Composite Bar*/
            //var sparklinecompositebars = $('[data-sparkline=compositebar]');
            var sparklinecompositebars = $('#' + elementId + ' span');//WeeklyEnergyBySliceSlineComposite' + ' span');
            $.each(sparklinecompositebars.first(), function () {
                $(this).sparkline(json.energyCostByBracket, {
                    type: 'bar',
                    disableHiddenCheck: true,
                    height: $(this).data('height'),
                    width: $(this).data('width'),
                    barColor: themeprimary, //getcolor($(this).data('barcolor')),
                    negBarColor: getcolor($(this).data('negbarcolor')),
                    zeroColor: getcolor($(this).data('zerocolor')),
                    barWidth: $(this).data('barwidth'),
                    barSpacing: $(this).data('barspacing'),
                    stackedBarColor: getcolor($(this).data('stackedbarcolor')),
                    tooltipFormat: '{{offset:slice}}<br/><span style="color: {{color}}">&#9679;</span> charge: ${{value}}',
                    tooltipValueLookups: {
                        slice: $.range_map({
                            '0': '00-04am', '1': '04-08am', '2': '08-12am',
                            '3': '12-16pm', '4': '16-20pm', '5': '20-24pm'
                        })
                    },
                    chartRangeMax: 2400.00
                });
                $(this).sparkline(json.energyChargesByBracket, {
                    type: 'line',
                    height: $(this).data('height'),
                    disableHiddenCheck: true,
                    width: $(this).data('width'),
                    lineColor: themesecondary, //getcolor($(this).data('linecolor')),
                    fillColor: getcolor($(this).data('fillcolor')),
                    spotRadius: $(this).data('spotradius'),
                    lineWidth: $(this).data('linewidth'),
                    spotRadius: $(this).data('spotradius'),
                    spotColor: getcolor($(this).data('spotcolor')),
                    minSpotColor: getcolor($(this).data('minspotcolor')),
                    maxSpotColor: getcolor($(this).data('maxspotcolor')),
                    highlightSpotColor: getcolor($(this).data('highlightspotcolor')),
                    highlightLineColor: getcolor($(this).data('highlightlinecolor')),
                    tooltipFormat: '<span style="color: {{color}}">&#9679;</span> cost per kw: ${{y}}',
                    composite: true,
                    chartRangeMax: 18.00
                });

            });
        }
    };
}();