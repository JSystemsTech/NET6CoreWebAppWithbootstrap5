/*!
 * place all pollyfill code here
 *
 * Copyright (c) 2021 Jonathan McGuire
 * Released under the MIT license
 */

(function (factory) {
    if (typeof define === "function" && define.amd) {
        define([], factory);
    } else if (typeof module === "object" && module.exports) {
        module.exports = factory();
    } else {
        factory();
    }
}(function () {
    'use strict';
    if (!Array.prototype.some) {
        Array.prototype.some = function (fun, thisArg) {
            if (this == null) {
                throw new TypeError('Array.prototype.some called on null or undefined');
            }

            if (typeof fun !== 'function') {
                throw new TypeError();
            }

            var t = Object(this);
            var len = t.length >>> 0;

            for (var i = 0; i < len; i++) {
                if (i in t && fun.call(thisArg, t[i], i, t)) {
                    return true;
                }
            }

            return false;
        };
    }
    if (!String.prototype.startsWith) {
        Object.defineProperty(String.prototype, 'startsWith', {
            value: function (search, rawPos) {
                var pos = rawPos > 0 ? rawPos | 0 : 0;
                return this.substring(pos, pos + search.length) === search;
            }
        });
    }
    if (!String.prototype.convertToRGB) {
        Object.defineProperty(String.prototype, 'convertToRGB', {
            value: function (opacity) {
                var color = this.trim();
                if (color.startsWith('#')) {
                    color = color.substring(1);                    
                }
                var aRgbHex = color.match(/.{1,2}/g);
                var r = parseInt(aRgbHex[0], 16);
                var g = parseInt(aRgbHex[1], 16);
                var b = parseInt(aRgbHex[2], 16);
                if (typeof opacity !== 'undefined' && opacity !== null) {
                    return "rgba(" + r + "," + g + "," + b + "," + opacity + ")";
                }
                return "rgb(" + r + "," + g + "," + b + ")";
                return this.substring(pos, pos + search.length) === search;
            }
        });
    }
    if (!String.prototype.isColor) {
        Object.defineProperty(String.prototype, 'isColor', {
            value: function () {                
                return this.startsWith('#') ||
                    this.startsWith('rgb(') ||
                    this.startsWith('rgba(') ||
                    this.startsWith('hsl(') ||
                    this.startsWith('hsla(');
            }
        });
    }
}));


