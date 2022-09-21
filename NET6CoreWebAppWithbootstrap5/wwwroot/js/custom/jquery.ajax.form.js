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
        var showErrors = function (errorMessage, errormap, errorlist) {
        var val = this;
            errormap.forEach(function (error, index) {
                var form = $(error.element).closest('form');
                var validationMessageEl = form.find('[data-valmsg-for="' + $(error.element).attr('id') + '"]');
                var focusedOnFirstError = false;
            if (error.message === null) {
                val.settings.unhighlight.call(val, error.element, val.settings.errorClass, val.settings.validClass);
                validationMessageEl.text('');
                
            } else {
                val.settings.highlight.call(val, error.element, val.settings.errorClass, val.settings.validClass);
                validationMessageEl.text(error.message);
                if (!focusedOnFirstError) {
                    $(error.element).focus();
                    focusedOnFirstError = true;
                }
            }            
        });
    };
    var resetErrors = function (form, validator) {
        var arr = form.serializeArray();
        var nullErrors = {};
        $.each(arr, function (i, param) {
            nullErrors[param.name] = null;
        });
        validator.showErrors(nullErrors);
    };
    var ajaxForm = function () {
        var form = this;
        var processingEl = $('<div class="d-none h3" role="alert"><i class="fas fa-sync-alt fa-spin"></i><span class="ms-1 fs-italic">Saving...</span></div>');
        form.append(processingEl);
        var validator = form.validate();
        validator.settings.showErrors = showErrors;

        var submitting = false;
        var postSubmit = function (url) {
            if (!submitting) {
                submitting = true;
                form.find('button[type="submit"').prop('disabled', true).bsHide();
                processingEl.bsShow();
                $.ajax({
                    url: url || form.attr('action'),
                    type: form.attr("method"),
                    dataType: "JSON",
                    data: new FormData(form[0]),
                    processData: false,
                    contentType: false,
                    success: function (data, status) {
                        submitting = false;
                        form.trigger('dataload', data);
                        form.find('button[type="submit"').prop('disabled', false).bsShow();
                        processingEl.bsHide();
                        if (!data.modelStateErrors) {
                            form.trigger('submit-success');
                        }
                    },
                    error: function (xhr, desc, err) {
                        submitting = false;
                        form.find('button[type="submit"').prop('disabled', false).bsShow();
                        processingEl.bsHide();
                    }
                });
            }
        };
        var formaction = null;
        form.find('button[type="submit"][formaction]').on('click', function (e) {
            e.preventDefault();
            formaction = $(e.target).closest('button[type="submit"][formaction]').attr('formaction');
            postSubmit(formaction);
        });
        form.submit(function (e) {
            e.preventDefault();
            postSubmit(formaction);            
            return false;
        });
        form.on('dataload', function (e, data) {
            resetErrors(form, validator);
            if (data.modelStateErrors) {
                validator.showErrors(data.modelStateErrors);
            }
        });
    };

    $.fn.ajaxForm = ajaxForm;
    $.each($('form[ajax]'), function (i, el) {
        $(el).ajaxForm();
    });
    $.RegisterOnLoadFunction(function (el) {
        el.find('form[ajax]').each(function (i, formEl) { $(formEl).ajaxForm(); });
    });
}));
