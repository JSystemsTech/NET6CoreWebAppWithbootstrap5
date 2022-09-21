/*!
 * chartjs.unobtrusive v1.0.0
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
    var labelColor = $.GetThemeColor('chart-label');
    var gridColor = $.GetThemeColor('chart-grid');
    var bar = function (options) {
        var canvas = this;
        options = $.extend(true, {
            color: null,
            title: true,
            titleColor: labelColor,
            xLabel:true,
            xLabelColor: labelColor,
            xTicks: true,
            xTicksColor: labelColor,
            xGrid: true,
            xGridColor: gridColor,
            xGridBorderColor: labelColor,
            yLabel: true,
            yLabelColor: labelColor,
            yTicks: true,
            yTicksColor: labelColor,
            yGrid: true,
            yGridColor: gridColor,
            yGridBorderColor: labelColor,
            theme: 'primary'
        }, canvas.data());
        options = $.extend(true, options, canvas.data());
        var form = $(options.form);

        if (options.color === null) {
            options.color = $.GetThemeColor(options.theme);
        }
        

        var chart = null;
        var drawChart = function () {

            var params = form.serializeArray();
            form.bsFormDisable();
            if (chart !== null) {
                chart.destroy();
            }
            $.post(form.attr('action'), params).done(function (config) {
                chart = new Chart(canvas, {
                    type: 'bar',
                    data: {
                        labels: config.labels,
                        datasets: [{
                            data: config.data,
                            backgroundColor: options.color.convertToRGB(0.7),
                            borderColor: options.color,
                            borderWidth: 2
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        scales: {
                            x: {
                                title: {
                                    display: options.xLabel,
                                    text: config.xLabel,
                                    font: {
                                        size: 14,
                                        weight: 'bold'
                                    },
                                    color: options.xLabelColor
                                },
                                ticks: {
                                    display: options.xTicks,
                                    font: {
                                        size: 10,
                                        weight: 'bold'
                                    },
                                    color: options.xTicksColor
                                },
                                grid: {
                                    display: options.xGrid,
                                    borderColor: options.xGridBorderColor,
                                    color: options.xGridColor
                                }
                            },
                            y: {
                                type: 'linear',
                                title: {
                                    display: options.yLabel,
                                    text: config.yLabel,
                                    font: {
                                        size: 14,
                                        weight: 'bold'
                                    },
                                    color: options.yLabelColor
                                },
                                ticks: {
                                    display: options.yTicks,
                                    beginAtZero: true,
                                    callback: function (value) {
                                        if (config.yTick === 'int') {
                                            if (value % 1 === 0) {
                                                return value;
                                            }
                                        }
                                        return value;
                                    },
                                    font: {
                                        size: 10,
                                        weight: 'bold'
                                    },
                                    color: options.yTicksColor
                                },
                                grid: {
                                    display: options.yGrid,
                                    borderColor: options.yGridBorderColor,
                                    color: options.yGridColor
                                }
                            }
                        },
                        plugins: {
                            title: {
                                display: options.title,
                                text: config.title,
                                font: {
                                    size: 16,
                                    weight: 'bold'
                                },
                                color: options.titleColor
                            },
                            legend: {
                                display: false
                            }
                        }
                    }
                });
                form.bsFormEnable();
            });
        };
        drawChart();
        form.on('change', drawChart);
    };

    var line = function (options) {
        var canvas = this;
        options = $.extend(true, {
            color: null,
            title: true,
            titleColor: labelColor,
            xLabel: true,
            xLabelColor: labelColor,
            xTicks: true,
            xTicksColor: labelColor,
            xGrid: true,
            xGridColor: gridColor,
            xGridBorderColor: labelColor,
            yLabel: true,
            yLabelColor: labelColor,
            yTicks: true,
            yTicksColor: labelColor,
            yGrid: true,
            yGridColor: gridColor,
            yGridBorderColor: labelColor,
            fill: false,
            fillColor: null,
            fillOpacity: 0.5,
            tension: 0.4,
            theme: 'primary'
        }, canvas.data());
        options = $.extend(true, options, canvas.data());
        var form = $(options.form);

        if (options.color === null) {
            options.color = $.GetThemeColor(options.theme);
        }
        if (options.fillColor === null) {
            options.fillColor = $.GetThemeColor(options.theme).convertToRGB(options.fillOpacity);
        }
        

        var chart = null;
        var drawChart = function () {
            
            var params = form.serializeArray();
            form.bsFormDisable();
            if (chart !== null) {
                chart.destroy();
            }
            $.post(form.attr('action'), params).done(function (config) {
                chart = new Chart(canvas, {
                    type: 'line',
                    data: {
                        labels: config.labels,
                        datasets: [{
                            data: config.data,
                            borderColor: options.color,
                            borderWidth: 2,
                            backgroundColor: options.fillColor,
                            fill: options.fill,
                            tension: options.tension
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        scales: {
                            x: {
                                title: {
                                    display: options.xLabel,
                                    text: config.xLabel,
                                    font: {
                                        size: 14,
                                        weight: 'bold'
                                    },
                                    color: options.xLabelColor
                                },
                                ticks: {
                                    display: options.xTicks,
                                    font: {
                                        size: 10,
                                        weight: 'bold'
                                    },
                                    color: options.xTicksColor
                                },
                                grid: {
                                    display: options.xGrid,
                                    borderColor: options.xGridBorderColor,
                                    color: options.xGridColor
                                }
                            },
                            y: {
                                type: 'linear',
                                title: {
                                    display: options.yLabel,
                                    text: config.yLabel,
                                    font: {
                                        size: 14,
                                        weight: 'bold'
                                    },
                                    color: options.yLabelColor
                                },
                                ticks: {
                                    display: options.yTicks,
                                    beginAtZero: true,
                                    callback: function (value) {
                                        if (config.yTick === 'int') {
                                            if (value % 1 === 0) {
                                                return value;
                                            }
                                        }
                                        return value;
                                    },
                                    font: {
                                        size: 10,
                                        weight: 'bold'
                                    },
                                    color: options.yTicksColor
                                },
                                grid: {
                                    display: options.yGrid,
                                    borderColor: options.yGridBorderColor,
                                    color: options.yGridColor
                                }
                            }
                        },
                        plugins: {
                            title: {
                                display: options.title,
                                text: config.title,
                                font: {
                                    size: 16,
                                    weight: 'bold'
                                },
                                color: options.titleColor
                            },
                            legend: {
                                display: false
                            }
                        }
                    }
                });
                form.bsFormEnable();
            });
        };
        drawChart();
        form.on('change', drawChart);
    };

    var pieDoughnut = function (options, canvas, type) {
        options = $.extend(true, {
            title: true,
            titleColor: labelColor,
            opacity: 0.8
        }, canvas.data());
        options = $.extend(true, options, canvas.data());
        var form = $(options.form);

        
        var chart = null;
        var drawChart = function () {

            var params = form.serializeArray();
            form.bsFormDisable();
            if (chart !== null) {
                chart.destroy();
            }
            $.post(form.attr('action'), params).done(function (config) {
                var borderColor = config.borderColor || [];
                
                if (borderColor.length === 0) {
                    borderColor = $.map(config.backgroundColor, function (bg) {
                        
                        if (!bg.isColor()) {
                            return $.GetThemeColor(bg);
                        }
                        return bg;
                    });
                    
                }
                chart = new Chart(canvas, {
                    type: type,
                    data: {
                        labels: config.labels,
                        datasets: [{
                            data: config.data,
                            backgroundColor: $.map(config.backgroundColor, function (bg) {
                                if (!bg.isColor()) {
                                    return $.GetThemeColor(bg).convertToRGB(options.opacity);
                                }
                                return bg;
                            }),
                            borderColor: borderColor,
                            borderWidth: 2,
                            hoverOffset: 5
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            title: {
                                display: options.title,
                                text: config.title,
                                font: {
                                    size: 16,
                                    weight: 'bold'
                                },
                                color: options.titleColor
                            },
                            legend: {
                                display: false
                            }
                        }
                    }
                });
                form.bsFormEnable();
            });
            
        };
        drawChart();
        form.on('change', drawChart);
        form.find('button[refresh]').on('click', drawChart);
    };

    $.fn.barChart = bar;
    $('canvas[chart="bar"]').each(function (i, el) { $(el).barChart(); });

    $.fn.lineChart = line;
    $('canvas[chart="line"]').each(function (i, el) { $(el).lineChart(); });

    $.fn.pieChart = function (options) { return pieDoughnut(options,this, 'pie') };
    $('canvas[chart="pie"]').each(function (i, el) { $(el).pieChart(); });

    $.fn.doughnutChart = function (options) { return pieDoughnut(options, this, 'doughnut') };
    $('canvas[chart="doughnut"]').each(function (i, el) { $(el).doughnutChart(); });

    $.RegisterOnLoadFunction(function (el) {
        el.find('canvas[chart="bar"]').each(function (i, canvas) { $(canvas).barChart(); });
        el.find('canvas[chart="line"]').each(function (i, canvas) { $(canvas).lineChart(); });
        el.find('canvas[chart="pie"]').each(function (i, canvas) { $(canvas).pieChart(); });
        el.find('canvas[chart="doughnut"]').each(function (i, canvas) { $(canvas).doughnutChart(); });
    });

}));


