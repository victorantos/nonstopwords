(function () {

    // Localize jQuery variable
    var jQuery;
    var ko;
    var gameHost2 = 'http://nonstopwords.com';// 
    var gameHost =  'http://localhost:53012';
    /******** Load jQuery if not present *********/
    if (window.jQuery === undefined || window.jQuery.fn.jquery < '1.6.2') {
        var script_tag = document.createElement('script');
        script_tag.setAttribute("type", "text/javascript");
        script_tag.setAttribute("src",
            "http://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js");
        if (script_tag.readyState) {
            script_tag.onreadystatechange = function () { // For old versions of IE
                if (this.readyState == 'complete' || this.readyState == 'loaded') {
                    scriptLoadHandler();
                }
            };
        } else { // Other browsers
            script_tag.onload = scriptLoadHandler;
        }
        // Try to find the head, otherwise default to the documentElement
        (document.getElementsByTagName("head")[0] || document.documentElement).appendChild(script_tag);
    } else {
        // The jQuery version on the window is the one we want to use
        jQuery = window.jQuery;
        jQuery.noConflict();
        main();
    }

    if (window.ko === undefined) {
        //|| window.ko.version < '2.1.1'
        var script_tag = document.createElement('script');
        script_tag.setAttribute("type", "text/javascript");
        script_tag.setAttribute("src",
            gameHost + "/scripts/knockout-2.3.0.js");
        if (script_tag.readyState) {
            script_tag.onreadystatechange = function () { // For old versions of IE
                if (this.readyState == 'complete' || this.readyState == 'loaded') {
                    //scriptLoadHandler();
                }
            };
        }
        (document.getElementsByTagName("head")[0] || document.documentElement).appendChild(script_tag);
    }
    else {
        ko = window.ko;

    }

    /******** Called once jQuery has loaded ******/
    function scriptLoadHandler() {
        // Restore $ and window.jQuery to their previous values and store the
        // new jQuery in our local jQuery variable
        jQuery = window.jQuery;
      
        // Call our main function
       
       
         
        main();
    }

    /******** Our main function ********/
    function main() {

        /**** LOAD jQuery UI ****/
        var script_tag = document.createElement('script');
        script_tag.setAttribute("type", "text/javascript");
        script_tag.setAttribute("src",
            "http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.11/jquery-ui.min.js");
        if (script_tag.readyState) {
            script_tag.onreadystatechange = function () { // For old versions of IE
                if (this.readyState == 'complete' || this.readyState == 'loaded') {
                    jQuery.ui = window.jQuery.ui;
                }
            };
        }
        (document.getElementsByTagName("head")[0] || document.documentElement).appendChild(script_tag);
        /****** -/jqueryUI-- ***/
       

        jQuery(document).ready(function ($) {

            /******* Load HTML *******/
            var container = document.getElementById("nswGame");
            var el = document.createElement("div");
            if (container != null) {
                container.parentNode.insertBefore(el, container.nextSibling);
                container.style.display = 'none';
            }


            /******* Load CSS *******/
            var css_link = jQuery("<link>", {
                rel: "stylesheet",
                type: "text/css",
                href: gameHost + "/scripts/countdown/assets/stopwatch/jquery.stopwatch.css"
            });
            css_link.appendTo('head');

            /******* Load CSS *******/
            var css_link2 = jQuery("<link>", {
                rel: "stylesheet",
                type: "text/css",
                href: gameHost + "/wordsearch.css"
            });
            css_link2.appendTo('head');

            /******* Load CSS *******/
            //var css_linkJqUi = jQuery("<link>", {
            //    rel: "stylesheet",
            //    type: "text/css",
            //    href: gameHost + "/content/themes/base/jquery.ui.all.css"
            //});
            //css_linkJqUi.appendTo('head');
 
            var widgetId = -1;
            if (container && container.getAttribute("data-widget-id"))
                widgetId = container.getAttribute("data-widget-id");
            var widget_url = gameHost2 + "/Scripts/wordsearch_data/" + widgetId + "?jsoncallback=?";

            //if (window.jQuery === undefined) {
            //    window.jQuery = jQuery;
            //    window.$ = jQuery;
            //}
            jQuery.getJSON(widget_url, function (data) {
                jQuery(el).html(data.html);
            });


            // Number of seconds in every time division
            var days = 24 * 60 * 60,
                hours = 60 * 60,
                minutes = 60;
            var isActive = false;
            var t;
            var left, d, h, m, s, positions;
            var options;
            function updateDuo(minor, major, value) {

                switchDigit(positions.eq(minor), Math.floor(value / 10) % 10);
                switchDigit(positions.eq(major), value % 10);
            }

            function tick() {

                // Time left
                options.timestamp += 1000;
                left = Math.floor((options.timestamp) / 1000);

                if (left < 0) {
                    left = 0;
                }

                // Number of days left
                d = Math.floor(left / days);
                updateDuo(0, 1, d);
                //left += d * days;

                // Number of hours left
                h = Math.floor(left % days);
                updateDuo(2, 3, h);
                //left += h * hours;

                // Number of minutes left
                m = Math.floor(left / minutes);
                updateDuo(4, 5, m);
                // left += m * minutes;

                // Number of seconds left
                s = Math.floor(left % minutes);
                updateDuo(6, 7, s);

                // Calling an optional user supplied callback
                options.callback(d, h, m, s);

                // Scheduling another call of this function in 1s

                t = setTimeout(tick, 1000);


            };

            var methods = {
                init: function (prop) {

                    if (!isActive) {
                        options = jQuery.extend({
                            callback: function () { },
                            timestamp: 0
                        }, prop);


                        // Initialize the plugin
                        init1(this, options);
                        positions = this.find('.position');

                        // tick();
                        isActive = false;
                    }

                },
                start: function () {
                    if (!isActive) {

                        tick();
                        isActive = true;
                    }
                },
                pause: function () {
                    //alert('pause');
                },
                cont: function () {
                    //alert('continue');
                },
                clear: function (prop) {
                    clearTimeout(t);
                    isActive = false;

                    var cl = jQuery.extend({
                        callback: function () { }
                    }, prop);
                    options.timestamp = 0;

                    cl.callback(d, h, m, s);
                }
            };

            // Creating the plugin
            jQuery.fn.stopwatch = function (method) {

                // Method calling logic
                if (methods[method]) {
                    return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
                } else if (typeof method === 'object' || !method) {
                    return methods.init.apply(this, arguments);
                } else {
                    jQuery.error('Method ' + method + ' does not exist on jQuery.stopwatch');
                }

                return this;
            };

            function init1(elem, options) {
                elem.addClass('countdownHolder');

                // Creating the markup inside the container
                jQuery.each(['Days', 'Hours', 'Minutes', 'Seconds'], function (i) {
                    jQuery('<span class="count' + this + '">').html(
                        '<span class="position">\
					<span class="digit static">0</span>\
				</span>\
				<span class="position">\
					<span class="digit static">0</span>\
				</span>'
                    ).appendTo(elem);

                    if (this != "Seconds") {
                        elem.append('<span class="countDiv countDiv' + i + '"></span>');
                    }
                });
            }

            // Creates an animated transition between the two numbers
            function switchDigit(position, number) {

                var digit = position.find('.digit')

                if (digit.is(':animated')) {
                    return false;
                }

                if (position.data('digit') == number) {
                    // We are already showing this number
                    return false;
                }

                position.data('digit', number);

                var replacement = jQuery('<span>', {
                    'class': 'digit',
                    css: {
                        top: '-2.1em',
                        opacity: 0
                    },
                    html: number
                });

                // The .static class is added when the animation
                // completes. This makes it run smoother.

                digit
                    .before(replacement)
                    .removeClass('static')
                    .animate({ top: '2.5em', opacity: 0 }, 'fast', function () {
                        digit.remove();
                    })

                replacement
                    .delay(100)
                    .animate({ top: 0, opacity: 1 }, 'fast', function () {
                        replacement.addClass('static');
                    });
            }
        });

    }
})();
