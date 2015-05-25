var gridbordercolor = "#eee";


var InitiateSparklineCharts = function () {
    return {
        init: function () {

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
    $.ajaxSetup( { "async": false } );  
    var jq = $.getJSON("/home/getdata/"+viewmodel+'_'+dim)
        .done(function (data) {
            $.each(data, function (i) { myArray.push(data[i]) });
        });
    //alert(target);
    //myArray = [1, 2, 3, 4];
    return myArray;
}

