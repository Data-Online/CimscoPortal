(function () {
    var filters = angular.module('customFilters', []);

    filters.filter('invType', function () {
        return function (inv, selectItems) {
            var items = {
                selectItems: selectItems,
                out: []
            };

            //angular.forEach(inv, function (value, key) {
            //    if (!(!this.selectItems.showApproved & !this.selectItems.showUnapproved & !this.selectItems.showVerified)) {
            //        if (   ( value.approved == this.selectItems.showApproved
            //            |   !value.approved == this.selectItems.showUnapproved )
            //            |   value.verified == this.selectItems.showVerified) {
            //            this.out.push(value);
            //        }
            //    }
            //}, items);

            angular.forEach(inv, function (value, key) {
                if (this.selectItems.showAll) {
                    this.out.push(value);
                }
                else {
                    if (  (this.selectItems.showApproved & value.approved == this.selectItems.showApproved )
                       // | ( !this.selectItems.showApproved & !value.approved )
                        | (this.selectItems.showMissing & value.missing)
                        | (this.selectItems.showToApprove & value.approved == !this.selectItems.showToApprove & (!value.missing))
                        )
                    {
                        this.out.push(value);
                    }
                }
            }, items);

            return items.out;
        }
    });

    filters.filter('cut', function () {
        return function (value, wordwise, max, tail) {
            if (!value) return '';

            max = parseInt(max, 10);
            if (!max) return value;
            if (value.length <= max) return value;

            value = value.substr(0, max);
            if (wordwise) {
                var lastspace = value.lastIndexOf(' ');
                if (lastspace != -1) {
                    //Also remove . and , so its gives a cleaner result.
                    if (value.charAt(lastspace - 1) == '.' || value.charAt(lastspace - 1) == ',') {
                        lastspace = lastspace - 1;
                    }
                    value = value.substr(0, lastspace);
                }
            }

            return value + (tail || ' …');
        };
    });

    filters.filter('percentage', function () {
        return function (input, max) {
            if (isNaN(input)) {
                return input;
            }
            return Math.floor((input * 100) / max) + '%';
        };
    });
    //filters.filter('invByType', function () {
    //    return function (inv, selectedType) {
    //        var items = {
    //            selectedType: selectedType,
    //            out: [],
    //            limitTo: 10
    //        };
    //        var count = 0;
    //        angular.forEach(inv, function (value, key) {
                
    //            if (value[items.selectedType] > 0 & count < items.limitTo) {
    //                this.out.push(value);
    //                count++;
    //            }
    //        }, items);
    //        return items.out;
    //    }
    //});
}());
