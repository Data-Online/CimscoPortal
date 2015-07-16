var createSparklineCharts = function () {
    return {
        composite: function () {
            var elementCount = 1;
            var sparklineCharts = $('[data-display=sparklineComposite]');
            $.each(sparklineCharts, function () {
                var elementId = elementCount + 'SlineComposite';
                InitiateSparklineCharts.composite(elementId);
                elementCount = elementCount + 1;
            }
            )
        },
        bar: function () {
            var elementCount = 1;
            var sparklineCharts = $('[data-display=sparklineBar]');
            $.each(sparklineCharts, function () {
                var elementId = elementCount + 'SlineBar';
                InitiateSparklineCharts.stdbar(elementId);
                elementCount = elementCount + 1;
            }
            )
        }
    };
}();

var InitiateSparklineCharts = function () {
    return {
        composite: function (elementId) {
            /*Composite Bar*/
            var sparklinecompositebars = $('#' + elementId + ' span');
            $.each(sparklinecompositebars.first(), function () {
                var seriesa = $(this).data('seriesa');
                var seriesb = $(this).data('seriesb');
                $(this).sparkline(seriesa, {
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
                    chartRangeMax: $(this).data('maxvalueforbar'),//2400.00
                    chartRangeMin: $(this).data('minvalueforbar')
                });
                $(this).sparkline(seriesb, {                  //json.energyChargesByBracket, {
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
                    tooltipFormat: '<span style="color: {{color}}">&#9679;</span> cost per kw: {{y}}c',
                    composite: true,
                    chartRangeMax: $(this).data('maxSeriesb'),
                    chartRangeMin: $(this).data('minSeriesb')
                });

            });
        },
        stdbar: function (elementId) {
            /*Composite Bar*/
            var sparklinebars = $('#' + elementId + ' span');
            $.each(sparklinebars.first(), function () {
                var seriesa = $(this).data('seriesa');
                var seriesb = $(this).data('seriesb');
                $(this).sparkline(seriesa, {
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
                    chartRangeMax: $(this).data('maxvalueforbar'),//2400.00
                    chartRangeMin: $(this).data('minvalueforbar')
                });
            });
        }
    }
}();