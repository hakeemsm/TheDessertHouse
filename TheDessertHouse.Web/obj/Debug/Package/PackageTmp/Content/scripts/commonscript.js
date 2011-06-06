var validationUrl;
var __editorConfig = {
    mode: "textareas",
    theme: "advanced",
    plugins: "advhr,advimage,advlink,contextmenu,inlinepopups,media,paste,safari,spellchecker,xhtmlxtras",

    theme_advanced_toolbar_location: "top",
    theme_advanced_toolbar_align: "left",
    theme_advanced_statusbar_location: "bottom",
    theme_advanced_resizing_use_cookie: false,
    theme_advanced_resize_horizontal: false,
    theme_advanced_resizing: true,
    theme_advanced_resizing_min_height: 200,

    convert_urls: false,

    gecko_spellcheck: true,
    dialog_type: "modal",

    paste_auto_cleanup_on_paste: true,
    paste_convert_headers_to_strong: true,
    paste_strip_class_attributes: "all"
};

function ShowMessage(input, message) {    
    DisplayMessage(true, input, "input-info", message);
}

function DisplayMessage(display, input, css, text) {
    var message = $(input).parent().children("span.input-message");
}