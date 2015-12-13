(function () {
    var filters = angular.module('customFilters', []);

    filters.filter('invType', function () {
        return function (inv, selectItems) {
            var items = {
                selectItems: selectItems,
                out: []
            };

            angular.forEach(inv, function (value, key) {
                if (!(!this.selectItems.showApproved & !this.selectItems.showUnapproved & !this.selectItems.showVerified)) {
                    if (   ( value.approved == this.selectItems.showApproved
                        |   !value.approved == this.selectItems.showUnapproved )
                        |   value.verified == this.selectItems.showVerified) {
                        this.out.push(value);
                    }
                }
            }, items);
            return items.out;
        }
    });
}());
