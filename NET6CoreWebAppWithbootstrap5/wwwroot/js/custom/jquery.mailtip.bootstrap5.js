/*!
 * jQuery auto select helper for emails
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
    $.mailtip = $.mailtip || {};
    $.mailtip.defaults = $.mailtip.defaults || {};
    $.mailtip.defaults.mails = ['@aol.com', '@gmail.com', '@msn.com', '@hotmail.com', '@outlook.com', '@yahoo.com'];
    $.mailtip.defaults.defaultPrefix = 'my.email'
    $.fn.mailtip = function (options) {
        options = $.extend(true, $.mailtip.defaults, options || {});
        $.each(this, function (index, input) {
            var inputEl = $(input);
            var id = inputEl.attr('id');
            inputEl.parent().addClass('position-relative');
            inputEl.addClass('dropdown-toggle');
            inputEl.attr('data-bs-toggle', 'dropdown');
            inputEl.attr('aria-expanded', false);
            inputEl.attr('autocomplete', 'off');

            var dropdownMenu = $('<ul>', { class: 'dropdown-menu w-100 shadow-sm', role:'listbox'});
            $.each(options.mails, function (i, email) {
                var dropdownMenuLi = $('<li>', {});
                var dropdownMenuItem = $('<a>', { 'data-option': email, class: 'dropdown-item small', role: 'option', 'aria-selected': false, href: '#' + id });
                dropdownMenuLi.append(dropdownMenuItem);
                dropdownMenu.append(dropdownMenuLi);
                dropdownMenuItem.on('click', function () {
                    dropdownMenu.find('.dropdown-item').attr('aria-selected', false);
                    $(this).attr('aria-selected', true);
                    inputEl.val($(this).text());
                });
            });
            inputEl.after(dropdownMenu);
            inputEl.on('click keyup', function () {
                var inputVal = inputEl.val().split('@')[0];
                $.each(dropdownMenu.find('.dropdown-item'), function (i, item) {
                    if (inputVal.trim() === '') {
                        $(item).text(options.defaultPrefix + $(item).data().option);
                    } else {
                        $(item).text(inputVal + $(item).data().option);
                    }
                    
                });
            });
        });
    };
    $('input[data-mailtip]').mailtip();
    
}));
