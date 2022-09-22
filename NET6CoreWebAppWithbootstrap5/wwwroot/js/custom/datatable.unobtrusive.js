/*!
 * datatable.unobtrusive v1.0.0
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
    'use strict';
    var DataTable = $.fn.dataTable;
    $.extend(true, DataTable.Buttons.defaults, {
        buttonCreated: function (config, button) {
            return button;
            //return config.buttons ?
            //	$('<div class="btn-group"/>').append(button) :
            //	button;
        }
    });
    $.fn.dataTable.ext.errMode = function (settings, techNote, message) {
        $.Notification.danger({
            title: 'Datatable Error',
            message: message,
            autohide: false
        });
    };
    var getProcessingTemplate = function(text){
        return '<div class="w-100 text-center">' +
            '<span class="visually-hidden">' + text + '</span>' +
            '</div>';
    };
    $.fn.dataTable.defaults.oLanguage.sLengthMenu = '<div class="input-group input-group-sm">' +
        '<span class="input-group-text">Show</span>' +
        '_MENU_' +
        '<span class="input-group-text">entries</span>' +
        '</div>';
    $.fn.dataTable.defaults.oLanguage.sSearch = '<div class="input-group input-group-sm">' +
        '<span class="input-group-text">Search</span>' +
        '_INPUT_' +
        '</div>';
    $.fn.dataTable.defaults.bProcessing = true;
    $.fn.dataTable.defaults.oLanguage.sProcessing = getProcessingTemplate('Processing ...');
    $.fn.dataTable.defaults.oLanguage.sLoadingRecords = getProcessingTemplate('Loading ...');
    $.fn.dataTable.ext.classes.sFilterInput = 'form-control ml-0';
    $.fn.dataTable.ext.classes.sLengthSelect = 'form-select ml-0';
    $.fn.dataTable.ext.classes.sInfo = "dataTables_info pt-0 small";
    $.fn.dataTable.ext.classes.sProcessing = "card-body border-bottom table-processing-custom";
    $.fn.dataTable.ext.classes.sWrapper = $.fn.dataTable.ext.classes.sWrapper + " h-100";
    $.fn.dataTable.ext.classes.sFilter = $.fn.dataTable.ext.classes.sFilter + " ps-2";
    
    $.fn.dataTable.Buttons.defaults.dom.container.className = 'dt-buttons btn-group btn-group-sm';
    $.fn.dataTable.Buttons.defaults.buttons = false;//disable buttons by default

    var cardDom = '<"card h-100"<"card-header px-1 pt-1 pb-0 border-bottom-0"<"row"<"col dataTables_form_wrapper">><"row"<"col-auto pe-1 mb-1"l><"col-auto px-1 dataTables_size_wrapper mb-1"><"col-auto px-1 mb-1"B><"col ps-1 mb-1"f>>><"card-body p-0 table-responsive"t>r<"card-footer px-1 py-0 border-top-0"<"row align-items-center"<"col"i><"col"p>>>>';
    $.fn.dataTable.defaults.dom = cardDom;
    $.fn.dataTable.ext.classes.sSizeSelect = 'form-select ml-0';
    $.fn.dataTable.defaults.oLanguage.sSize = '<div class="input-group input-group-sm">' +
        '<span class="input-group-text">Size</span>' +
        '_MENU_' +
        '<button class="btn btn-outline-secondary dataTables_refresh" type="button" title="Refresh" data-bs-toggle="tooltip" data-bs-trigger="hover"><i class="fa fa-sync"></i><span class="sr-only">Refresh</span></button>'+
        '</div>';
    $.fn.dataTable.settings.aaSize = [["Normal", "table-sm"], ["Large", ""]];
    var initFeatureTableSize = function (settings, table) {
        var container = table.closest('.dataTables_wrapper').find('.dataTables_size_wrapper');
        var label = $('<label></label>');
        container.append(label);
        var menuOptions = $.fn.dataTable.settings.aaSize;
        var menu = $('<select class="dt-size-select ' + $.fn.dataTable.ext.classes.sSizeSelect + ' aria-controls="' + table.attr('id') + '"></select>');
        for (var i = 0; i < menuOptions.length; i++) {
            menu.append('<option value="' + menuOptions[i][1] + '">' + menuOptions[i][0] + '</option>')
        }
        var content = $.fn.dataTable.defaults.oLanguage.sSize.replace('_MENU_', menu[0].outerHTML);
        label.append(content);
        menu = label.find('select.dt-size-select'); // reset jquery object
        var onSizeInitOrChange = function () {
            for (var i = 0; i < menuOptions.length; i++) {
                table.removeClass(menuOptions[i][1]);
            }
            table.addClass(menu.val());
        }
        menu.on('change', onSizeInitOrChange);
        onSizeInitOrChange();
        var dt = table.DataTable();
        container.find('.dataTables_refresh').tooltip();
        container.find('.dataTables_refresh').on('click', function () {
            if (dt != null) {
                dt.ajax.reload();
            }
        });
    };
    
    $.fn.dataTable.defaults.fnInitComplete = function (settings, json) {
        initFeatureTableSize(settings, this);
    };

    $.extend(true, $.fn.dataTable.defaults, {
        dataLoad: {
            callback: function () { },
            headerSelector: 'thead th'
        }
    });
    var DataLoad = function (table, options, data) {
        options = $.extend(true, {
            dataLoad: $.fn.dataTable.defaults.dataLoad
        }, options || {});
        if (typeof options.dataLoad.callback === 'function') {
            if (typeof options.ajax !== 'undefined') {
                options.dataLoad.callback(data);
            } else {
                var heads = [];
                table.find(options.dataLoad.headerSelector).each(function () {
                    heads.push($(this).text().trim());
                });
                var data = [];
                $.each(table.DataTable().rows().data(), function (i, row) {
                    cur = {};
                    $.each(row, function (colIndex, col) {
                        cur[heads[colIndex]] = col.trim();
                    });
                    data.push(cur);
                });
                options.dataLoad.callback(data);
            }            
        }
    };    
    $.fn.dataTable.Api.register('columns().show()', function (columns) {
        if (typeof columns === 'string') {
            columns = columns.split(',');
        }
        else if (!Array.isArray(columns)) {
            columns = [columns];
        }
        var dt = this.table();
        var settings = this.settings();
        $.each(settings.init().columns, function (index, col) {            
            dt.column(index).visible(columns.indexOf(col.name) >= 0);
        });
    });
    $.fn.dataTable.Api.register('form()', function (form) {
        if (typeof form === 'string') {
            form = $(form);
        }
        
        var dt = this.table();
        var tableData = $(dt.node()).data();
        if (typeof tableData.dtForm == 'string') {
            $(tableData.dtForm).find('[trigger-dt-refresh]').off('change');
        }
        $(dt.node()).data('dtForm', '#' + form.attr('id'));
        tableData = $(dt.node()).data();        
        $(tableData.dtForm).find('[trigger-dt-refresh]').on('change', function () {            
            if (dt != null) {
                dt.ajax.reload();
            }
        });
    });
    $.fn.dataTable.Api.register('serversideModal()', function (options) {
        var dt = this.table();
        var tableData = $(dt.node()).data();
        if (!tableData.modalContainer) {
            var container = $('<div></div>');
            $('body').append(container);
            $(dt.node()).data('modalContainer', container);
            tableData = $(dt.node()).data();
        }        
        tableData.modalContainer.serversideModal(options);
    });
    $.fn.dataTable.ext.buttons.download = {
        className: 'btn btn-primary',
        text: '<i class="fas fa-file-download me-1"></i> Download',
        init: function (api, node, config) {
            $(node).removeClass('btn-secondary');
        },
        action: function (e, dt, node, config) {
            dt.download();
        }
    };
    $.fn.dataTable.Api.register('processing()', function (show) {
        return this.iterator('table', function (ctx) {
            ctx.oApi._fnProcessingDisplay(ctx, show);
        });
    });
    $.fn.dataTable.Api.register('download()', function () {
        var dt = this.table();
        var options = $(dt.node()).data().dtOptions.download;
        if (options.enabled === true) {
            var data = dt.ajax.params();
            data.download = true;
            data.downloadAll = options.downloadAll;
            dt.processing(true);
            var url = options.url;
            if (url === null || url.trim() === '') {
                url = dt.ajax.url();
            }
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                xhrFields: {
                    responseType: 'blob' // to avoid binary data being mangled on charset conversion
                },
                success: function (blob, status, xhr) {
                    // check for a filename
                    var filename = "";
                    var disposition = xhr.getResponseHeader('Content-Disposition');
                    if (disposition && disposition.indexOf('attachment') !== -1) {
                        var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                        var matches = filenameRegex.exec(disposition);
                        if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
                    }

                    if (typeof window.navigator.msSaveBlob !== 'undefined') {
                        // IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
                        window.navigator.msSaveBlob(blob, filename);
                    } else {
                        var URL = window.URL || window.webkitURL;
                        var downloadUrl = URL.createObjectURL(blob);

                        if (filename) {
                            // use HTML5 a[download] attribute to specify filename
                            var a = document.createElement("a");
                            // safari doesn't support this yet
                            if (typeof a.download === 'undefined') {
                                window.location.href = downloadUrl;
                            } else {
                                a.href = downloadUrl;
                                a.download = filename;
                                document.body.appendChild(a);
                                a.click();
                            }
                        } else {
                            window.location.href = downloadUrl;
                        }

                        setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100); // cleanup
                    }
                    dt.processing(false);
                },
                error: function () {
                    dt.processing(false);
                }
            });
        }
        
    });
    
    $(document).on('init.dt', function (e, settings) {
        if (e.namespace !== 'dt') {
            return;
        }
        DataLoad($(e.target), settings.oInit,null);
    });
    $(document).on('xhr.dt', function (e, settings, data, xhr) {
        if (e.namespace !== 'dt') {
            return;
        }
        var tableData = $(e.target).data();
        if (typeof tableData.dtForm === 'string' && tableData.dtForm !== '') {
            $(tableData.dtForm).trigger('dataload', data);
        }
        DataLoad($(e.target), settings.oInit, data);
    });
    $(document).on('draw.dt', function (e, settings, data, xhr) {
        if (e.namespace !== 'dt') {
            return;
        }
        var table = $(e.target);
        var tableData = table.data();
        table.find('td').addClass('align-middle');
        table.find('td button[row-action]').off('click').on('click', function () {
            var action = $(this).attr('row-action');
            var data = table.DataTable().row($(this).parents('tr')).data();
            table.trigger('row-action-' + action, data);
            if (typeof tableData.dtForm === 'string' && tableData.dtForm !== '') {
                $(tableData.dtForm).trigger('dt-row-action-' + action, data);
            }
        });
        table.OnLoadContent();
    });

    $.fn.serializeObject = function () {
        var arr = this.serializeArray();
        var obj = {};
        $.each(arr, function (i, param) {
            if (typeof obj[param.name] !== 'undefined') {
                if (Array.isArray(obj[param.name])) {
                    obj[param.name].push(param.value);
                } else {
                    obj[param.name] = [obj[param.name], param.value];
                }
            } else{
                obj[param.name] = param.value;
            }
        });
        return obj;
    };
    
    $.fn.dataTableCustom = function (options) {
        var table = this;
        
        var tableData = this.data();
        if (typeof options.ajax === 'object' && options.ajax !== null) {
            if (typeof tableData.dtForm === 'string' && tableData.dtForm !== '') {
                $(tableData.dtForm).ajaxForm();
            }
            options.ajax.data = function (data) {
                var parameters = {};
                if (typeof tableData.dtForm === 'string' && tableData.dtForm !== '') {
                    parameters = $.extend(true, parameters, $(tableData.dtForm).serializeObject());
                    if (typeof parameters.__RequestVerificationToken !== 'undefined') {
                        data.__RequestVerificationToken = parameters.__RequestVerificationToken;
                    }                  
                    
                }
                if (typeof options.parameters === 'function') {
                    parameters = $.extend(true, parameters, options.parameters(parameters));
                } else if (typeof options.parameters === 'object' && options.parameters !== null) {
                    parameters = $.extend(true, parameters, options.parameters);
                }
                data.Parameters = parameters;
                return data;
            };
            $(tableData.dtForm).find('[trigger-dt-refresh]').on('change', function () {
                var dt = table.DataTable();
                if (dt != null) {
                    dt.ajax.reload();
                }
            });
            
        }
        if (options.serverSide === true) {
            var oldInitComplete = options.initComplete || function (settings, json) {
                initFeatureTableSize(settings, table);
            };

            var footer;
            var useColumnFilters = options.columns.some(c => typeof c.filter !== 'undefined' && c.filter !== null);
            if (useColumnFilters) {
                footer = $('<tfoot style="display: table-header-group;"><tr>' + Array(options.columns.length + 1).join('<td></td>') + '</tr></tfoot>');
                table.append(footer);
            }
            
            options.initComplete = function (settings, json) {
                var api = this.api();
                if (tableData.appendForm === true) {
                    //var searchInputGroup = table.closest('.dataTables_wrapper').find('.dataTables_filter .input-group');
                    //var formDropdownEl = $('<button class="btn btn-outline-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false">Filter</button>');
                    //var formDropdownMenuEl = $('<di class="dropdown-menu dropdown-menu-end"></div>');
                    //formDropdownMenuEl.append($(tableData.dtForm));
                    //searchInputGroup.append(formDropdownEl).append(formDropdownMenuEl);

                    var formWrapper = table.closest('.dataTables_wrapper').find('.dataTables_form_wrapper');
                    formWrapper.append($(tableData.dtForm));
                    $(tableData.dtForm).bsShow();
                }


                var searchWaitInterval;
                var stop = function () {
                    if (searchWaitInterval !== null) {
                        clearInterval(searchWaitInterval);
                        searchWaitInterval = null;
                    }
                }
                // Grab the datatables input box and alter how it is bound to events
                $(settings.nTableWrapper).find(".dataTables_filter input")
                    .unbind() // Unbind previous default bindings
                    .bind("input", function (e) { // Bind our desired behavior
                        var item = $(this);
                        stop();
                        searchWaitInterval = setInterval(function () {
                            stop();
                            api.search($(item).val()).draw();
                        }, options.searchDelay || 1000);
                        return;
                    });
                if (useColumnFilters) {
                    table.find('thead').after(footer);
                    api.columns().every(function () {
                        var column = this;
                        var config = options.columns[column.index()];
                        var container = $(column.footer());
                        container.empty();
                        var filterId = table.attr('id') + config.name + 'Filter';
                        var title = $(column.header()).text();
                        if (typeof config.filter !== 'undefined' && config.filter !== null) {
                            var label = config.filter.label || title;
                            var searchWaitInterval;
                            var stop = function () {
                                if (searchWaitInterval !== null) {
                                    clearInterval(searchWaitInterval);
                                    searchWaitInterval = null;
                                }
                            }
                            var type = config.filter.type || 'text';
                            if (type === 'checkbox') {

                                //var wrapper = $('<div class="form-check"></div>').appendTo(container);
                                
                                var filter = $('<input class="btn-check" type="checkbox" value="" id="' + filterId + '" autocomplete="off">').appendTo(container);
                                var labelEl = $('<label class="btn btn-outline-primary btn-sm w-100" for="' + filterId + '">' + label + '</label>').appendTo(container);
                                var onFilter = function () {
                                    var val = filter.prop('checked');
                                    column
                                        .search(val)
                                        .draw();
                                }
                                filter.on('change', onFilter);
                                onFilter();//imediate filter;
                            }
                            else if (type === 'switch') {

                                var wrapper = $('<div class="form-check form-switch"></div>').appendTo(container);
                                var filter = $('<input class="form-check-input" type="checkbox" role="switch" value="" id="' + filterId + '">').appendTo(wrapper);
                                var labelEl = $('<label class="form-check-label" for="' + filterId + '">' + label + '</label>').appendTo(wrapper);
                                var onFilter = function () {
                                    var val = filter.prop('checked');
                                    column
                                        .search(val)
                                        .draw();
                                }
                                filter.on('change', onFilter);
                                onFilter();//imediate filter;
                            }
                            else if (type === 'radio' && Array.isArray(config.filter.options)) {
                                var wrapper = $('<div class="btn-group btn-group-sm w-100"></div>').appendTo(container);
                                $.each(config.filter.options, function (index, option) {
                                    var value = option[0];
                                    var label = option[1] || value;
                                    var optionId = filterId + index;
                                    var optionEl = $('<input class="btn-check" type="radio" name="' + filterId + '" value="' + value + '" id="' + optionId + '" autocomplete="off">');
                                    if (index === 0) {
                                        optionEl = $('<input class="btn-check" type="radio" name="' + filterId + '" value="' + value + '" id="' + optionId + '" autocomplete="off" checked>');
                                    }
                                    optionEl.appendTo(wrapper);
                                    var labelEl = $('<label class="btn btn-outline-primary" for="' + optionId + '">' + label + '</label>').appendTo(wrapper);
                                });
                                var filter = $('input[name="' + filterId + '"]');
                                var onFilter = function () {
                                    var val = $('input[name="' + filterId + '"]:checked').val();
                                    column
                                        .search(val)
                                        .draw();
                                }
                                filter.on('change', onFilter);
                                onFilter();//imediate filter;
                            }
                            else if (type === 'select' && Array.isArray(config.filter.options)) {
                                var filter = $('<select id="' + filterId+'" class="form-select form-select-sm" aria-label="' + label +'"></select>').appendTo(container);
                                $('<option value="" class="text-muted" selected>Select '+label+'</option>').appendTo(filter);
                                $.each(config.filter.options, function (index, option) {
                                    var value = option[0];
                                    var label = option[1] || value;
                                    var optionEl = $('<option value="' + value + '">' + label +'</option>').appendTo(filter);
                                });
                                var onFilter = function () {
                                    var val = filter.val();
                                    column
                                        .search(val)
                                        .draw();
                                }
                                filter.on('change', onFilter);
                                filter.bootstrapSelect({
                                    classes: 'btn-outline-secondary btn-sm'
                                });
                            }
                            else {
                                var filter = $('<input class="form-control form-control-sm" type="' + type + '" placeholder="Search ' + label + '" />')
                                    .appendTo(container)
                                    .on('input clear', function () {
                                        var item = $(this);
                                        stop();
                                        searchWaitInterval = setInterval(function () {
                                            stop();
                                            var val = item.val();
                                            if (type === 'text ') {
                                                val = $.fn.dataTable.util.escapeRegex(item.val());
                                            }
                                            column
                                                .search(val)
                                                .draw();
                                        }, options.searchDelay || 1000);

                                    });
                            }
                            
                        } else {
                            var filter = $('<input class="form-control form-control-sm" type="hidden" placeholder="Search Unavailable" disabled/>')
                                .appendTo(container);
                            $('<span>Search Unavailable</span>').appendTo(container);
                            container.addClass('table-filter disabled');
                        }
                    });
                }
                
               
                oldInitComplete(settings, json);
            };
        }
        table.data('dtOptions', options);
        return this.dataTable(options);
    };

	$.fn.dataTableUnobtrusive = function () {
        $.each(this, function (index, tableEl) {
            
			var table = $(tableEl);
            var data = table.data();
            if (typeof data.dtOptionsUrl === 'string') {                
                $.get(data.dtOptionsUrl).done(function (options) {
                    if (!options.error) {
                        table.dataTableCustom(options);
                    } 
                    
                });
            } else {
                var options = window.datatableUnobtrusiveOptions[data.tableOptionsId];
                table.dataTableCustom(options);
            }            
		});
    };
    $.fn.onDataTableRowAction = function (action, cb) {
        this.on('row-action-' + action, cb);
        return this;
    };
    $('table[data-type="datatable"]').dataTableUnobtrusive();
    
}));


