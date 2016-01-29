(function () {
    'use strict';

    angular.module('CustomView', []) //no dependencies
        .controller("MainController", MainController)

    function MainController()
    {
        this.model = GetModel();
        if (this.model.autoRedirect && this.model.redirectUrl) {
            if (this.model.autoRedirectDelay < 0) {
                this.model.autoRedirectDelay = 0;
            }
            window.setTimeout(function () {
                window.location = this.model.redirectUrl;
            }, this.model.autoRedirectDelay * 1000);
        }

        function GetModel() {
            var modelJson = document.getElementById("modelJson");
            var encodedJson = '';
            if (typeof (modelJson.textContent) !== undefined) {
                encodedJson = modelJson.textContent;
            } else {
                encodedJson = modelJson.innerHTML;
            }
            var json = Encoder.htmlDecode(encodedJson);
            var model = JSON.parse(json);
            return model;
        }
    }
})();