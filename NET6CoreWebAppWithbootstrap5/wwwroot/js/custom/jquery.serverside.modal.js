/*!
 * custom Modal handler 
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
	$.ServersideModal = {
		defaults: {
			modalFooterContentSelector: '[modal-content="footer"]',
			alert: function (message) { alert(message); },
			url: $('body').data().bsModalUrl,
			paramNames: ['title',
				'description',
				'size',
				'fullscreen',
				'backdrop',
				'static',
				'keyboard',
				'focus',
				'footer',
				'centered',
				'scrollable',
				'animation',
				'animationClass',
				'loading',
				'close',
				'modalClass'
			]
        }
	};
	var buildPostData = function (options) {
		var postData = {};
		$.each($.ServersideModal.defaults.paramNames, function (index, name) {
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
			$.ServersideModal.defaults.alert(message);
		}
	};
	var disposeModal = function (container, currentModal) {
		currentModal.dispose();
		container.empty();
	};
	var DisposeAndRecreate = function (container, options) {
		var currentModal = bootstrap.Modal.getInstance(container.find('>.modal')[0]);
		if (container.find('>.modal.show').length > 0) {
			container.find('>.modal').one('hidden.bs.modal', function () {
				disposeModal(container, currentModal);
				container.serversideModal(options);
			});
			currentModal.hide();
		} else {
			disposeModal(container, currentModal);
			container.serversideModal(options);
        }		
	};
	var ServersideModal = function (options) {
		var container = this;


		options = $.extend(true, {
			url: false,
			data: {},
			alert: 'alert',
			reload: true,
			appendContentSelector: null,
			onLoad: function () { }
		}, options || {});
		if (options.appendContentSelector != null) {
			options.url = false;
			options.reload = false;
		}
		if (container.find('>.modal').length > 0) {
			if (options.reload === false) {
				var currentModal = bootstrap.Modal.getInstance(container.find('>.modal')[0]);
				currentModal.show();
			} else {
				DisposeAndRecreate(container, options);
            }			
			return;
		}
		
		
		$.post($.ServersideModal.defaults.url, buildPostData(options)).done(function (modalHtml) {
			container.html(modalHtml);
			var modal = new bootstrap.Modal(container.find('.modal')[0]);
			modal.show();
			if (options.url && typeof options.url === 'string') {
				$.post(options.url, options.data).done(function (modalBodyHtml) {
					var modalBody = container.find('.modal-body');
					var modalFooter = container.find('.modal-footer');
					modalBody.html(modalBodyHtml);

					var modalFooterContent = modalBody.find($.ServersideModal.defaults.modalFooterContentSelector);
					if (modalFooter.length > 0 && modalFooterContent.length > 0) {
						modalFooter.append(modalFooterContent);
					} else {
						modalFooter.remove();
					}
					container.OnLoadContent();
					if (typeof options.onLoad === 'function') {
						options.onLoad(modal, container.find('.modal'));
					}
				}).fail(function (jqXHR, textStatus, errorThrown) {
					if (container.find('>.modal.show').length > 0) {
						container.find('>.modal').one('hidden.bs.modal', function () {
							disposeModal(container, modal);
							innerAlert(errorThrown);
						});
						modal.hide();
					} else {
						disposeModal(container, modal);
						innerAlert(errorThrown);
					}

				});

			} else {
				var modalBody = container.find('.modal-body');
				modalBody.empty();
				$(options.appendContentSelector).appendTo(modalBody);
				if (typeof options.onLoad === 'function') {
					options.onLoad(modal, container.find('.modal'));
				}
            }

		}).fail(function (jqXHR, textStatus, errorThrown) {
			innerAlert(errorThrown);
		});
	};

	$.fn.serversideModal = ServersideModal;

	var ServersideModalButton = function (options) {
		var button = this;
		options = $.extend(true, options || {}, button.data());
		if (options.container != null) {
			button.on('click', function () {
				options.container.serversideModal(options);
			});
		} else {
			var container = $('<div><div>');
			$('body').append(container);
			button.on('click', function () {
				container.serversideModal(options);
			});	
        }
			
	};
	$.fn.serversideModalButton = ServersideModalButton;
}));