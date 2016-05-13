(function () {
    //var monthSpan = 12;
    'use strict';

    angular
        .module("app.dashboard")
        .controller("app.dashboard.ctrl", dashboard);

    dashboard.$inject = ['$scope', 'dbDataSource', 'userDataSource'];
    function dashboard($scope, dbDataSource, userDataSource) {

        userDataSource.getUserData()
            .then(onUserData, onError);

        function onUserData(data) {
            $scope.monthSpanOptions = data.monthSpanOptions;
            $scope.monthSpan = data.monthSpan;
            console.log(data.monthSpanOptions);
        };

        function onError(reason) {
            console.log('Error reading user data');
            $scope.reason = reason;
        };

        var yearArray = {
            years: [2015, 2015, 2016, 2016, 2016, 2016, 2016, 2016, 2016, 2016, 2016, 2016],
            months: ['November', 'December', 'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October']
        };

        var firstDataset = {
            label: '2016',
            fillColor: themeprimary,
            strokeColor: themeprimary, //'rgba(220,220,220,1.0)',
            highlightFill: themeprimary,
            highlightStroke: 'rgba(0,0,0,0.5)',
            data: [65, 59, 80, 81, 56, 55, 40, 45, 34, 98, 49, 23]
        };
        var secondDataset = {
            label: '2015',
            fillColor: themesecondary,
            strokeColor: themesecondary, //'rgba(151,187,205,1.0)',
            highlightFill: themesecondary,
            highlightStroke: 'rgba(0,0,0,0.5)',
            data: [28, 48, 40, 19, 86, 27, 90, 45, 34, 98, 49, 23]
        };

        $scope.siteCategories = ['Hardware and Building Supplies Retailing'];
        $scope.divisions = ['MEGA Stores', 'Mitre 10 Stores'];
        $scope.groupCompanyName = 'Mitre 10 New Zealand';
        $scope.togglePreviousYearsData = function ($event) {
            var checkbox = $event.target;
            refreshData(checkbox.checked);
        };

        function refreshData(showPreviousYear) {
            //console.log('refreshData called ..');

            if (showPreviousYear) {
                $scope.barChartData = {
                    labels: yearArray.months,
                    datasets: [firstDataset, secondDataset]
                }
            }
            else {
                $scope.barChartData = {
                    labels: yearArray.months,
                    datasets: [firstDataset]
                }
            }
        };

        refreshData(false);

        // Chart.js Options
        $scope.barChartOptions = {

            multiTooltipTemplate: "<%=datasetLabel%> : <br\> $<%= value %>",

            tooltipTemplate: function (v) { return singleTooltip(v, yearArray); },
            //tooltipTemplate: "<%=label%> : $<%= value %>",
            // Sets the chart to be responsive
            responsive: true,

            //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
            scaleBeginAtZero: true,

            //Boolean - Whether grid lines are shown across the chart
            scaleShowGridLines: true,

            //String - Colour of the grid lines
            scaleGridLineColor: "rgba(0,0,0,.05)",

            //Number - Width of the grid lines
            scaleGridLineWidth: 1,

            //Boolean - If there is a stroke on each bar
            barShowStroke: true,

            //Number - Pixel width of the bar stroke
            barStrokeWidth: 2,

            //Number - Spacing between each of the X value sets
            barValueSpacing: 5,

            //Number - Spacing between data sets within X values
            barDatasetSpacing: 1,

            //String - A legend template
            //legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>'
            legendTemplate: ' '
        };
    };


    function singleTooltip(v, yearArray) {
        //console.log(zz);
        return v.label + ' ' + yearArray.years[yearArray.months.indexOf(v.label)] + ' : ' + '$' + v.value;
    }
    function multiTooltip(v) {
        return yearArray.years[0] + ' : ' + '$' + v.value;
    }



})();