/**
 * @name		jQuery Stopwatch Plugin
 * @author		Victor Antofica
 * @version 	1.0
 * @url			http://victorantos.com/
 * @license		MIT License
 */

(function ($) {

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

})(jQuery);