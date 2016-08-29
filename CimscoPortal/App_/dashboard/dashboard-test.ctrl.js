(function () {
    'use strict';

    angular
        .module("app.dashboard")
        .constant("dbConstants", {
            "costsDataElement": "costsBarChart",
            "consDataElement": "consBarChart",
            "filterSelectDelay": "15"
            //,"filters": ["divisions", "categories"]
        })
        .controller("app.dashboard.ctrl", dashboard);

    dashboard.$inject = ['$scope', '$parse', '$interval', '$timeout', 'dbDataSource', 'userDataSource', 'filterData', 'dbConstants', 'toaster', 'googleChart'];
    function dashboard($scope, $parse, $interval, $timeout, dbDataSource, userDataSource, filterData, dbConstants, toaster, googleChart ) {


        var onWelcomeMessage = function (data) {
            $scope.welcomeHeader = data.header;
            $scope.welcomeText = data.text;
        };

        var onUserData = function (data) {
            $scope.monthSpanOptions = data.monthSpanOptions;
            $scope.monthSpan = data.monthSpan;
             //dbDataSource.getAllFilters()
             ////filterData.getAllFilters()
             //   .then(onFiltersOk, onError);
        };

        var onFiltersOk = function (data) {
            //filterData.zztest();
            //createMultiDropdown('divisions', data.divisions, true);
            filterData.createMultiDropdown('divisions', data.divisions, true, $scope);
            //createMultiDropdown('categories', data.categories, true);
            filterData.createMultiDropdown('categories', data.categories, true, $scope);
        };

        var onError = function (reason) {
            $scope.reason = reason;
        };

        dbDataSource.getWelcomeScreen()
            .then(onWelcomeMessage, onError);

        userDataSource.getUserData()
           .then(onUserData, onError);

        filterData.getAllFilters()
                .then(onFiltersOk, onError);

    };

    String.prototype.capitalizeFirstLetter = function () {
        return this.charAt(0).toUpperCase() + this.slice(1);
    };
    var yearFromArray = function (v, yearArray) {
        return yearArray.years[yearArray.months.indexOf(v.label) + GetOffSet(yearArray.length, v.x)];
    };
    var totalInvoicesFromArray = function (v, yearArray, whichYear) {
        if (whichYear == 12) {
            return yearArray.invoices12[yearArray.months.indexOf(v.label) + GetOffSet(yearArray.length, v.x)];
        }
        else {
            return yearArray.invoices[yearArray.months.indexOf(v.label) + GetOffSet(yearArray.length, v.x)];
        }
    };

    function GetOffSet(length, x) {
        if (length > 12 && v.x > 400)
            return 12
        else
            return 0
    };


    function singleTooltip(v, yearArray) {
        //console.log(v);
        var unit = v.datasetLabel.split(":").pop();
        if (v.datasetLabel.split(":", 1) != 'current') {
            _yearNumber = _yearNumber - 1;
            var _invNumber = totalInvoicesFromArray(v, yearArray, 12);
        }
        else {
            var _invNumber = totalInvoicesFromArray(v, yearArray);
        }
        //    console.log(pluralise(_invNumber));
        if (unit == "$")
            var _format = v.label + ' ' + yearFromArray(v, yearArray) + ' : ' + unit + numberWithCommas(v.value) + ' (' + _invNumber + ' invoice' + pluralise(_invNumber) + ')';
        else
            var _format = v.label + ' ' + yearFromArray(v, yearArray) + ' : ' + numberWithCommas(v.value) + ' ' + unit + ' (' + _invNumber + ' invoice' + pluralise(_invNumber) + ')';
        return (_format);
        // return v.label + ' ' + yearFromArray(v, yearArray) + ' : ' + '$' + v.value.toFixed(2);
    }

    function multiTooltip(v, yearArray) {
        var _yearNumber = yearFromArray(v, yearArray);

        var unit = v.datasetLabel.split(":").pop();
        if (v.datasetLabel.split(":", 1) != 'current') {
            _yearNumber = _yearNumber - 1;
            var _invNumber = totalInvoicesFromArray(v, yearArray, 12);
        }
        else {
            var _invNumber = totalInvoicesFromArray(v, yearArray);
        }
        //   console.log(pluralise(_invNumber));
        if (unit == "$")
            var _format = _yearNumber + ' : ' + unit + numberWithCommas(v.value) + ' (' + _invNumber + ' invoice' + pluralise(_invNumber) + ')';
        else
            var _format = _yearNumber + ' : ' + numberWithCommas(v.value) + ' ' + unit + ' (' + _invNumber + ' invoice' + pluralise(_invNumber) + ')';
        return (_format);
    }

    function pluralise(count) {
        // Very simple at the moment
        //  console.log(count);
        if (count.length >= 0)
        { return "s"; }
        else
        { return ""; }
    };

    function numberWithCommas(x) {
        return x.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

})();


//data.cols = [];
//var colpart = { id: "2", label: "Month", title: "Month", type: "string" };
//data.cols.push(colpart);
//var colpart = { id: "3", title: "Invoice Total excl GST", label: "Invoice Total excl GST", type: "number" };
//data.cols.push(colpart);
//var colpart = { id: "8", type: "string", role: "tooltip", p: { 'html': true } };
//data.cols.push(colpart);
//var colpart = { id: "4", type: "number", title: "Test", label: "test" };
//data.cols.push(colpart);
//var colpart = { id: "7", type: "string", role: "tooltip" };
//data.cols.push(colpart);
//var colpart = { id: "5", title: "Cost / Sqm", label: "Cost / Sqm", type: "number" };
//data.cols.push(colpart);
//var colpart = { id: "6", title: "Kwh / SqM", label: "Kwh / SqM", type: "number" };
//data.cols.push(colpart);

//data.rows = [];
//var cpart = [{ v: "August", f: null }, { v: "$0.0", f: null }, { v: "$0.0", f: null }, { v: "$0.0", f: null }, { v: "$0.0", f: null }, { v: "$0.0", f: null }, { v: "$0.0", f: null }];
//data.rows.push({ c: cpart });
//var cpart = [{ v: "September", f: null }, { v: "32218.3900", f: "$32,218.39 (3 invoices)" }, { f: "<div style='width:150px; margin:10px;'>September</br><b>$32,218.39</br>(3 invoices)</b></div>" },
//                { v: "42218.3900", f: "$32,218.39 (3 invoices)" }, { f: "A label" },
//                { v: "52218.3900", f: "$32,218.39 (3 invoices)" }, { v: "62218.3900", f: "$32,218.39 (3 invoices)" }];
//data.rows.push({ c: cpart });