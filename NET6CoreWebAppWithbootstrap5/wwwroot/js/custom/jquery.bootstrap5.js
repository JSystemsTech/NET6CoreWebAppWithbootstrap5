/*!
 * jQuery page load events
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
	$.popover = $.popover || {};
	$.popover.defaults = $.extend(true, {
		selector: '[data-bs-toggle="popover"]'
	}, $.popover.defaults || {});

	$.fn.popover = function (options) {
		$.each(this, function (index, el) {
			var popover = new bootstrap.Popover(el, options);
			$(el).data('_popover', popover);
		});
	};

	$.fn.Popover = function () {
		return this.data()._popover;
	};


	$.tooltip = $.tooltip || {};
	$.tooltip.defaults = $.extend(true, {
		selector: '[data-bs-toggle="tooltip"]'
	}, $.tooltip.defaults || {});
	
	$.fn.tooltip = function (options) {
		$.each(this, function (index, el) {
			var popover = new bootstrap.Tooltip(el, options);
			$(el).data('_tooltip', popover);
		});
	};

	$.carousel = $.carousel || {};
	$.carousel.defaults = $.extend(true, {
		selector: '[data-bs-ride="carousel"]'
	}, $.carousel.defaults || {});

	$.fn.carousel = function (options) {
		$.each(this, function (index, el) {
			var carousel = new bootstrap.Carousel(el, options);
			$(el).data('_carousel', carousel);
		});
	};


	$.fn.Tooltip = function () {
		return this.data()._tooltip;
	};
	$.fn.Carousel = function () {
		return this.data()._carousel;
	};

	$.fn.bsShow = function () {
		return this.removeClass('d-none');
	};
	$.fn.bsHide = function () {
		return this.addClass('d-none');
	};
	$.fn.bsFormDisable = function () {
		this.find("input,select").prop("disabled", true);
		this.find("button").attr("disabled", true);
	};
	$.fn.bsFormEnable = function () {
		this.find("input,select").prop("disabled", false);
		this.find("button").attr("disabled", false);
	};
	var OnLoadFunctions = [
		function (el) {
			el.find($.popover.defaults.selector).popover();
			el.find($.tooltip.defaults.selector).tooltip();
			el.find($.carousel.defaults.selector).carousel();
		}
	];
	$.RegisterOnLoadFunction = function (cb) {
		if (typeof cb === 'function') {
			OnLoadFunctions.push(cb);
		}
	};
	$.fn.OnLoadContent = function (cb) {
		var el = this;
		$.each(OnLoadFunctions, function (index, cb) {
			cb(el);
		});
		if (typeof cb === 'function') {
			cb();
		}
	};
	$.GetThemeColor = function (theme, opacity) {
		return window.getComputedStyle(document.body).getPropertyValue('--bs-' + theme);
	};
	$($.popover.defaults.selector).popover();
	$($.tooltip.defaults.selector).tooltip();
	$($.carousel.defaults.selector).carousel();
}));
