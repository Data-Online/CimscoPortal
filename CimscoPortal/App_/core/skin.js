(function () {
    angular.module("app.core")
        .service('getSkin', function () {
            this.cssFile = 'Content/css/skins/deepblue.min.css',
            this.color = {
                themeprimary: '#001940',
                themesecondary: '#d73d32',
                themethirdcolor: '#ffce55',
                themefourthcolor: '#a0d468',
                themefifthcolor: '#e75b8d'
            }
        });
}());