/************************************************************
* Posts assincronos                                         *
* By: Caio Humberto Francisco                               *
* Date: 18/02/2013 14:34                                    *
*************************************************************/
jQuery.extend({
    ajaxPartialView: function (options) {

        var sDefaults = {
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            cache: false,
            traditional: true
        }

        var options = jQuery.extend(sDefaults, options);

        return $.ajax(options);
    },

    ajaxJson: function (options) {
        var sDefaults = {
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            cache: false,
            dataType: "json"
        }

        var options = jQuery.extend(sDefaults, options);

        return $.ajax(options);
    }
});