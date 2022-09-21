/*!
 * jQuery ajax setup 
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
	window.initData = function () {
		return $('[js-init-data="true"]').data();
	};
	var data = $('body').data();
	var logout = function () {
		var logoutButton = $('a[ajax-logout]');
		if (logoutButton.length > 0) {
			logoutButton[0].click();
		}
	};
	var reload = function () {
		var reloadButton = $('a[ajax-reload]');
		if (reloadButton.length > 0) {
			reloadButton[0].click();
		}
	};
	$.ReloadPage = reload;
	var timeoutDateStr = '';
	var cd;
	var warn = false;
	var updateLogoutCountdown = function (sessionExpireDate) {
		if (typeof sessionExpireDate !== 'undefined') {
			if (sessionExpireDate !== timeoutDateStr) {
				timeoutDateStr = sessionExpireDate;
				if (timeoutDateStr !== '') {
					if (cd) {
						cd.reset(timeoutDateStr);
					} else {
						cd = $('[data-bs="logout-countdown"]').counter({
							selector: '[data-bs="logout-countdown"]',
							countdown: true,
							date: timeoutDateStr,
							done: function () {
								logout();
							},
							render: function (text, diff) {
								return diff <= 5 * 60 * 1000 ?
									'<span class="badge rounded-pill bg-primary text-dark "><i class="fas fa-exclamation-circle"></i><span class="ms-2">' + text + '</span></span>' :
									diff <= 2 * 60 * 1000 ?
									'<span class="badge rounded-pill bg-primary text-dark "><i class="fas fa-exclamation-triangle"></i><span class="ms-2">' + text + '</span></span>' :
									'<span class="badge rounded-pill bg-success-dark text-light "><i class="fas fa-check"></i><span class="ms-2">' + text + '</span></span>'
							},
							tick: function (diff) {
								if (diff <= data.sessionTimoutWarning) {
									if (!warn) {
										console.log('session will timout soon');
										warn = true;
									}
								} else {
									warn = false;
                                }
							}
							
						});
                    }					
				}
			}
		}
	};

	$(document).ajaxError(function (event, request, settings) {
		if (request.status === 401) {
			logout();
		} else if (request.status === 403) {
			var message = request.responseJSON && request.responseJSON.Message ? request.responseJSON.Message : 'You are unauthorized to access this url.';
			$.Notification.warning({
				title: request.status + ' Unauthorized',
				message: '<div class="small">' + message + '</div><div class="small"><b class="me-1">Url:</b>' + settings.url +'</div>',
				autohide: false,
				html: true
			});
		} else if (request.status === 500) {
			$.Notification.warning({
				title: request.status + ' Not Found',
				message: '<div class="small">Unable to find</div><div class="small"><b class="me-1">Url:</b>' + settings.url + '</div>',
				autohide: false,
				html: true
			});
		} else {
			$.Notification.warning({
				title: request.status + ' Unknown Error',
				message: '<div class="small">Unable to reach or get a response from</div><div class="small"><b class="me-1">Url:</b>' + settings.url + '</div>',
				autohide: false,
				html: true
			});
		}
	});
	$(document).ajaxSuccess(function (event, request, settings) {
		if (request.responseJSON) {
			var responseJSON = request.responseJSON;
			updateLogoutCountdown(responseJSON.sessionExpireDate);
			if (responseJSON.notification) {
				$.Notification.add(responseJSON.notification);
			}
        }		
	});
	updateLogoutCountdown(data.sessionExpireDate);


	
}));