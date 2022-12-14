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
	


	/*
 Check if the stylesheet is internal or hosted on the current domain.
 If it isn't, attempting to access sheet.cssRules will throw a cross origin error.
 See https://developer.mozilla.org/en-US/docs/Web/API/CSSStyleSheet#Notes
 
 NOTE: One problem this could raise is hosting stylesheets on a CDN with a
 different domain. Those would be cross origin, so you can't access them.
*/
	const isSameDomain = (styleSheet) => {
		// Internal style blocks won't have an href value
		if (!styleSheet.href) {
			return true;
		}

		return styleSheet.href.indexOf(window.location.origin) === 0;
	};

	/*
	 Determine if the given rule is a CSSStyleRule
	 See: https://developer.mozilla.org/en-US/docs/Web/API/CSSRule#Type_constants
	*/
	const isStyleRule = (rule) => rule.type === 1;

	/**
	 * Get all custom properties on a page
	 * @return array<array[string, string]>
	 * ex; [["--color-accent", "#b9f500"], ["--color-text", "#252525"], ...]
	 */
	const getCSSCustomPropIndex = (prefix) =>
		// styleSheets is array-like, so we convert it to an array.
		// Filter out any stylesheets not on this domain
		[...document.styleSheets].filter(isSameDomain).reduce(
			(finalArr, sheet) =>
				finalArr.concat(
					// cssRules is array-like, so we convert it to an array
					[...sheet.cssRules].filter(isStyleRule).reduce((propValArr, rule) => {
						const props = [...rule.style]
							.map((propName) => [
								propName.trim(),
								rule.style.getPropertyValue(propName).trim()
							])
							// Discard any props that don't start with "--". Custom props are required to.
							.filter(([propName]) => propName.indexOf(prefix) === 0);

						return [...propValArr, ...props];
					}, [])
				),
			[]
		);
	$.ThemeColor = {};
	$.each(getCSSCustomPropIndex("--bs-chart-color-"), function (i, [name, value]) {
		$.ThemeColor[name.replace("--bs-chart-color-", "")] = value;
	});

	$.GetThemeColor = (theme) => $.ThemeColor[theme];


	$($.popover.defaults.selector).popover();
	$($.tooltip.defaults.selector).tooltip();
	$($.carousel.defaults.selector).carousel();
}));
