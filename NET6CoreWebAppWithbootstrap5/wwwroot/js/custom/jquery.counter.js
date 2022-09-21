
/*!
 * jQuery Counter
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
	$.fn._updateCounterEl = function (diff, format, render) {
		render = render || function (text) {
			return text;
		};
		var target = this;
		var data = target.data();
		format = data.format || format;
		var duration = moment.duration(diff, 'milliseconds');
		var years = duration.years();
		var months = duration.months();
		var days = duration.days();
		var hours = duration.hours();
		var minutes = duration.minutes();
		var seconds = duration.seconds();

		var text = format
			.replaceAll('YYYY', years + ' year<#>')
			.replaceAll('YYY', years + ' yr<#>')
			.replaceAll('YY', years < 10 ? '0' + years : years)
			.replaceAll('Y', years)
			.replaceAll('MMMM', months + ' <~>ont<*><#>')
			.replaceAll('MMM', months + ' <~>o')
			.replaceAll('MM', months < 10 ? '0' + months : months)
			.replaceAll('M', months)
			.replaceAll('DDDD', days + ' day<#>')
			.replaceAll('DDD', days + ' d')
			.replaceAll('DD', days < 10 ? '0' + days : days)
			.replaceAll('D', days)
			.replaceAll('hhhh', hours + ' <*>our<#>')
			.replaceAll('hhh', hours + ' <*>r')
			.replaceAll('hh', hours < 10 ? '0' + hours : hours)
			.replaceAll('h', hours)
			.replaceAll('mmmm', minutes + ' <~>inute<#>')
			.replaceAll('mmm', minutes + ' <~>in')
			.replaceAll('mm', minutes < 10 ? '0' + minutes : minutes)
			.replaceAll('m', minutes)
			.replaceAll('ssss', seconds + ' <#>econd<#>')
			.replaceAll('sss', seconds + ' <#>ec')
			.replaceAll('ss', seconds < 10 ? '0' + seconds : seconds)
			.replaceAll('s', seconds)
			.replaceAll('<~>', 'm')
			.replaceAll('<#>', 's')
			.replaceAll('<*>', 'h');
		target.html(render(text, diff));
	};
	$.fn.counter = function (options) {
		var targetEls = this;
		options = $.extend(true, {
			interval: 1000,
			format: 'hh:mm:ss',
			done: function () { },
			render: function (text) { return text; },
			tick: function () { },
			countdown: false,
			selector: null
		}, (options || {}));
		var timer;
		var timerInterval;

		var clearCountdownInterval = function () {
			if (timerInterval) {
				clearInterval(timerInterval);
				timerInterval = null;
			}
		};
		var begin = function (date) {
			if (timer) {
				clearTimeout(timer);
			}
			clearCountdownInterval();
			var startDate = moment.isMoment(date) ? date : moment(date);
			var getDiff = function () {
				var now = moment();
				return options.countdown ? startDate.diff(now) : now.diff(startDate);
			};
			if (options.countdown) {
				timer = setTimeout(function () {
					clearCountdownInterval();
					options.done();
				}, getDiff());
            }			
			var update = function () {
				var diff = getDiff();
				if (options.selector) {
					targetEls = $(options.selector);
                }
				$.each(targetEls, function (i, target) {
					$(target)._updateCounterEl(diff, options.format,options.render);
				});
				options.tick(diff);
			};
			timerInterval = setInterval(update, options.interval);
			update();
		};
		begin(options.date);
		return {
			reset: begin
		};
	};	
}));