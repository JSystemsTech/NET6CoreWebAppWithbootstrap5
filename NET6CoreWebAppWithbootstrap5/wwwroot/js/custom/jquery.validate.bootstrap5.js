/*!
 * jQuery Validation Plugin Overrides for Bootstrap 5 v1.0.0
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
        var defaultOptions = {
        errorClass: 'is-invalid',
        validClass: 'is-valid',
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(errorClass)
                .removeClass(validClass);
        },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass(errorClass);
                if ($(element).val() !== '' && $(element).prop('tagName') !== 'SELECT' && $(element).attr('type') !== 'hidden') {
                    $(element).addClass(validClass);
                }
        }
    };

    $.validator.setDefaults(defaultOptions);

    $.validator.unobtrusive.options = {
        errorClass: defaultOptions.errorClass,
        validClass: defaultOptions.validClass,
    };

}));
