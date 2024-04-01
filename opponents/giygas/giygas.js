if (!giygas) var giygas = (function (root) {

    /* Polyfill String.prototype.repeat */
    if (!String.prototype.repeat) {
        String.prototype.repeat = function (count) {
            'use strict';
            if (this == null)
                throw new TypeError('can\'t convert ' + this + ' to object');

            var str = '' + this;
            // To convert string to integer.
            count = +count;
            // Check NaN
            if (count != count)
                count = 0;

            if (count < 0)
                throw new RangeError('repeat count must be non-negative');

            if (count == Infinity)
                throw new RangeError('repeat count must be less than infinity');

            count = Math.floor(count);
            if (str.length == 0 || count == 0)
                return '';

            // Ensuring count is a 31-bit integer allows us to heavily optimize the
            // main part. But anyway, most current (August 2014) browsers can't handle
            // strings 1 << 28 chars or longer, so:
            if (str.length * count >= 1 << 28)
                throw new RangeError('repeat count must not overflow maximum string size');

            var maxCount = str.length * count;
            count = Math.floor(Math.log(count) / Math.log(2));
            while (count) {
                str += str;
                count--;
            }
            str += str.substring(0, maxCount - str.length);
            return str;
        }
    }

    var exports = {};

    root.Sentry.setTag('giygas-loaded', true);
    root.Sentry.addBreadcrumb({
        category: 'giygas',
        message: 'Initializing giygas.js...',
        level: 'info'
    });

    function loadScript(scriptName) {
        console.log("[Giygas] Loading module: " + scriptName);

        const script = document.createElement('script');
        script.src = scriptName;
        script.async = false;

        script.addEventListener('load', function () {
            console.log("[Giygas] Loaded module: " + scriptName);
        });

        script.addEventListener('error', function () {
            console.error("[Giygas] Error loading " + scriptName);
        });

        root.document.head.appendChild(script);
    }

    function reportException(prefix, e) {
        console.log("[Giygas] Exception swallowed " + prefix + ": ");

        Sentry.withScope(function (scope) {
            scope.setTag("giygas-error", true);
            scope.setExtra("where", prefix);
            captureError(e);
        });

        captureError(e);
    }
    exports.reportException = reportException;

    function registerBehaviourCallback(name, func) {
        exports[name] = function () {
            try {
                return func.apply(null, arguments);
            } catch (e) {
                reportException('in behaviour callback ' + name, e);
            }
        };
    }

    exports.registerBehaviourCallback = registerBehaviourCallback;

    /* Set up hooks system... */
    var registeredHooks = {};
    exports.hooks = registeredHooks;

    function registerHook(hookedID, hookType, hookFunc) {
        registeredHooks[hookedID][hookType].push(hookFunc);
        return hookFunc;
    }
    exports.registerHook = registerHook;

    function unregisterHook(hookedID, hookType, hookFunc) {
        var hookList = registeredHooks[hookedID][hookType];
        var idx = hookList.indexOf(hookFunc);
        if (idx < 0) return;

        hookList.splice(idx, 1);
    }
    exports.unregisterHook = unregisterHook;

    function hookWrapper(func_id) {
        registeredHooks[func_id] = {
            'pre': [],
            'instead': [],
            'post': []
        };

        var original_function = root[func_id];
        return function () {
            /* Prevent the original function from firing if any pre-hook
             * returns true.
             */
            var wrapper_args = arguments;

            if (registeredHooks[func_id].instead[0]) {
                try {
                    return registeredHooks[func_id].instead[0].apply(null, arguments);
                } catch (e) {
                    reportException("in instead-" + func_id + " hooks", e);
                }
            } else {
                try {
                    if (registeredHooks[func_id].pre.some(function (hook) {
                            return hook.apply(null, wrapper_args);
                        })) {
                        return;
                    };
                } catch (e) {
                    reportException("in pre-" + func_id + " hooks", e);
                }

                var retval = original_function.apply(null, arguments);

                try {
                    registeredHooks[func_id].post.forEach(function (hook) {
                        hook.apply(null, wrapper_args);
                    });
                } catch (e) {
                    reportException("in post-" + func_id + " hooks", e);
                } finally {
                    return retval;
                }
            }
        }
    }

    root.updateMainButtonExchangeLabel = hookWrapper('updateMainButtonExchangeLabel');
    root.restartGame = hookWrapper('restartGame');

    var shouldPray = false;

    function showPrayText () {
        if (shouldPray && $mainButton.html() === 'Keep all') {
            $mainButton.html('Pray');
        }
    }
    registerHook('updateMainButtonExchangeLabel', 'post', showPrayText);

    function startPraying () {
        shouldPray = true;
    }
    exports.startPraying = startPraying;

    function stopPraying () {
        shouldPray = false;
    }
    registerHook('restartGame', 'post', stopPraying);
    exports.stopPraying = stopPraying;

    return exports;
}(this));