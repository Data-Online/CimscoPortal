var gridbordercolor = "#eee";

var tax_data = [
                 { "period": "2011 Q3", "licensed": 3407, "sorned": 660 },
                 { "period": "2011 Q2", "licensed": 3351, "sorned": 629 },
                 { "period": "2011 Q1", "licensed": 3269, "sorned": 618 },
                 { "period": "2010 Q4", "licensed": 3246, "sorned": 661 },
                 { "period": "2009 Q4", "licensed": 3171, "sorned": 676 },
                 { "period": "2008 Q4", "licensed": 3155, "sorned": 681 },
                 { "period": "2007 Q4", "licensed": 3226, "sorned": 620 },
                 { "period": "2006 Q4", "licensed": 3245, "sorned": null },
                 { "period": "2005 Q4", "licensed": 3289, "sorned": null }
];

var json_data = [
                  { Month: 'Jan', Energy: 100, Line: 90, Other: 80 },
                  { Month: 'Feb', Energy: 75, Line: 65, Other: 25 },
                  { Month: 'March', Energy: 50, Line: 40, Other: 90 },
                  { Month: 'April', Energy: 75, Line: 65, Other: 15 },
                  { Month: 'May', Energy: 50, Line: 40, Other: 50 },
                  { Month: 'June', Energy: 75, Line: 65, Other: 10 },
                  { Month: 'July', Energy: 100, Line: 90, Other: 90 },
                  { Month: 'Aug', Energy: 100, Line: 90, Other: 80 },
                  { Month: 'Sept', Energy: 75, Line: 65, Other: 25 },
                  { Month: 'Oct', Energy: 50, Line: 40, Other: 90 },
                  { Month: 'Nov', Energy: 75, Line: 65, Other: 15 },
                  { Month: 'Dec', Energy: 75, Line: 65, Other: 15 }

];

var InitiateAreaChart = function () {
    return {
        init: function () {
            Morris.Area({
                element: 'area-chart',
                data: [
                  { period: '2010 Q1', iphone: 2666, ipad: null, itouch: 2647 },
                  { period: '2010 Q2', iphone: 2778, ipad: 2294, itouch: 2441 },
                  { period: '2010 Q3', iphone: 4912, ipad: 1969, itouch: 2501 },
                  { period: '2010 Q4', iphone: 3767, ipad: 3597, itouch: 5689 },
                  { period: '2011 Q1', iphone: 6810, ipad: 1914, itouch: 2293 },
                  { period: '2011 Q2', iphone: 5670, ipad: 4293, itouch: 1881 },
                  { period: '2011 Q3', iphone: 4820, ipad: 3795, itouch: 1588 },
                  { period: '2011 Q4', iphone: 15073, ipad: 5967, itouch: 5175 },
                  { period: '2012 Q1', iphone: 10687, ipad: 4460, itouch: 2028 },
                  { period: '2012 Q2', iphone: 8432, ipad: 5713, itouch: 1791 }
                ],
                xkey: 'period',
                ykeys: ['iphone', 'ipad', 'itouch'],
                labels: ['iPhone', 'iPad', 'iPod Touch'],
                pointSize: 2,
                hideHover: 'auto',
                lineColors: [themethirdcolor, themesecondary, themeprimary]
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
                xkey: 'Month',
                ykeys: ['Energy', 'Line', 'Other'],
                labels: ['Energy', 'Line', 'Other'],
                hideHover: 'auto',
                barColors: [themeprimary, themesecondary, themethirdcolor],
                stacked: true
            });
        }
    };
}();

var InitiateLineChart = function () {
    return {
        init: function () {
            Morris.Line({
                element: 'line-chart',
                data: tax_data,
                xkey: 'period',
                ykeys: ['licensed', 'sorned'],
                labels: ['Licensed', 'Off the road'],
                lineColors: [themeprimary, themethirdcolor]
            });

        }
    };
}();

var InitiateLineChart2 = function () {
    return {
        init: function () {
            Morris.Line({
                element: 'line-chart-2',
                data: [
                  { y: '2006', a: 100, b: 90 },
                  { y: '2007', a: 75, b: 65 },
                  { y: '2008', a: 50, b: 40 },
                  { y: '2009', a: 75, b: 65 },
                  { y: '2010', a: 50, b: 40 },
                  { y: '2011', a: 75, b: 65 },
                  { y: '2012', a: 100, b: 90 }
                ],
                xkey: 'y',
                ykeys: ['a', 'b'],
                labels: ['Series A', 'Series B'],
                lineColors: [themeprimary, themethirdcolor]
            });

        }
    };
}();

var InitiateDonutChart = function () {
    return {
        init: function (json, elementId) {
            elementId += "Donut";
            //alert(elementId);
            Morris.Donut({
                element: elementId,
                data: json,
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
            //alert(donutCharts.length);
            var AJAXdata = [];
            AJAXdata.push('header');
            $.each(donutCharts, function () {
                var dataFor = $(this).data('datafor');               
                AJAXdata.push(getJsonData('DonutChartData', dataFor));
                //var objectNo = $(this).data('objectno');
            });
            $.when.apply($, AJAXdata).done(function (data) {
                var obj = [];
                for (var i = 0, len = arguments.length; i < len; i++) {
                    obj.push(arguments[i][0]);
                }
                // Read Html element, apply element id for Donut chart
                // 
                for (var i = 1, len = obj.length; i < len; i++)
                {
                    updateDonutChartElementOnPage(obj[i]);
                    //alert(obj[i].DataFor);
                    InitiateDonutChart.init(obj[i].DonutChartData, obj[i].DataFor);
                   // alert(obj[i].DonutChartData.length);
                    
                }
               // InitiateDonutChart.init(obj[1].DonutChartData, obj[1].DataFor);
                //alert(obj[2].length);
                //alert(obj[1].SummaryData[0].Title);
            });
        }
    }
}();

function updateDonutChartElementOnPage(data) {
    var pageElement = document.getElementById(data.DataFor).innerHTML;
    var infoLines = document.getElementById(data.DataFor).getElementsByClassName("databox-infoLines")[0].innerHTML;

    document.getElementById(data.DataFor).innerHTML = '';
    var newpageElement = '';
    newpageElement = insertModelDataIntoElementA(data, pageElement);
    document.getElementById(data.DataFor).innerHTML = newpageElement;

    var newinfoLines = '';
    for (var i = 0; i < data.SummaryData.length; i++) {
        newinfoLines += insertModelDataIntoElementB(data.SummaryData[i], infoLines);
    }
    document.getElementById(data.DataFor).getElementsByClassName("databox-infoLines")[0].innerHTML = newinfoLines;
}

function insertModelDataIntoElementA(data, element) {
    return element
                    .replace('{{dataFor}}', data.DataFor)
                    .replace('{{header}}', data.Header)
    ;
}

function insertModelDataIntoElementB(data, element) {
    return element
                    .replace('{{title}}', data.Title)
                    .replace('{{detail}}', data.Detail)
    ;
}

var seedDonutChart = function () {
    return {
        summary: function (pageElementId, dataElementId, summaryData) {
            var fullTemplate = document.getElementById(pageElementId).innerHTML;
            var dataLines = document.getElementById("databox-infoLines").innerHTML;
        },
        datalines: function (pageElementId) { },
        test: function () {
            
            var donutCharts = $('[data-display=donutChartPlusSummary]');
            alert(donutCharts.length);
            $.each(donutCharts, function () {
                alert($(this).data('viewmodel'))
                });
        }

    }
}();


function getJsonData(elementType, elementDataName) {
    var url = "/portal/" + elementType + "/" + elementDataName;//GetPieChartData"
    return $.getJSON(url);
}
    //var InitiateDonutChart = function () {
    //    return {
    //        init: function () {
    //            Morris.Donut({
    //                element: 'donut-chart',
    //                data: [
    //                  { label: 'Line', value: 3540.45 , },
    //                  { label: 'Energy', value: 10968.34 },
    //                  { label: 'Other', value: 234.89 }
    //                ],
    //                colors: [themeprimary, themesecondary, themethirdcolor, themefourthcolor],
    //                formatter: function (y) { return "$" + y }
    //            });
    //        }
    //    };



