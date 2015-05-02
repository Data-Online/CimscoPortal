var gridbordercolor = "#eee";


var InitiateSparklineCharts = function () {
    return {
        init: function () {

            /*Bar*/
            var sparklinebars = $('[data-sparkline=bar]');
            $.each(sparklinebars, function () {
                $(this).sparkline('html', {
                    type: 'bar',
                    disableHiddenCheck: true,
                    height: $(this).data('height'),
                    width: $(this).data('width'),
                    barColor: getcolor($(this).data('barcolor')),
                    negBarColor: getcolor($(this).data('negbarcolor')),
                    zeroColor: getcolor($(this).data('zerocolor')),
                    barWidth: $(this).data('barwidth'),
                    barSpacing: $(this).data('barspacing'),
                    stackedBarColor: $(this).data('stackedbarcolor')
                });
            });

            /*Line*/
            var sparklinelines = $('[data-sparkline=line]');
            $.each(sparklinelines, function () {
                $(this).sparkline('html', {
                    type: 'line',
                    disableHiddenCheck: true,
                    height: $(this).data('height'),
                    width: $(this).data('width'),
                    fillColor: getcolor($(this).data('fillcolor')),
                    lineColor: getcolor($(this).data('linecolor')),
                    spotRadius: $(this).data('spotradius'),
                    lineWidth: $(this).data('linewidth'),
                    spotColor: getcolor($(this).data('spotcolor')),
                    minSpotColor: getcolor($(this).data('minspotcolor')),
                    maxSpotColor: getcolor($(this).data('maxspotcolor')),
                    highlightSpotColor: getcolor($(this).data('highlightspotcolor')),
                    highlightLineColor: getcolor($(this).data('highlightlinecolor'))
                });
            });
            /*Composite Line*/
            var sparklinecompositelines = $('[data-sparkline=compositeline]');
            $.each(sparklinecompositelines, function () {
                $(this).sparkline('html', {
                    type: 'line',
                    disableHiddenCheck: true,
                    height: $(this).data('height'),
                    width: $(this).data('width'),
                    lineColor: getcolor($(this).data('linecolor')),
                    fillColor: getcolor($(this).data('fillcolor')),
                    spotRadius: $(this).data('spotradius'),
                    lineWidth: $(this).data('linewidth'),
                    spotColor: getcolor($(this).data('spotcolor')),
                    minSpotColor: getcolor($(this).data('minspotcolor')),
                    maxSpotColor: getcolor($(this).data('maxspotcolor')),
                    highlightSpotColor: getcolor($(this).data('highlightspotcolor')),
                    highlightLineColor: getcolor($(this).data('highlightlinecolor'))
                });
                $(this).sparkline(stringtoarray($(this).attr("data-composite")), {
                    type: 'line',
                    disableHiddenCheck: true,
                    height: $(this).data('height'),
                    width: $(this).data('width'),
                    lineColor: getcolor($(this).data('secondlinecolor')),
                    fillColor: getcolor($(this).data('secondfillcolor')),
                    lineWidth: $(this).data('secondlinewidth'),
                    spotRadius: $(this).data('spotradius'),
                    spotColor: getcolor($(this).data('spotcolor')),
                    minSpotColor: getcolor($(this).data('minspotcolor')),
                    maxSpotColor: getcolor($(this).data('maxspotcolor')),
                    highlightSpotColor: getcolor($(this).data('highlightspotcolor')),
                    highlightLineColor: getcolor($(this).data('highlightlinecolor')),
                    composite: true
                });
            });

            /*Composite Bar*/
            var sparklinecompositebars = $('[data-sparkline=compositebar]');
            $.each(sparklinecompositebars, function () {
                $(this).sparkline('html', {
                    type: 'bar',
                    disableHiddenCheck: true,
                    height: $(this).data('height'),
                    width: $(this).data('width'),
                    barColor: getcolor($(this).data('barcolor')),
                    negBarColor: getcolor($(this).data('negbarcolor')),
                    zeroColor: getcolor($(this).data('zerocolor')),
                    barWidth: $(this).data('barwidth'),
                    barSpacing: $(this).data('barspacing'),
                    stackedBarColor: getcolor($(this).data('stackedbarcolor'))
                });
                $(this).sparkline(stringtoarray($(this).attr("data-composite")), {
                    type: 'line',
                    height: $(this).data('height'),
                    disableHiddenCheck: true,
                    width: $(this).data('width'),
                    lineColor: getcolor($(this).data('linecolor')),
                    fillColor: getcolor($(this).data('fillcolor')),
                    spotRadius: $(this).data('spotradius'),
                    lineWidth: $(this).data('linewidth'),
                    spotRadius: $(this).data('spotradius'),
                    spotColor: getcolor($(this).data('spotcolor')),
                    minSpotColor: getcolor($(this).data('minspotcolor')),
                    maxSpotColor: getcolor($(this).data('maxspotcolor')),
                    highlightSpotColor: getcolor($(this).data('highlightspotcolor')),
                    highlightLineColor: getcolor($(this).data('highlightlinecolor')),
                    composite: true
                });

            });

            /*Tristate*/
            var sparklinetristates = $('[data-sparkline=tristate]');
            $.each(sparklinetristates, function () {
                $(this).sparkline('html', {
                    type: 'tristate',
                    disableHiddenCheck: true,
                    height: $(this).data('height'),
                    width: $(this).data('width'),
                    posBarColor: getcolor($(this).data('posbarcolor')),
                    negBarColor: getcolor($(this).data('negbarcolor')),
                    zeroBarColor: getcolor($(this).data('zerobarcolor')),
                    barWidth: $(this).data('barwidth'),
                    barSpacing: $(this).data('barspacing'),
                    zeroAxis: $(this).data('zeroaxis')
                });
            });

            /*Descrete*/
            var sparklinediscretes = $('[data-sparkline=discrete]');
            $.each(sparklinediscretes, function () {
                $(this).sparkline('html', {
                    type: 'discrete',
                    disableHiddenCheck: true,
                    lineHeight: $(this).data('lineheight'),
                    lineColor: getcolor($(this).data('linecolor')),
                    thresholdValue: $(this).data('thresholdvalue'),
                    thresholdColor: $(this).data('thresholdcolor')
                });
            });

            /*Bullet*/
            var sparklinebullets = $('[data-sparkline=bullet]');
            $.each(sparklinebullets, function () {
                $(this).sparkline('html', {
                    type: 'bullet',
                    disableHiddenCheck: true,
                    targetColor: $(this).data('targetcolor'),
                    performanceColor: $(this).data('performancecolor'),
                    rangeColors: $(this).data('rangecolors')
                });
            });

            /*Box Plot*/
            var sparklinebox = $('[data-sparkline=box]');
            $.each(sparklinebox, function () {
                $(this).sparkline('html', {
                    type: 'box',
                    disableHiddenCheck: true,
                });
            });

            /*Pie*/
            var sparklinepie = $('[data-sparkline=pie]');
            $.each(sparklinepie, function () {
                $(this).sparkline('html', {
                    type: 'pie',
                    disableHiddenCheck: true,
                    width: $(this).data('width'),
                    height: $(this).data('height'),
                    sliceColors: $(this).data('slicecolors'),
                    borderColor: getcolor($(this).data('bordercolor'))
                });
            });

            /*PieTST*/
            var sparklinepie = $('[data-sparkline=pietst]');
            $.each(sparklinepie, function () {
                $(this).sparkline(getjsonData($(this).data('viewmodel')), {
                    type: 'pie',
                    disableHiddenCheck: true,
                    width: $(this).data('width'),
                    height: $(this).data('height'),
                    sliceColors: $(this).data('slicecolors'),
                    borderColor: getcolor($(this).data('bordercolor'))
                });
            });

            /*Composite BarTST*/
            var sparklinecompositebars = $('[data-sparkline=compositebartst]');
            $.each(sparklinecompositebars, function () {
                $(this).sparkline(getjsonData($(this).data('viewmodel'), '1'), {
                    type: 'bar',
                    disableHiddenCheck: true,
                    height: $(this).data('height'),
                    width: $(this).data('width'),
                    barColor: getcolor($(this).data('barcolor')),
                    negBarColor: getcolor($(this).data('negbarcolor')),
                    zeroColor: getcolor($(this).data('zerocolor')),
                    barWidth: $(this).data('barwidth'),
                    barSpacing: $(this).data('barspacing'),
                    stackedBarColor: getcolor($(this).data('stackedbarcolor'))
                });
                $(this).sparkline(getjsonData($(this).data('viewmodel'), '2'), {
                    type: 'line',
                    height: $(this).data('height'),
                    disableHiddenCheck: true,
                    width: $(this).data('width'),
                    lineColor: getcolor($(this).data('linecolor')),
                    fillColor: getcolor($(this).data('fillcolor')),
                    spotRadius: $(this).data('spotradius'),
                    lineWidth: $(this).data('linewidth'),
                    spotRadius: $(this).data('spotradius'),
                    spotColor: getcolor($(this).data('spotcolor')),
                    minSpotColor: getcolor($(this).data('minspotcolor')),
                    maxSpotColor: getcolor($(this).data('maxspotcolor')),
                    highlightSpotColor: getcolor($(this).data('highlightspotcolor')),
                    highlightLineColor: getcolor($(this).data('highlightlinecolor')),
                    composite: true
                });

            });

        }
    };
}();


function stringtoarray(str) {
    var myArray = str.split(",");
    for (var i = 0; i < myArray.length; i++) {
        myArray[i] = +myArray[i];
    }
    for (var i = 0; i < myArray.length; i++) {
        myArray[i] = parseInt(myArray[i], 10);
    }
    return myArray;
}

function getjsonData2() {

    //var myArray = new Array();
    //$.ajax({type: 'GET', url: '/home/getdata',
    //    dataType: 'json', 
    //    success: function(data) { $.each(data, function (i) { myArray.push(data[i]) } },
    //    async: false;
    //});
    ////alert(myArray.length);
    //return myArray;
}

function getjsonData(viewmodel, dim) {
    if (typeof dim === 'undefined') { dim = ''; }
    //alert(viewmodel+' '+dim);
    var myArray = [];
    // Ensures the data is returned before passing to spark element
    $.ajaxSetup({ "async": false });
    var jq = $.getJSON("/home/getdata/" + viewmodel + '_' + dim)
        .done(function (data) {
            $.each(data, function (i) { myArray.push(data[i]) });
        });
    //alert(target);
    //myArray = [1, 2, 3, 4];
    return myArray;
}

