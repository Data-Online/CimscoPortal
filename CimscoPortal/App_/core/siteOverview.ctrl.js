(function () {
    var monthSpan = 12;
    var module = angular.module("app");

    var siteOverview = function ($scope, $timeout, $filter, soDataSource, sharedProperties) {

        var siteId = 0;//sharedProperties.getSiteId();
        //console.log("Site id = " + siteId);
        $scope.monthSpanOptions = [3, 6, 12, 24];

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

        var onRepo = function (data) {
            console.log('Get data call');
            $scope.invoiceData = data;
        };

        var onError = function (reason) {
            console.log('An error occured : ' + reason.status);
            $scope.reason = "There was a problem (code:" + reason.status + ")";
        };

        var invoiceApproved = function (result, invoiceId) {
            console.log("Approved ok for invoiceId : " + invoiceId);
            var foundIndex = $filter('filter')($scope.invoiceData, { invoiceId: invoiceId }, true)[0].$id - 1;

            $scope.invoiceData[foundIndex].approversName = result.data.approversName;
            $scope.invoiceData[foundIndex].approvedDate = result.data.approvedDate;
            $scope.invoiceData[foundIndex].approved = result.data.approved;
        };

        $scope.reviseMonths = function (newMonthSpan) {
            console.log('revise data...' + newMonthSpan);
            monthSpan = newMonthSpan;
            coDataSource.getInvoiceTally(monthSpan)
                .then(onRepo, onError);
        };

        $scope.setSiteId = function (_siteId) {
            console.log("Set site Id to" + _siteId);
            siteId = _siteId;
            //sharedProperties.setSiteId(_siteId);
        };
        //soDataSource.getInvoiceData(siteId, $scope.invoiceData[index].invoiceId)
        //       .then(function success(data) { return updateInvoice(data, index) }, onError);

        var _delayedCall = function () {
            soDataSource.getSiteInvoiceData(siteId)
                .then(onRepo, onError)
        };

        $timeout(_delayedCall, 500);

        $scope.tabTableHeader = 'Invoice Details';

        $scope.invTheme = [{ "theme": "success", "text": "Approved" }, { "theme": "info", "text": "To Approve" }, { "theme": "warning", "text": "Missing" }];

        $scope.approveInv = function (setting, invoiceId) {
            soDataSource.postInvoiceApproval(invoiceId)
                .then(function success(data) { return invoiceApproved(data, invoiceId) }, onError);
        };

        $scope.removeInvoiceByIndex = function (index) {
            console.log('Removing item index = ' + index);
            $scope.invoiceData.splice(index, 1);
        };

        $scope.pctBoxStyle = function (myValue) {
            var num = parseInt(myValue);
            var style = 'databox-stat radius-bordered';
            //console.log(num);
            if (num <= -999) {
                style = style + ' bg-ivory';
            }
            else if (num > 0) {
                style = style + ' bg-warning';
            }
            else if (num < 0) {
                style = style + ' bg-green';
            }
            else {
                style = style + ' bg-sky';
            }
            return style;
        };

        $scope.negativeValue = function (myValue) {
            var num = parseInt(myValue);
            var style = 'stat-icon';
            if (num == -999) {

            }
            else if (num == 0) {
                style = style + ' fa fa-arrows-h';
            }
            else if (num > 0) {
                style = style + ' fa fa-long-arrow-up';
            }
            else if (num < 0) {
                style = style + ' fa fa-long-arrow-down';
            }
            // console.log('Style for value ' + num +' set to ' + style);
            return style;
        };


        var data = [
        ["1", 10],
        ["2", 20], ];
        var data2 = [
            ["1", 5],
            ["2", 10],
        ];

        $scope.visitChartData = [
           {
               label: "data 1",
               data: data,
               bars: { show: true,
                   barWidth: 0.15,
                   align: "center",
                   order: 1
               }

           }, {
               label: "data 2",
               data: data2,
               bars: { show: true,
                   barWidth: 0.15,
                   align: "center",
                   order: 1
               }

           }
        ];

        $scope.visitChartOptions = {
            series: {
                bars: {
                    show: true,
                    barWidth: 0.15,
                    align: "center"
                    //,
                    //order: 10
                }
            },
            xaxis: {
                mode: "categories",
                ticks: [
                    [0, "Jan"],
                    [1, "Feb"],
                ],
                tickLength: 1,

            },
            grid: {
                hoverable: true,
            },
            yAxis: {
                allowDecimals: false,
            }
        };


    ////    $scope.visitChartData = [
    ////{
    ////    //color: $rootScope.settings.color.themesecondary,
    ////    label: "Direct Visits",
    ////    data: [
    ////        [3, 2], [4, 5], [5, 4], [6, 11], [7, 12], [8, 11], [9, 8], [10, 14], [11, 12], [12, 16], [13, 9],
    ////        [14, 10], [15, 14], [16, 15], [17, 9]
    ////    ],

    ////    lines: {
    ////        show: true,
    ////        fill: true,
    ////        lineWidth: .1,
    ////        fillColor: {
    ////            colors: [
    ////                {
    ////                    opacity: 0
    ////                }, {
    ////                    opacity: 0.4
    ////                }
    ////            ]
    ////        }
    ////    },
    ////    points: {
    ////        show: false
    ////    },
    ////    shadowSize: 0
    ////},
    ////{
    ////    //color: $rootScope.settings.color.themesecondary,
    ////    label: "Direct Visits",
    ////    data: [
    ////        [3, 2], [4, 5], [5, 4], [6, 11], [7, 12], [8, 11], [9, 8], [10, 14], [11, 12], [12, 16], [13, 9],
    ////        [14, 10], [15, 14], [16, 15], [17, 9]
    ////    ],

    ////    lines: {
    ////        show: true,
    ////        fill: true,
    ////        lineWidth: .1,
    ////        fillColor: {
    ////            colors: [
    ////                {
    ////                    opacity: 0
    ////                }, {
    ////                    opacity: 0.4
    ////                }
    ////            ]
    ////        }
    ////    },
    ////    points: {
    ////        show: false
    ////    },
    ////    shadowSize: 0
    ////},
    ////{
    ////    //color: $rootScope.settings.color.themeprimary,
    ////    label: "Referral Visits",
    ////    data: [
    ////        [3, 10], [4, 13], [5, 12], [6, 16], [7, 19], [8, 19], [9, 24], [10, 19], [11, 18], [12, 21], [13, 17],
    ////        [14, 14], [15, 12], [16, 14], [17, 15]
    ////    ],
    ////    bars: {
    ////        order: 1,
    ////        show: true,
    ////        borderWidth: 0,
    ////        barWidth: 0.4,
    ////        lineWidth: .5,
    ////        fillColor: {
    ////            colors: [
    ////                {
    ////                    opacity: 0.4
    ////                }, {
    ////                    opacity: 1
    ////                }
    ////            ]
    ////        }
    ////    }
    ////},
    ////{
    ////    //color: $rootScope.settings.color.themethirdcolor,
    ////    label: "Search Engines",
    ////    data: [
    ////        [3, 18], [4, 11], [5, 10], [6, 9], [7, 5], [8, 8], [9, 5], [10, 6], [11, 4], [12, 7], [13, 4],
    ////        [14, 3], [15, 4], [16, 6], [17, 4]
    ////    ],
    ////    lines: {
    ////        show: true,
    ////        fill: false,
    ////        fillColor: {
    ////            colors: [
    ////                {
    ////                    opacity: 0.3
    ////                }, {
    ////                    opacity: 0
    ////                }
    ////            ]
    ////        }
    ////    },
    ////    points: {
    ////        show: true
    ////    }
    ////}
    ////    ];
    ////    $scope.visitChartOptions = {
    ////        legend: {
    ////            show: false
    ////        },
    ////        xaxis: {
    ////            tickDecimals: 0,
    ////            color: '#f3f3f3'
    ////        },
    ////        yaxis: {
    ////            min: 0,
    ////            color: '#f3f3f3',
    ////            tickFormatter: function (val, axis) {
    ////                return "";
    ////            },
    ////        },
    ////        grid: {
    ////            hoverable: true,
    ////            clickable: false,
    ////            borderWidth: 0,
    ////            aboveData: false,
    ////            color: '#fbfbfb'

    ////        },
    ////        tooltip: true,
    ////        tooltipOpts: {
    ////            defaultTheme: false,
    ////            content: " <b>%x May</b> , <b>%s</b> : <span>%y</span>",
    ////        }
    ////    };

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

    module.controller("app.core.siteOverview", siteOverview);

}());