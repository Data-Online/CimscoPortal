// Requires jQuery from http://jquery.com/
// and jQuerySparklines from http://omnipotent.net/jquery.sparkline

// AngularJS directives for jquery sparkline
angular.module('sparkline', []);

angular.module('sparkline')
    .directive('jqSparkline', ['$filter', function ($filter) {
        'use strict';
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elem, attrs, ngModel) {
                //console.log('sparkline');
                var opts = {};
                //TODO: Use $eval to get the object
                opts.type = attrs.type || 'line';

                scope.$watch(attrs.ngModel, function () {
                    render();
                });

                scope.$watch(attrs.opts, function () {
                    render();
                }
                  );
                var render = function () {
                    var model;
                    if (attrs.opts) angular.extend(opts, angular.fromJson(attrs.opts));
                    //console.log(opts);
                    // Trim trailing comma if we are a string
                    angular.isString(ngModel.$viewValue) ? model = ngModel.$viewValue.replace(/(^,)|(,$)/g, "") : model = ngModel.$viewValue;
                    var data;
                    // Make sure we have an array of numbers
                    angular.isArray(model) ? data = model : data = model.split(',');

                    var out = [];
                    for (var i = 0; i < data.length; i++) {
                        out.push($filter('number')(data[i],2).replace(/,/g,""))
                    }

                    //console.log(opts);
                    $(elem).sparkline(out, opts);
                };
            }
        }
    }]);