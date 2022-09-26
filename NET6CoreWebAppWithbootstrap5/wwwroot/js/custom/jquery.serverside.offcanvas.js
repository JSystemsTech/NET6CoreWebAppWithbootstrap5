/*!
 * custom Offcanvas handler 
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
	$.ServersideOffcanvas = {
		defaults: {
			alert: function (message) { alert(message); },
			url: $('body').data().bsOffcanvasUrl,
			paramNames: ['title',
				'description',
				'placement',
				'backdrop',
				'static',
				'keyboard',
				'focus',
				'scroll',
				'animation',
				'animationClass',
				'loading',
				'close']
        }
	};
	var buildPostData = function (options) {
		var postData = {};
		$.each($.ServersideOffcanvas.defaults.paramNames, function (index, name) {
			if (typeof options[name] !== 'undefined') {
				postData[name] = options[name];
            }
		});
		return postData;
	};
	
	var innerAlert = function (options, message) {
		if (typeof options.alert === 'function') {
			options.alert(message);
		} else if (options.alert === 'alert') {
			alert(message);
		} else {
			$.ServersideOffcanvas.defaults.alert(message);
		}
	};
	var disposeOffcanvas = function (container, currentOffcanvas) {
		currentOffcanvas.dispose();
		container.empty();
	};
	var DisposeAndRecreate = function (container, options) {
		var currentOffcanvas = bootstrap.Offcanvas.getInstance(container.find('>.offcanvas')[0]);
		if (container.find('>.offcanvas.show').length > 0) {
			container.find('>.offcanvas').one('hidden.bs.offcanvas', function () {
				disposeOffcanvas(container, currentOffcanvas);
				container.serversideOffcanvas(options);
			});
			currentOffcanvas.hide();
		} else {
			disposeOffcanvas(container, currentOffcanvas);
			container.serversideOffcanvas(options);
        }		
	};
	var ServersideOffcanvas = function (options) {
		var container = this;


		options = $.extend(true, {
			url: false,
			data: {},
			alert: 'alert',
			appendContentSelector: null,
			reload: true,
			onLoad: function () { }
		}, options || {});
		if (options.appendContentSelector != null) {
			options.url = false;
			options.reload = false;
		}
		if (container.find('>.offcanvas').length > 0) {
			if (options.reload === false) {
				var currentOffcanvas = bootstrap.Offcanvas.getInstance(container.find('>.offcanvas')[0]);
				currentOffcanvas.show();
			} else {
				DisposeAndRecreate(container, options);
            }			
			return;
		}
		
		$.post($.ServersideOffcanvas.defaults.url, buildPostData(options)).done(function (offcanvasHtml) {
			container.html(offcanvasHtml);
			var offcanvas = new bootstrap.Offcanvas(container.find('.offcanvas')[0]);
			offcanvas.show();
			if (options.url && typeof options.url === 'string') {
				$.post(options.url, options.data).done(function (offcanvasBodyHtml) {
					var offcanvasBody = container.find('.offcanvas-body');
					offcanvasBody.html(offcanvasBodyHtml);

					container.OnLoadContent();
					if (typeof options.onLoad === 'function') {
						options.onLoad(offcanvas, container.find('.offcanvas'));
					}
				}).fail(function (jqXHR, textStatus, errorThrown) {
					if (container.find('>.offcanvas.show').length > 0) {
						container.find('>.offcanvas').one('hidden.bs.offcanvas', function () {
							disposeOffcanvas(container, offcanvas);
							innerAlert(errorThrown);
						});
						offcanvas.hide();
					} else {
						disposeOffcanvas(container, offcanvas);
						innerAlert(errorThrown);
					}

				});
			} else {
				var offcanvasBody = container.find('.offcanvas-body');
				offcanvasBody.empty();
				$(options.appendContentSelector).appendTo(offcanvasBody);
				if (typeof options.onLoad === 'function') {
					options.onLoad(offcanvas, container.find('.offcanvas'));
				}
			}

		}).fail(function (jqXHR, textStatus, errorThrown) {
			innerAlert(errorThrown);
		});
	};

	$.fn.serversideOffcanvas = ServersideOffcanvas;

	var ServersideOffcanvasButton = function (options) {
		var button = this;
		options = $.extend(true, options || {}, button.data());
		if (options.container != null) {
			button.on('click', function () {
				options.container.serversideOffcanvas(options);
			});
		} else {
			var container = $('<div><div>');
			$('body').append(container);
			button.on('click', function () {
				container.serversideOffcanvas(options);
			});
		}	
	};

	$.fn.serversideOffcanvasButton = ServersideOffcanvasButton;
}));