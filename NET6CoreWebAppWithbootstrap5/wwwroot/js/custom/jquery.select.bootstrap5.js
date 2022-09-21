/*!
 * custom selet component helper for boostrap 5
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
    var parseSelectOptions = function (section) {
        var options = [];
        $.each(section.find('>option'), function (index, option) {
            var el = $(option);
            var config = {
                value: el.attr('value'),
                text: el.text(),
                selected: el.is(':selected'),
                disabled: el.prop('disabled'),
                data: el.data()
            };
            options.push(config);
        });
        return options;
    };
    var mapOptions = function (select) {
        var map = [];
        $.each(select.find('option'), function (index, option) {
            var el = $(option);
            var config = {
                value: el.attr('value'),
                text: el.text()
            };
            map.push(config);
        });
        return map;
    };
    var mapValues = function (select) {
        var values = [];
        $.each(select.find('option'), function (index, option) {
            var el = $(option);
            values.push(el.attr('value'));
        });
        return values;
    };
    var parseSelect = function (select) {
        var optGroups = select.find('>optgroup');
        var sections = [];
        if (optGroups.length > 0) {
            $.each(optGroups, function (index, group) {
                var el = $(group);
                sections.push({
                    label: el.attr('label'),
                    options: parseSelectOptions(el)
                });
            });
        } else {
            sections.push({
                label: null,
                options: parseSelectOptions(select)
            });
        }
        var map = mapOptions(select);
        var config = {
            select: select,
            values: mapValues(select),
            map: map,
            getSelectedMaps: function () {
                var val = select.val();
                if (!Array.isArray(val)) {
                    val = [val];
                };
                var selectedMaps = {
                    names: [],
                    values: []
                };
                $.each(map, function (i, item) {
                    if (val.indexOf(item.value) >= 0) {
                        selectedMaps.names.push(item.text);
                        selectedMaps.values.push(item.value);
                    }
                });
                return selectedMaps;
            },
            name: select.attr('name'),
            id: select.attr('id'),
            disabled: select.prop('disabled'),
            multiple: select.prop('multiple'),
            required: select.prop('required'),
            data: select.data(),
            sections: sections
        };
        return config;
    };
    var addMenuItem = function (ul, config, group) {
        var attrs = {
            class: 'dropdown-item',
            type: 'button'
        };
        var liveSearchValues = [config.text];
        if (typeof config.data.tokens === 'string') {
            liveSearchValues = liveSearchValues.concat(config.data.tokens.split(','));
        }
        if (typeof group === 'string' && group.trim() !== '') {
            liveSearchValues.push(group);
        }
        if (typeof config.data.subtext === 'string' && config.data.subtext.trim() !== '') {
            liveSearchValues.push(config.data.subtext);
        }
        var liAttrs = {
            role: 'option',
            'data-value': config.value,
            'data-live-search': liveSearchValues.join(',').toLowerCase(),
            'data-group': group
        };
        if (config.selected === true && config.disabled !== true) {
            attrs.class = attrs.class + ' active';
            liAttrs['aria-current'] = true;
        }
        if (config.disabled === true) {
            attrs.class = attrs.class + ' disabled';
            attrs.tabindex = -1;
            attrs['aria-disabled'] = true;
            attrs['disabled'] = true;
        }
        var button = $('<button>', attrs);
        var buttonText = $('<span>', { class: 'ms-1' }).text(config.text);
        var buttonIcon = $('<i>', { 'ds-button-icon': true, class: 'fa fa-check' });
        button.append(buttonIcon).append(buttonText);
        if (typeof config.data.subtext === 'string' && config.data.subtext.trim() !== '') {
            var subtext = $('<span>', { class: 'ms-1 fw-light' }).text(config.data.subtext).css('font-size', '0.8em');
            button.append(subtext);
        }
        $('<li>', liAttrs).append(button).appendTo(ul);
    };
    var addMenuToolbar = function (ul, config) {
        var buttonGroup = $('<span>', { class: 'btn-group btn-group-sm w-100' });
        var li = $('<li>', { class: 'px-2' }).append(buttonGroup);
        var selectAll = $('<button>', { type: 'button', class: 'btn btn-outline-primary' }).text('Select All');
        var deselectAll = $('<button>', { type: 'button', class: 'btn btn-outline-primary' }).text('Deselect All');
        buttonGroup.append(selectAll).append(deselectAll);
        li.appendTo(ul);
        selectAll.on('click', function () {
            config.select.val(config.values);
            config.select.trigger('change');
        });
        deselectAll.on('click', function () {
            config.select.val([]);
            config.select.trigger('change');
        });
    };
    var addMenuLiveSearch = function (ul, config) {
        var input = $('<input>', { class: 'form-control form-control-sm', type: 'text', placeholder: 'Search for items' });
        var text = $('<div>', { class: 'fw-light mt-1' }).css('font-size', '0.8em');
        var li = $('<li>', { class: 'px-2' }).append(input).append(text);
        if (config.multiple === true) {
            li.addClass('pt-2');
        }
        li.appendTo(ul);
        var timer;
        input.keyup(function () {
            if (timer) {
                clearTimeout(timer);
            }
            timer = setTimeout(function () {
                if (input.val().trim() === '') {
                    ul.find('[data-value]').removeClass('sr-only');
                    ul.find('[data-opt-group]').removeClass('sr-only');
                    text.empty();
                } else {
                    $.each(ul.find('[data-value]'), function (i, el) {
                        var liveSearch = $(el).data().liveSearch.split(',');
                        var matches = liveSearch.filter(s => s.includes(input.val().trim().toLowerCase()));
                        if (matches.length > 0) {
                            $(el).removeClass('sr-only');
                        } else {
                            $(el).addClass('sr-only');
                        }
                    });
                    $.each(ul.find('[data-opt-group]'), function (i, el) {
                        var group = $(el).data().optGroup;
                        if (ul.find('[data-value][data-group="' + group + '"]').not('.sr-only').length > 0) {
                            $(el).removeClass('sr-only');
                        } else {
                            $(el).addClass('sr-only');
                        }
                    });
                    text.text(ul.find('[data-value]').not('.sr-only').length + ' results found');
                }
            }, 200);
        });
    };
    var buildULSelectMenu = function (config, options) {
        options = $.extend(true, {
            classes: 'btn-outline-secondary'
        }, options);
        var dropdown = $('<div>', { class: 'dropdown w-100' });
        var button = $('<button>', {
            id: config.id + 'DropdownToggle',
            class: 'btn ' + options.classes +' dropdown-toggle w-100',
            type: 'button',
            'data-bs-toggle': 'dropdown',
            'aria-expanded': false
        });
        var menu = $('<ul>', {
            class: 'dropdown-menu shadow',
            role: 'listbox',
            tabindex: -1,
            'aria-labelledby': config.id + 'DropdownToggle',
        });
        menu.css('overflow-y', 'auto');
        menu.css('max-height', '200px');
        if (config.multiple === true) {
            addMenuToolbar(menu, config);
        }
        addMenuLiveSearch(menu, config);
        $('<li>', { class: 'dropdown-divider' }).appendTo(menu);
        $.each(config.sections, function (index, section) {
            if (section.label !== null) {
                $('<li>', { 'data-opt-group': section.label }).append($('<h6>', { class: 'dropdown-header py-1' }).text(section.label)).appendTo(menu);
            }
            $.each(section.options, function (optIndex, option) {
                addMenuItem(menu, option, section.label);
            });
        });
        dropdown.append(button);
        dropdown.append(menu);
        config.select.after(dropdown);
        config.select.addClass('d-none');

        //init dropdown
        var dropdown = new bootstrap.Dropdown(document.querySelector('#' + config.id + 'DropdownToggle'), {
            autoClose: 'outside',
            popperConfig: {
                strategy: 'fixed'
            }
        });

        var updateMenu = function () {
            var selectedMaps = config.getSelectedMaps();
            if (selectedMaps.names.length === 0) {
                button.text('Nothing selected');
            } else if (selectedMaps.names.length === 1) {
                button.text(selectedMaps.names[0]);
            } else {
                button.text(selectedMaps.names.length + ' items selected');
            }
            menu.find('[data-value]').removeAttr('aria-current');
            menu.find('[data-value]> button').removeClass('active');
            menu.find('[data-value] [ds-button-icon]').css('opacity', 0);
            $.each(selectedMaps.values, function (i, value) {
                menu.find('[data-value="' + value + '"]').attr('aria-current', 'true');
                menu.find('[data-value="' + value + '"]>button').addClass('active');
                menu.find('[data-value="' + value + '"] [ds-button-icon]').css('opacity', 1);
            });
        };
        menu.find('[data-value]> button').on('click', function () {
            var li = $(this).parent();
            var value = li.data().value;

            var selectedMaps = config.getSelectedMaps();
            if (config.multiple === true && selectedMaps.values.indexOf(String(value)) >= 0) {
                selectedMaps.values.splice(selectedMaps.values.indexOf(String(value)), 1);
                config.select.val(selectedMaps.values);
                config.select.trigger('change');
            }
            else if (selectedMaps.values.indexOf(String(value)) < 0) {

                var newVal = selectedMaps.values;
                if (config.multiple === true) {
                    newVal.push(value);
                } else {
                    newVal = value;
                }
                config.select.val(newVal);
                config.select.trigger('change');
            }
        });
        updateMenu();
        config.select.on('change', updateMenu);
        
        
        return {
            val: function (value) {
                config.select.val(value);
                config.select.trigger('change');
            }
        };
    };
    $.fn.bootstrapSelect = function (options) {
        options = $.extend(true, this.data(), options || {});
        return buildULSelectMenu(parseSelect(this), options);
    };
    $('[data-bs-toggle="select"]').each(function (i, el) { $(el).bootstrapSelect(); });
    $.RegisterOnLoadFunction(function (el) {
        el.find('[data-bs-toggle="select"]').each(function (i, targetEl) { $(targetEl).bootstrapSelect(); });
    });
}));
