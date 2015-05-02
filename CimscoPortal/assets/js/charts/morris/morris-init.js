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


var InitiateBarChart = function () {

    return {
        init: function (json, elementId) {
            Morris.Bar({
                element: elementId,
                data: json,
                xkey: 'month',
                ykeys: ['energy', 'line', 'other'],
                labels: ['Energy', 'Line', 'Other'],
                hideHover: 'auto',
                barColors: [themeprimary, themesecondary, themethirdcolor],
                stacked: true
                //,
                //hoverCallback: function (index, options, content) {
                //    var row = options.data[index];
                //    return "sin(" + row.x + ") = " + row.y;
                //}
            });
        }
    };
}();

var InitiateDonutChart = function () {
    return {
        init: function (json, elementId) {
           // alert(element);
            //var elementId = json.headerData.dataFor + "Donut";
            //alert(elementId);
            Morris.Donut({
                element: elementId,
                data: json.donutChartData,
                colors: [themeprimary, themesecondary, themethirdcolor, themefourthcolor],
                formatter: function (y) { return "$" + y }
            });
        }
    };
}();
// Custom code to manage generic object types

var createPageDonutCharts = function () {
    return {
        withSummary: function () {
            var donutCharts = $('[data-display=donutChartPlusSummary]');
            var AJAXdata = [];
            AJAXdata.push('header');
            $.each(donutCharts, function () {
                var dataFor = $(this).data('datafor');               
                AJAXdata.push(getJsonData('DonutChartData', dataFor));
            });
            $.when.apply($, AJAXdata).done(function (data) {
                var obj = [];
                for (var i = 0, len = arguments.length; i < len; i++) {
                    obj.push(arguments[i][0]);
                }
                // Read Html element, apply element id for Donut chart
                for (var i = 1, len = obj.length; i < len; i++)
                {
                    var elementId = updateDonutChartElementOnPage(obj[i]);
                    InitiateDonutChart.init(obj[i], elementId);
                }
            });
        }
    }
}();

var createPageBarCharts = function () {
    return {
        standard: function () {
            var AJAXdata = [];
            AJAXdata.push({});
            AJAXdata.push(getJsonData('GetMonthlyEnergySummary', 'a'));
            $.when.apply($, AJAXdata).done(function (data) {
                var obj = [];
                obj.push({});
                for (var i = 1, len = arguments.length; i < len; i++) {
                    obj.push(arguments[i][0]);
                }
                updateBarChartElementOnPage(obj[1].barChartSummaryData);
                InitiateBarChart.init(obj[1].monthlyData, 'id-yearToDateChart');
            });
        }
    }
}();

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
                    var elementId = updateElementOnPage.compositeSparkline(obj[i]);
                    InitiateSparklineCharts.init(obj[i], elementId);
                }              
            });
        }
    }
}();

function updateDonutChartElementOnPage(data) {
    var pageElement = document.getElementById(data.headerData.dataFor).innerHTML;
    var infoLines = document.getElementById(data.headerData.dataFor).getElementsByClassName("databox-infoLines")[0].innerHTML;
    
    document.getElementById(data.headerData.dataFor).innerHTML = '';
    var newpageElement = '';
    newpageElement = insertModelDataIntoElement(pageElement, data.headerData);
    
    document.getElementById(data.headerData.dataFor).innerHTML = newpageElement;
    
    var graphicElementId = $("#" + data.headerData.dataFor + " .chart").attr('id'); //alert(graphicElementId);
    var newinfoLines = '';
    for (var i = 0; i < data.summaryData.length; i++) {
        //data.summaryData[i].detail = '$'+parseFloat(data.summaryData[i].detail).toMoney();
        newinfoLines += insertModelDataIntoElement(infoLines, data.summaryData[i]);
    }
    document.getElementById(data.headerData.dataFor).getElementsByClassName("databox-infoLines")[0].innerHTML = newinfoLines;

    return graphicElementId;
}

function updateBarChartElementOnPage(data) {
    var pageElement = document.getElementById('yearToDateGraph').innerHTML;
    document.getElementById('yearToDateGraph').innerHTML = '';
    var newPageElement = '';
    newPageElement = insertModelDataIntoElement(pageElement, data);
    document.getElementById('yearToDateGraph').innerHTML = newPageElement;
}


var updateElementOnPage = function () {
    return {
        compositeSparkline: function (data) {
            var pageElement = document.getElementById(data.headerData.dataFor).innerHTML;
            //alert(pageElement);
            var newpageElement = '';
            newpageElement = insertModelDataIntoElement(pageElement, data.headerData);
            newpageElement = insertModelDataIntoElement(newpageElement, data);
            document.getElementById(data.headerData.dataFor).innerHTML = newpageElement;
            var graphicElementId = $("#" + data.headerData.dataFor + " .cp-graph-element").attr('id');
            return graphicElementId;
            //alert(graphicElementId);
        }
    }
}();

// GPA: 1. Standard function for mapping required

function insertModelDataIntoElement(element, data) {
    var $currentElem = new RegExp();
    for (var key in data) {
        $currentElem = new RegExp('{{' + key + '}}');
        element = element.replace($currentElem, data[key]);
    }
    return element;
}


//function insertModelDataIntoElementA(data, element) {
//    return element
//                    .replace('{{dataFor}}', data.dataFor)
//                    .replace('{{header}}', data.header)
//    ;
//}

//function insertModelDataIntoElementB(data, element) {
//    return element
//                    .replace('{{title}}', data.title)
//                    .replace('{{detail}}', data.detail) 
//    ;
//}

//function insertModelDataIntoElementC(data, element) {
//    return element
//                    .replace('{{title}}', data.title)
//                    .replace('{{subTitle}}', data.subTitle)
//                    .replace('{{percentChange}}', data.percentChange)
//    ;
//}

//function insertModelDataIntoElementD(data, element) {
//    return element
//                    .replace('{{maxCharge}}', data.maxCharge)
//                    .replace('{{totalCost}}', data.totalCost)
//    ;
//}

function getJsonData(elementType, elementDataName) {
    //alert("Get data" + elementType + ' ' + elementDataName);
    var url = "/portal/" + elementType + "/" + elementDataName;
    return $.getJSON(url);
}

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

/* 
decimal_sep: character used as deciaml separtor, it defaults to '.' when omitted
thousands_sep: char used as thousands separator, it defaults to ',' when omitted
*/
Number.prototype.toMoney = function (decimals, decimal_sep, thousands_sep) {
    var n = this,
    c = isNaN(decimals) ? 2 : Math.abs(decimals), //if decimal is zero we must take it, it means user does not want to show any decimal
    d = decimal_sep || '.', //if no decimal separator is passed we use the dot as default decimal separator (we MUST use a decimal separator)

    /*
    according to [http://stackoverflow.com/questions/411352/how-best-to-determine-if-an-argument-is-not-sent-to-the-javascript-function]
    the fastest way to check for not defined parameter is to use typeof value === 'undefined' 
    rather than doing value === undefined.
    */
    t = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep, //if you don't want to use a thousands separator you can pass empty string as thousands_sep value

    sign = (n < 0) ? '-' : '',

    //extracting the absolute value of the integer part of the number and converting to string
    i = parseInt(n = Math.abs(n).toFixed(c)) + '',

    j = ((j = i.length) > 3) ? j % 3 : 0;
    return sign + (j ? i.substr(0, j) + t : '') + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : '');
}



