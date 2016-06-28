(function () {
    'use strict';

    angular.module("app.detailBySite")
        .controller("app.detailBySite.ctrl", detailBySite)

    detailBySite.$inject = ['$scope', 'soDataSource', 'userDataSource'];
    function detailBySite($scope, soDataSource, userDataSource) {
        // $scope.monthSpanOptions = [3, 6, 12, 24];
        var monthSpan = 12; // Refactor out
        // Accordion
        $scope.oneAtATime = true;

        $scope.groups = [
          {
              title: 'Dynamic Group Header - 1',
              content: 'Dynamic Group Body - 1'
          },
          {
              title: 'Dynamic Group Header - 2',
              content: 'Dynamic Group Body - 2'
          }
        ];

        $scope.items = ['Item 1', 'Item 2', 'Item 3'];

        $scope.addItem = function () {
            var newItemNo = $scope.items.length + 1;
            $scope.items.push('Item ' + newItemNo);
        };

        $scope.status = {
            isFirstOpen: true,
            isFirstDisabled: false
        };

        // NVD3 test
        $scope.exampleData = [  [1025409600000, 0], [1028088000000, -6.3382185140371], [1030766400000, -5.9507873460847], [1033358400000, -11.569146943813], [1036040400000, -5.4767332317425], [1038632400000, 0.50794682203014],
                                [1041310800000, -5.5310285460542], [1043989200000, -5.7838296963382], [1046408400000, -7.3249341615649], [1049086800000, -6.7078630712489], [1051675200000, 0.44227126150934], [1054353600000, 7.2481659343222]];
        $scope.exampleData = [[1, 10], [2, 11], [3, 12], [4, 13], [5, 14], [6, 15],
                              [7, 16], [8, 20], [9, 22], [10, 22], [11, 43], [12, 54]];

        $scope.slData = [12398, 23221, 33421, 34251, 34921, 34252];

        $scope.xFunction = function () {
            return function (d) {
                return d[0];
            }
        }
        $scope.yFunction = function () {
            return function (d) {
                return d[1];
            }
        }
        var onSiteData = function (data) {
            $scope.siteDetailData = data.siteDetailData;
            $scope.divisions = data.divisions;
            //for (index = 0, len = data.siteDetailData.length; index < len; ++index)
            //{
            //    // Calculate the stats from passed data
            //};
        }

        var onRepo = function (data) {
            var index; var len;
            var tallyArray = [];
            var sumArray = [];
            console.log('Data read..' + data.invoiceTallies.length);
            var maxTotalInvoices = getMaxNoOfInvoices(data.invoiceTallies);
            var maxEnergyCharge = getMaxEnergyCharge(data.invoiceCosts);
            var maxKwh = getMaxKwh(data.invoiceCosts);
            var maxUnitsPerSqm = getUnitsPerSqm(data.invoiceCosts);
            var maxCostPerSqm = getMaxCostPerSqm(data.invoiceCosts);
            console.log(maxTotalInvoices);
            for (index = 0, len = data.invoiceTallies.length; index < len; ++index) {
                // maxEnergyCharge = maxEnergyCharge * data.invoiceTallies[index].calculatedLossRate;
                tallyArray.push({
                    "site": data.invoiceTallies[index].siteName,
                    "siteId": data.invoiceTallies[index].siteId,
                    "firstInvoiceDate": data.invoiceTallies[index].firstInvoiceDate,
                    "totalInvoicesOnFile": data.invoiceTallies[index].totalInvoicesOnFile,
                    "data": [
                        { "percent": (data.invoiceTallies[index].approvedInvoices / maxTotalInvoices * 100), "noOfInv": data.invoiceTallies[index].approvedInvoices },
                        { "percent": (data.invoiceTallies[index].pendingInvoices / maxTotalInvoices * 100), "noOfInv": data.invoiceTallies[index].pendingInvoices },
                        { "percent": (data.invoiceTallies[index].missingInvoices / maxTotalInvoices * 100), "noOfInv": data.invoiceTallies[index].missingInvoices }
                    ]
                });

                sumArray.push({
                    "site": data.invoiceTallies[index].siteName,
                    "siteId": data.invoiceTallies[index].siteId,
                    "invCount": data.invoiceTallies[index].totalInvoicesOnFile,
                    "billTotal": data.invoiceCosts[index].invoiceValue,
                    "power": [{
                        "reading": data.invoiceCosts[index].energyCharge, /// (1 + data.invoiceTallies[index].calculatedLossRate),
                        "percent": (data.invoiceCosts[index].energyCharge / (1 + data.invoiceTallies[index].calculatedLossRate)) / maxEnergyCharge * 100
                    },
                    {
                        "reading": (data.invoiceCosts[index].energyCharge / (1 + data.invoiceTallies[index].calculatedLossRate)) * data.invoiceTallies[index].calculatedLossRate,
                        "percent": ((data.invoiceCosts[index].energyCharge / (1 + data.invoiceTallies[index].calculatedLossRate)) * data.invoiceTallies[index].calculatedLossRate)
                                        / maxEnergyCharge * 100
                    }],
                    "kWh": [{ "reading": data.invoiceCosts[index].totalKwh, "percent": data.invoiceCosts[index].totalKwh / maxKwh * 100, "units": "kWh" }],
                    "upsqm": [{ "reading": data.invoiceCosts[index].unitsPerSqm, "percent": data.invoiceCosts[index].unitsPerSqm / maxUnitsPerSqm * 100, "units": "" }],
                    "cpsqm": [{ "reading": data.invoiceCosts[index].costPerSqm, "percent": data.invoiceCosts[index].costPerSqm / maxCostPerSqm * 100, "units": "$" }]
                });
                //  console.log("energy charge = " + data.invoiceCosts[index].energyCharge + " max charge = " + maxEnergyCharge + " loss rate = " + data.invoiceTallies[index].calculatedLossRate);
            };
            // console.log(sumArray);
            //console.log(tallyArray);
            $scope.invDistn = tallyArray;
            $scope.invoiceDetail = sumArray;
            $scope.monthSpan = monthSpan;
            $scope.groupCompanyDetail = data.groupCompanyDetail;
            $scope.customerList = data.customerList;
            //console.log($scope.customerList);
            console.log('change month span ... ' + monthSpan);

            $scope.loading = false;
        };

        var onUserData = function (data) {
            $scope.monthSpanOptions = data.monthSpanOptions;
            $scope.monthSpan = data.monthSpan;
            //$scope.companyId = data.companyId;
            monthSpan = data.monthSpan;
            var companyId = 0;

            soDataSource.getInvoiceTally(monthSpan, companyId)
                .then(onSiteData, onError);
                //.then(onRepo, onError);

            $scope.loading = false;
        };

        var getMaxKwh = function (list) {
            var max = 0;
            var index; var len;
            for (index = 0, len = list.length; index < len; ++index) {
                max = Math.max(max, list[index].totalKwh);
            }
            return max;
        };
        var getUnitsPerSqm = function (list) {
            var max = 0;
            var index; var len;
            for (index = 0, len = list.length; index < len; ++index) {
                max = Math.max(max, list[index].unitsPerSqm);
            }
            return max;
        };
        var getMaxCostPerSqm = function (list) {
            var max = 0;
            var index; var len;
            for (index = 0, len = list.length; index < len; ++index) {
                max = Math.max(max, list[index].costPerSqm);
            }
            return max;
        };

        var getMaxEnergyCharge = function (list) {
            var max = 0;
            var index; var len;
            for (index = 0, len = list.length; index < len; ++index) {
                max = Math.max(max, list[index].energyCharge);
                //console.log("energy charge : " + list[index].energyCharge);
            }
            return max;
        };

        var getMaxNoOfInvoices = function (list) {
            var max = 0;
            var index; var len;
            for (index = 0, len = list.length; index < len; ++index) {
                max = Math.max(max, list[index].totalInvoices);
            }
            return max;
        };

        var onError = function (reason) {
            $scope.reason = reason;
        };

        $scope.reviseMonths = function (newMonthSpan) {
            $scope.loading = true;
            console.log('revise data...' + newMonthSpan);
            var companyId = 0;
            monthSpan = newMonthSpan;
            soDataSource.getInvoiceTally(monthSpan, companyId)
                .then(onRepo, onError);
        };

        $scope.changeCompany = function (newCompanyId) {
            console.log('change company data...month span: ' + monthSpan + ' company Id: ' + newCompanyId);
            companyId = newCompanyId;
            soDataSource.getInvoiceTally(monthSpan, companyId)
                .then(onRepo, onError);
        };

        $scope.loading = true;
        userDataSource.getUserData()
            .then(onUserData, onError);

        $scope.tabTableHeader = 'Site Details';

        $scope.invTheme = [{ "theme": "success", "text": "Approved" }, { "theme": "info", "text": "To Approve" }, { "theme": "warning", "text": "Missing" }];


        $scope.order = function (predicate) {
            if ($scope.predicate != predicate) { $scope.sortArrow = "fa-long-arrow-down" };
            $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : true;
            $scope.predicate = predicate;
            if ($scope.reverse) {
                $scope.sortArrow = "fa-long-arrow-up";
            }
            else {
                $scope.sortArrow = "fa-long-arrow-down";
            }
        };

        //
        // Google chart sparkline test
        $scope.myChartObject = {};

        var _cols = [];
        var _rows = [];
        var _cpart = [];

        init();

        function init() {
            $scope.myChartObject.type = "LineChart";
            $scope.myChartObject.displayed = false;
            $scope.myChartObject.data = {
                "cols": [{
                    id: "month",
                    label: "Month",
                    type: "string"
                }, {
                    id: "laptop-id",
                    label: "Laptop",
                    type: "number"
                }],
                "rows": [{
                    c: [{
                        v: ""
                    }, {
                        v: 19,
                        f: ""
                    }]
                }, {
                    c: [{
                        v: ""
                    }, {
                        v: 13
                    }]

                }, {
                    c: [{
                        v: ""
                    }, {
                        v: 24
                    }]
                }]
            };
            $scope.myChartObject.options = {
                "width": 120, "height": 40, "showAxisLines": false,  "showValueLabels": false, "labelPosition": 'left'
            };

            $scope.myChartObject.view = {
                columns: [0, 1]
            };
        }
        //console.log($scope.invoiceDistribution);
        //console.log($scope.invDistn);

        $scope.visitChartData = [
    {
        //color: $rootScope.settings.color.themesecondary,
        label: "Direct Visits",
        data: [
            [3, 2], [4, 5], [5, 4], [6, 11], [7, 12], [8, 11], [9, 8], [10, 14], [11, 12], [12, 16], [13, 9],
            [14, 10], [15, 14], [16, 15], [17, 9]
        ],

        lines: {
            show: true,
            fill: true,
            lineWidth: .1,
            fillColor: {
                colors: [
                    {
                        opacity: 0
                    }, {
                        opacity: 0.4
                    }
                ]
            }
        },
        points: {
            show: false
        },
        shadowSize: 0
    },
    {
        //color: $rootScope.settings.color.themeprimary,
        label: "Referral Visits",
        data: [
            [3, 10], [4, 13], [5, 12], [6, 16], [7, 19], [8, 19], [9, 24], [10, 19], [11, 18], [12, 21], [13, 17],
            [14, 14], [15, 12], [16, 14], [17, 15]
        ],
        bars: {
            order: 1,
            show: true,
            borderWidth: 0,
            barWidth: 0.4,
            lineWidth: .5,
            fillColor: {
                colors: [
                    {
                        opacity: 0.4
                    }, {
                        opacity: 1
                    }
                ]
            }
        }
    },
    {
        //color: $rootScope.settings.color.themethirdcolor,
        label: "Search Engines",
        data: [
            [3, 18], [4, 11], [5, 10], [6, 9], [7, 5], [8, 8], [9, 5], [10, 6], [11, 4], [12, 7], [13, 4],
            [14, 3], [15, 4], [16, 6], [17, 4]
        ],
        lines: {
            show: true,
            fill: false,
            fillColor: {
                colors: [
                    {
                        opacity: 0.3
                    }, {
                        opacity: 0
                    }
                ]
            }
        },
        points: {
            show: true
        }
    }
        ];
        $scope.visitChartOptions = {
            legend: {
                show: false
            },
            xaxis: {
                tickDecimals: 0,
                color: '#f3f3f3'
            },
            yaxis: {
                min: 0,
                color: '#f3f3f3',
                tickFormatter: function (val, axis) {
                    return "";
                },
            },
            grid: {
                hoverable: true,
                clickable: false,
                borderWidth: 0,
                aboveData: false,
                color: '#fbfbfb'

            },
            tooltip: true,
            tooltipOpts: {
                defaultTheme: false,
                content: " <b>%x May</b> , <b>%s</b> : <span>%y</span>",
            }
        };

        //Selectable Chart
        $scope.SelectableChartData = [
            {
                //color: $rootScope.settings.color.themeprimary,
                label: "Windows",
                data: [[1990, 18.9], [1991, 18.7], [1992, 18.4], [1993, 19.3], [1994, 19.5], [1995, 19.3], [1996, 19.4], [1997, 20.2], [1998, 19.8], [1999, 19.9], [2000, 20.4], [2001, 20.1], [2002, 20.0], [2003, 19.8], [2004, 20.4]]
            }, {
                //color: $rootScope.settings.color.themethirdcolor,
                label: "Linux",
                data: [[1990, 10.0], [1991, 11.3], [1992, 9.9], [1993, 9.6], [1994, 9.5], [1995, 9.5], [1996, 9.9], [1997, 9.3], [1998, 9.2], [1999, 9.2], [2000, 9.5], [2001, 9.6], [2002, 9.3], [2003, 9.4], [2004, 9.79]]
            }, {
                //color: $rootScope.settings.color.themesecondary,
                label: "Mac OS",
                data: [[1990, 5.8], [1991, 6.0], [1992, 5.9], [1993, 5.5], [1994, 5.7], [1995, 5.3], [1996, 6.1], [1997, 5.4], [1998, 5.4], [1999, 5.1], [2000, 5.2], [2001, 5.4], [2002, 6.2], [2003, 5.9], [2004, 5.89]]
            }, {
                //color: $rootScope.settings.color.themefourthcolor,
                label: "DOS",
                data: [[1990, 8.3], [1991, 8.3], [1992, 7.8], [1993, 8.3], [1994, 8.4], [1995, 5.9], [1996, 6.4], [1997, 6.7], [1998, 6.9], [1999, 7.6], [2000, 7.4], [2001, 8.1], [2002, 12.5], [2003, 9.9], [2004, 19.0]]
            }
        ];

        $scope.SelectableChartOptions = {
            series: {
                lines: {
                    show: true
                },
                points: {
                    show: true
                }
            },
            legend: {
                noColumns: 4
            },
            xaxis: {
                tickDecimals: 0,
                color: '#eee'
            },
            yaxis: {
                min: 0,
                color: '#eee'
            },
            selection: {
                mode: "x"
            },
            grid: {
                hoverable: true,
                clickable: false,
                borderWidth: 0,
                aboveData: false
            },
            tooltip: true,
            tooltipOpts: {
                defaultTheme: false,
                content: "<b>%s</b> : <span>%x</span> : <span>%y</span>",
            },
            crosshair: {
                mode: "x"
            }
        };

    };

}());