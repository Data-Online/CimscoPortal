﻿var module = angular.module('widget', []);
//Maximize Widget
module
    .directive('widgetMaximize', function () {
        return {
            restrict: 'A',
            template: '<i class="fa fa-expand"></i>',
            link: function (scope, el, attr) {
                el.on('click', function () {
                    var widget = el.parents(".widget").eq(0);
                    var button = el.find("i").eq(0);
                    var compress = "fa-compress";
                    var expand = "fa-expand";
                    if (widget.hasClass("maximized")) {
                        if (button) {
                            button.addClass(expand).removeClass(compress);
                        }
                        widget.removeClass("maximized");
                        widget.find(".widget-body").css("height", "auto");
                    } else {
                        if (button) {
                            button.addClass(compress).removeClass(expand);
                        }
                        widget.addClass("maximized");
                        if (widget) {
                            var windowHeight = $(window).height();
                            var headerHeight = widget.find(".widget-header").height();
                            widget.find(".widget-body").height(windowHeight - headerHeight);
                        }
                    }
                });
            }
        };
    });

//Collapse Widget
module
    .directive('widgetCollapse', function () {
        return {
            restrict: 'A',
            template: '<i class="fa fa-minus"></i>',
            link: function (scope, el, attr) {
                el.on('click', function () {
                    var widget = el.parents(".widget").eq(0);
                    var body = widget.find(".widget-body");
                    var button = el.find("i");
                    var down = "fa-plus";
                    var up = "fa-minus";
                    var slidedowninterval = 300;
                    var slideupinterval = 200;
                    if (widget.hasClass("collapsed")) {
                        if (button) {
                            button.addClass(up).removeClass(down);
                        }
                        widget.removeClass("collapsed");
                        body.slideUp(0, function () {
                            body.slideDown(slidedowninterval);
                        });
                    } else {
                        if (button) {
                            button.addClass(down)
                                .removeClass(up);
                        }
                        body.slideUp(slideupinterval, function () {
                            widget.addClass("collapsed");
                        });
                    }
                });
            }
        };
    });

//Expand Widget
module
    .directive('widgetExpand', function () {
        return {
            restrict: 'A',
            template: '<i class="fa fa-plus"></i>',
            link: function (scope, el, attr) {
                el.on('click', function () {
                    var widget = el.parents(".widget").eq(0);
                    var body = widget.find(".widget-body");
                    var button = el.find("i");
                    var down = "fa-plus";
                    var up = "fa-minus";
                    var slidedowninterval = 300;
                    var slideupinterval = 200;
                    if (widget.hasClass("collapsed")) {
                        if (button) {
                            button.addClass(up).removeClass(down);
                        }
                        widget.removeClass("collapsed");
                        body.slideUp(0, function () {
                            body.slideDown(slidedowninterval);
                        });
                    } else {
                        if (button) {
                            button.addClass(down)
                                .removeClass(up);
                        }
                        body.slideUp(slideupinterval, function () {
                            widget.addClass("collapsed");
                        });
                    }
                });
            }
        };
    });

//Dispose Widget
module
    .directive('widgetDispose', function () {
        return {
            restrict: 'A',
            template: '<i class="fa fa-times"></i>',
            link: function (scope, el, attr) {
                el.on('click', function () {
                    var widget = el.parents(".widget").eq(0);
                    var disposeinterval = 300;
                    widget.hide(disposeinterval, function () {
                        widget.remove();
                    });
                });
            }
        };
    });

//Config Widget
module
    .directive('widgetConfig', function () {
        return {
            restrict: 'A',
            template: '<i class="fa fa-cog"></i>',
            link: function (scope, el, attr) {
                el.on('click', function () {
                    //Do what you intend for configishared widgets
                });
            }
        };
    });

//Config Widget
module
    .directive('widgetRefresh', function () {
        return {
            restrict: 'A',
            template: '<i class="fa fa-undo"></i>',
            link: function (scope, el, attr) {
                el.on('click', function () {
                    //Refresh widget content
                });
            }
        };
    });

module
    .filter('padNumber', function () {
        return function (n, len) {
            var num = parseInt(n, 10);
            len = parseInt(len, 10);
            if (isNaN(num) || isNaN(len)) {
                return n;
            }
            num = '' + num;
            while (num.length < len) {
                num = '0' + num;
            }
            return num;
        };
    });