/*!
 * checklist.unobtrusive v1.0.0
 *
 * Copyright (c) 2021 Jonathan McGuire
 * Released under the MIT license
 */

(function (factory) {
    if (typeof define === "function" && define.amd) {
        define(["jquery"], factory);
    } else if (typeof module === "object" && module.exports) {
        module.exports = factory(require("jquery"));
    } else {
        factory(jQuery);
    }
}(function ($) {   
    var checklist = function (options) {
        var list = this;
        var mask = list.find('[data-toggle="mask"]'); 
        options = $.extend(true, {
            url: null,
            delay: 500
        }, options);
        options = $.extend(true, options, list.data());
        
        if (options !== null) {
            var timer;
            list.on('change', function () {
                if (timer) {
                    clearTimeout(timer);
                }
                timer = setTimeout(function () {
                    mask.bsShow();
                    $.post(options.url, list.serializeArray(), function () {
                        mask.bsHide();
                    }).fail(function (e) {
                        mask.bsHide();
                    });
                }, options.delay);
            });
        }
        
    };
    $.fn.checklist = checklist;
    $('[data-bs-toggle="checklist"]').each(function (i, el) { $(el).checklist(); });
    $.RegisterOnLoadFunction(function (el) {
        el.find('[data-bs-toggle="checklist"]').each(function (i, clEl) { $(clEl).checklist(); });
    });

}));


