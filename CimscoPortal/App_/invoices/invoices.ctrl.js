﻿(function () {
    'use strict';

    angular
        .module("app.invoices")
        .controller("app.invoices.ctrl", detailBySite)

    detailBySite.$inject = ['$scope', '$filter', 'inDataSource', 'userDataSource', 'filterData', 'toaster', 'dataFormatting'];
    function detailBySite($scope, $filter, inDataSource, userDataSource, filterData, toaster, dataFormatting) {

        var _triggerEventName = filterData.getEventName();
        var _invTypes = [];
        var _previousFilter = "__";
        var _previousSiteId = -1;
        var _previousMonthSpan = 0;
        var debugStatus_showMessages = false;
        var _userData = [];

        $scope.requests = [];

        // Invoice display control
        $scope.toggleAll = function () {
            var toggleStatus = $scope.selectItems.showAll;
            angular.forEach($scope.selectItems, function (value, key) { $scope.selectItems[key] = toggleStatus });
        };

        $scope.showInvoices = function (invType, siteId, currentSiteName) {
           // console.log("Inv type:" + invType);
            $scope.invType = invType;
            _previousSiteId = $scope.siteId;
            $scope.siteId = siteId;
            $scope.currentSiteName = currentSiteName;
            //console.log(filterData.createApiFilter([{ id: "A" }, { id: "B" }], []));
            $scope.$emit(_triggerEventName);
        };
        //  $scope.oneAtATime = true;  // Accordion 

        $scope.reviseMonths = function (newMonthSpan) {
            //console.log(newMonthSpan);
            _previousMonthSpan = $scope.monthSpan;
            $scope.monthSpan = newMonthSpan;
            $scope.$emit(_triggerEventName);

            updateUserData("monthSpan", newMonthSpan);

            // $scope.loading = false;
        };

        var updateUserData = function (setting, value) {
            _userData = userDataSource.updateUserData(_userData, setting, value);
        };

        $scope.$on(_triggerEventName, function (data) {
            if (debugStatus_showMessages) { toaster.pop('success', "Event triggered", ""); }
            manageDataRefresh();

            //   $scope.loading = false; // Remove when loading data again
        });

        var manageDataRefresh = function () {
            //var companyId = 0;
            // On filter change
            // If Site Id > 0 (i.e. single site), update the header, set SiteId back to -1
            // If Site Id == 0 update Invoice data also
            // If Site Id == -1 update header only (no selection)


            // dataType --> lookup index in hash table, return integer value and append to filter string
            //console.log("Data type (get data):" + $scope.invType);
            //console.log("Index for type = " + getInvTypeId($scope.invType));
            //console.log("Scope site ID = " + $scope.siteId + " previous site id = " + _previousSiteId);
            var filterData = constructFilterInfo(); //getReturnIds();
            //filterData += "_" + getInvTypeId($scope.invType);
            setCheckBoxes($scope.invType, ($scope.siteId == 0));
            //console.log("Filter = " + filterData.filter + " Previous - " + _previousFilter);
            //console.log("Div Cat change:" + filterData.sDivCatChg + " Inv Type change:" + filterData.sInvTypeChg + " month change:" + filterData.sMonthChg  + " site Id change:" + filterData.sSiteIdChg)
            //if (!filterData.sDivCatChg & !filterData.sInvTypeChg & _previousSiteId == $scope.siteId & _previousMonthSpan == $scope.monthSpan) {
            if (!filterData.sDivCatChg & !filterData.sMonthChg & !filterData.sSiteIdChg & !($scope.siteId == 0 & filterData.sInvTypeChg)) {
                if (debugStatus_showMessages) { toaster.pop('warning', "No change to filter", "Exit without data update"); }
                return;
            };

            // If invoice type ID changes then flags if single site, new request if all sites

            cancelAllRequests();
            //$scope.loading = true;
            //console.log("grpdiv select = " + filterData.sDivCat + " inv select = " + filterData.sInvType);
            if ($scope.siteId != 0) {
                // If SiteId == 0 then selecting all sites. This is handled in next section
                //          Possible settings : siteId (-1 == not selected)
                //                            : sDivCatChg == A change in divisin and/or category selection has been made
                //                            : sInvTypeChg == Change in invoice type made
                //                            : Change in number of months for statistics selection

                // If no site selected, simply update the current statistics
                if ($scope.siteId == -1) {
                    // No site selection made, can only update filters where there has been a change
                    if (debugStatus_showMessages) { toaster.pop('success', "A: No site selected - Update stats.", ""); }
                    readAndShowInvStats(filterData.filter);
                }

                else if (filterData.sDivCatChg | filterData.sMonthChg) {
                    // Refresh filters and clear displayed data
                    if (debugStatus_showMessages) { toaster.pop('success', "aa: Filter change - Update stats, reset data", ""); }
                    resetHeaderFilters();                    
                    resetOnScreenData();
                    readAndShowInvStats(filterData.filter);
                }

                //else if (filterData.sDivCatChg) {
                //    if (debugStatus_showMessages) { toaster.pop('success', "B: Catgory change - Update stats, reset data", ""); }
                //    resetHeaderFilters();
                //    readAndShowInvStats(filterData.filter);
                //    resetOnScreenData();
                //}
                else if ($scope.siteId > 0) {
                    // Selecting by individual site
                    // No selection on invoice types, so we are just updating the stats
                    if (_previousSiteId != $scope.siteId) {
                        if (debugStatus_showMessages) { toaster.pop('success', "C: Read data for selected site", "Set check boxes"); }
                        resetOnScreenData($scope.currentSiteName);
                        getBusinessData("site");
                        setCheckBoxes($scope.invType, false);
                    }
                    else if (filterData.sInvType) {
                        if (debugStatus_showMessages) { toaster.pop('success', "D: Invoice type change", "Set check boxes"); }
                        setCheckBoxes($scope.invType, false);
                    } else {
                        if (debugStatus_showMessages) { toaster.pop('success', "E: Update stats and reset filters", ""); }
                        readAndShowInvStats(filterData.filter);
                        if (filterData.sDivCat) { resetHeaderFilters(); resetOnScreenData(); }
                    }
                }
            }

            else {
                // Selecting all sites (siteId = 0)
                if (_previousSiteId != $scope.siteId | filterData.sInvTypeChg) {
                    if (debugStatus_showMessages) {
                        toaster.pop('success', "Site/Inv Type change : Read data for all sites", "");
                    }
                    resetOnScreenData($scope.currentSiteName);
                    getBusinessData('all', filterData.filter);
                }
                else if (filterData.sDivCatChg) {
                    if (debugStatus_showMessages) { toaster.pop('success', "Cat/Div change : Clear data and update stats", "Re-read business data"); }
                    resetOnScreenData($scope.currentSiteName);
                    readAndShowInvStats(filterData.filter);
                    getBusinessData('all', filterData.filter);
                }
                else if (filterData.sMonthChg) {
                    if (debugStatus_showMessages) { toaster.pop('success', "Month change : Clear data and update stats", "Re-read business data"); }
                    resetOnScreenData($scope.currentSiteName);
                    readAndShowInvStats(filterData.filter);
                    getBusinessData('all', filterData.filter);
                }
                //if (debugStatus_showMessages) { toaster.pop('success', "Read data for all sites", "Filter applied"); }
                //getBusinessData('all', filterData.filter);
            }

            _previousFilter = filterData.filter;
            _previousSiteId = $scope.siteId;
            _previousMonthSpan = $scope.monthSpan;
        };

        var getBusinessData = function (scope, filter) {
            if (scope == 'site') {
                // Filter is not required - siteId is subject to Group/Division/Industry filter. All invoices are returned, to be filtered on the live page
                $scope.loading = true;
                var dataRequest = inDataSource.getSiteInvoiceData($scope.siteId, $scope.monthSpan);
            }
            else if (scope == 'all') {
                $scope.loading = true;
                var dataRequest = inDataSource.getAllInvoiceData($scope.monthSpan, filter, 1);
            }
            $scope.requests.push(dataRequest);
            dataRequest.promise.then(onInvoiceData, onError);
        };

        var cancelAllRequests = function () {
            if (debugStatus_showMessages) { toaster.pop('warning', "All requests cancelled", ""); }
            for (i = 0; i < $scope.requests.length; i++) {
                $scope.requests[i].cancel("User Cancel");
            }
            $scope.requests = [];
        };

        var cancelRequest = function (request) {
            toaster.pop('warning', "Current request cancelled", "");
            request.cancel("User cancelled");
            clearRequest(request);
        };

        var clearRequest = function (request) {
            $scope.requests.splice($scope.requests.indexOf(request), 1);
        };


        var onInvoiceData = function (data) {
            //console.log(data);
            // $scope.monthSpan = monthSpan;
            $scope.invoiceData = data;
            $scope.loading = false;
            if (debugStatus_showMessages) { toaster.pop('success', "Invoice Data Loaded!", ""); }
            clearRequest($scope.requests[0]); // Remove request from list, as now complete
        };

        //// TEMP
        //var readAndShowInvStats = function () {
        //    toaster.pop('success', "Refresh Test Stats Data...", "");
        //    $scope.loading = true;

        //    $scope.siteSummaryData = [{ siteId: 5, siteName: "Site 1", invApproval: 10 }, { siteId: 8, siteName: "Site 2", invApproval: 30 }, { siteId: 6, siteName: "Site 3", invApproval: 30 }];
        //    $scope.filedPercent = 23;
        //    $scope.filedOptions = {
        //        animate: {
        //            duration: 5,
        //            enabled: true
        //        },
        //        barColor: '#2C3E50',
        //        scaleColor: false,
        //        lineWidth: 5,
        //        lineCap: 'circle',
        //        size: 60
        //    }
        //    $scope.totalSites = 56;
        //    $scope.approvalPercent = 45;
        //    $scope.approvalTotal = 65;
        //    $scope.approvalOptions = {
        //        animate: {
        //            duration: 5,
        //            enabled: true
        //        },
        //        barColor: '#2C3E50',
        //        scaleColor: false,
        //        lineWidth: 5,
        //        lineCap: 'circle',
        //        size: 60
        //    }
        //    $scope.loading = false;
        //};

        var readAndShowInvStats = function (filter) {
            if (debugStatus_showMessages) { toaster.pop('success', "Refresh Stats Data from DB", ""); }
            $scope.loadingStats = true;
            var dataRequest = inDataSource.getInvoiceStatsBySite($scope.monthSpan, filter);
            $scope.requests.push(dataRequest);

            dataRequest.promise.then(onSiteStatsData, onError);
        };

        var onSiteStatsData = function (data) {

            // GPA ** refactoring required
            var _summary = [
                { sites: 0, entries: 0, percent: 0, filter: "pending", header: "to be approved" },
                { sites: 0, entries: 0, percent: 0, filter: "missing", header: "missing" },
                { sites: 0, entries: 0, percent: 0, filter: "approved", header: "approved" }
            ];

            angular.forEach(data, function (entry) {
                if (entry.pending> 0) { _summary[0].sites++; }
                if (entry.missing> 0) { _summary[1].sites++; }
                if (entry.approved> 0) { _summary[2].sites++; }
                _summary[0].entries = _summary[0].entries + entry.pending;
                _summary[1].entries = _summary[1].entries + entry.missing;
                _summary[2].entries = _summary[2].entries + entry.approved;
            });
            var _totalEntries = 0;
            angular.forEach(_summary, function (entry) {
                _totalEntries = _totalEntries + entry.entries;
            });
            angular.forEach(_summary, function (entry) {
                entry.percent = (entry.entries / _totalEntries) * 100;
            });

            //console.log(_summary);
            $scope.invoiceStatsBySite = data;
            $scope.invoiceStatsBySite.summary = _summary;
            //console.log(data);
            $scope.loading = false;
            $scope.loadingStats = false;
        };

        var getReturnIds = function () {
            return filterData.createApiFilter($scope.categoriesModel, $scope.divisionsModel);
        };

        var onUserData = function (data) {
            $scope.loading = true;
            _userData = data;
            userDataSource.assignUserData($scope, _userData);

            //$scope.monthSpanOptions = data.monthSpanOptions;
            //$scope.monthSpan = data.monthSpan;
            _previousMonthSpan = $scope.monthSpan;
            if (debugStatus_showMessages) { toaster.pop('success', "User data read", ""); }
            //filterData.getAllFilters()
            //    .then(onFiltersOk, onError);
            //getBusinessData();
            //console.log($scope.selectItems);
            setInitialScope();

        };

        var updateUserSettings = function (setting, value) {
            _userData = userDataSource.updateUserSettings(_userData, setting, value);
        };

        var setInitialScope = function () {
            $scope.epOptions = {
                animate: {
                    duration: 5,
                    enabled: true
                },
                barColor: '#2C3E50',
                scaleColor: false,
                lineWidth: 5,
                lineCap: 'circle',
                size: 60
            };
            $scope.selectItems = {
                showAll: true,
                showApproved: true,
                showToApprove: true,
                showMissing: true
            };
            resetHeaderFilters();
            _previousFilter = constructFilterInfo().filter;
            //console.log("Previous filter set " + _previousFilter);
            //readAndShowInvStats();
            readAndShowInvStats(_previousFilter);
        };

        var resetHeaderFilters = function () {
            $scope.invType = " ";
            $scope.siteId = -1;
        };

        var resetOnScreenData = function (siteName) {
            $scope.invoiceData = [];
            $scope.currentSiteName = siteName;
        };

        var constructFilterInfo = function () {
            var _invTypeId = getInvTypeId($scope.invType);
            //console.log("Scope type: " + $scope.invType + " code: " + _invTypeId);
            var _filter = getReturnIds();
            //console.log("ZTEST " + _previousFilter.split("_")[1]  + _previousFilter.split("_")[2] + " _filter = " + _filter.split("_")[1]  + _filter.split("_")[2]);
            //console.log("_filter = " + _filter + "_" + _invTypeId + " previous = " + _previousFilter);
            var _filters = {
                sInvType: (_invTypeId != "0")  // Whether there is a selection on Invoice Types (approved/not approved/etc)
                , sDivCat: (_filter != "__")   // Whether there is a selection on Division / CAtegory types
                , filter: _filter + "_" + _invTypeId // Current filter that will be passed back to server for data query
                , sInvTypeChg: (_previousFilter.split("_")[3] != _invTypeId) // Whether there has been a change in the selection of Invoice Type (eg Approved --> To be approved)
                , sDivCatChg: (_previousFilter.split("_")[1] + _previousFilter.split("_")[2] != _filter.split("_")[1] + _filter.split("_")[2]) // Whether the Category Division filter has changed
                , sMonthChg: (_previousMonthSpan != $scope.monthSpan)
                , sSiteIdChg: (_previousSiteId != $scope.siteId)
            };

            //console.log(_filters);
            //console.log("Inv change " + _filters.sInvTypeChg);
            //console.log(_filter);

            //_filter = _filter + "_" + _invTypeId;
            return _filters;
        };

        var setCheckBoxes = function (category, disable) {
            switch (category) {
                case 'pending':
                    $scope.selectItems = {
                        showAll: false,
                        showApproved: false,
                        showToApprove: true,
                        showMissing: false
                    };
                    break;
                case 'approved':
                    $scope.selectItems = {
                        showAll: false,
                        showApproved: true,
                        showToApprove: false,
                        showMissing: false
                    };
                    break;
                case 'missing':
                    $scope.selectItems = {
                        showAll: false,
                        showApproved: false,
                        showToApprove: false,
                        showMissing: true
                    };
                    break;
            };
            if (disable) { $scope.disableCheckboxes = true; }
            else { $scope.disableCheckboxes = false; }
        };

        var onFiltersOk = function (data) {
            filterData.createMultiDropdown('divisions', data.divisions, true, $scope);
            filterData.createMultiDropdown('categories', data.categories, true, $scope);
            _invTypes = data.invTypes
            //console.log(getInvTypeId("toapprove"));
        };

        var getInvTypeId = function (invType) {
            //     console.log("look for " + invType);
            //    console.log("in " + _invTypes);
            var _entry = $filter("filter")(_invTypes, { label: invType });
            //console.log(_entry.length);
            if (_entry == null) {
                return 0;
            } else {
                return _entry.length > 0 ? _entry[0].id : 0;
            }
        };

        var onError = function (reason) {
            // Status 0 == cancel read due to new request
            if (reason.status != 0) {
                toaster.pop('error', "Unable to read data", "Reason: " + reason.statusText + " (status:" + reason.status + ")");
            }
            $scope.loading = false;
            $scope.loadingStats = false;
        };

        // GPA ** --> Also in SiteOverview. Re-factor to common factory
        $scope.approveInv = function (setting, invoiceId) {
            inDataSource.postInvoiceApproval(invoiceId)
                .then(function success(data) { return invoiceApproved(data, invoiceId) }, onError);
        };
        var invoiceApproved = function (result, invoiceId) {
            //  console.log("Approved ok for invoiceId : " + invoiceId);
            var foundIndex = $filter('filter')($scope.invoiceData, { invoiceId: invoiceId }, true)[0].$id - 1;

            $scope.invoiceData[foundIndex].approversName = result.data.approversName;
            $scope.invoiceData[foundIndex].approvedDate = result.data.approvedDate;
            $scope.invoiceData[foundIndex].approved = result.data.approved;

            toaster.pop('info', "Approved!", "Notification email has been sent");
        };

        //$scope.selectItems = {
        //    showAll: true,
        //    showApproved: true,
        //    showToApprove: true,
        //    showMissing: true
        //};

        // Load data chain start
        //   $scope.loading = true;
        userDataSource.getUserData()
            .then(onUserData, onError);
        filterData.getAllFilters()
            .then(onFiltersOk, onError);

        //  $scope.tabTableHeader = 'Site Details';

        $scope.invTheme = [{ "theme": "success", "text": "Approved" }, { "theme": "info", "text": "To Approve" }, { "theme": "warning", "text": "Missing" }];


        // Manage data revision
        //$scope.reviseMonths = function (newMonthSpan) {
        //    $scope.loading = true;
        //    console.log('revise data...' + newMonthSpan);
        //    monthSpan = newMonthSpan;
        //    getInvoiceData();
        //};

        //$scope.changeCompany = function (newCompanyId) {
        //    console.log('change company data...month span: ' + monthSpan + ' company Id: ' + newCompanyId);
        //    companyId = newCompanyId;
        //    inDataSource.getInvoiceTally(monthSpan, companyId)
        //        .then(onRepo, onError);
        //};

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
        $scope.greaterThanZero = function (prop) {
            //console.log(prop);

            return function (item) {
                return item[prop] > 0;
            }
        };

        $scope.pctBoxStyle = dataFormatting.pctBoxStyle;
        $scope.negativeValue = dataFormatting.negativeValue;


        $scope.getCountByFilter = function (field, data) {
            // GPA*** refactor!!
            var _max = 0;
            if (field == 'pending') {
                return data.pending
            };
            if (field == 'missing') { return data.missing};
            if (field == 'approved') { return data.approved};
            return 0;
        };

        $scope.getPercentByFilter = function (field, data) {
            // GPA*** refactor!!
            var _max = 0;
            if (field == 'pending') {
                return data.pendingByPercent
            };
            if (field == 'missing') { return data.missingByPercent };
            if (field == 'approved') { return data.approvedByPercent };
            return 0;
        };
        $scope.getMax = function (a, b) {
            if (a > b) { return a }
            else { return b };
        };
    };

}());