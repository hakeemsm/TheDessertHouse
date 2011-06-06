/**
 * Created by .
 * User: Abdul-Hakeem
 * Date: 4/4/11
 * Time: 6:55 PM
 * To change this template use File | Settings | File Templates.
 */
(function ($) {
    $.getCookie = function (cookieName) {
        var cookieNameStr, cookieStart, cookieValue;
        cookieNameStr = encodeURIComponent(cookieName) + "=";
        cookieStart = document.cookie.indexOf(cookieNameStr);
        cookieValue = null;

        if (cookieStart > -1) {
            var cookieEnd = document.cookie.indexOf(";", cookieStart);
            if (cookieEnd == 1) cookieEnd = document.cookie.length;
            cookieValue = decodeURIComponent(document.cookie.substring(cookieStart + cookieNameStr.length, cookieEnd));
        }
        return cookieValue;

    },

    $.setCookie = function (name, value, expires, path, domain, secure) {
        var cookieText = encodeURIComponent(name) + "=" + encodeURIComponent(value);
        if (expires instanceof Date) cookieText += "; expires=" + expires.toGMTString();
        if (path) cookieText += "; path=" + path;
        if (domain) cookieText += "; domain=" + domain;
        if (secure) cookieText += "; secure";
        document.cookie = cookieText;
    }
})(jQuery);