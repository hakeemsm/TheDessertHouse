$(function () {
    $("form").validate({
        onfocusout: false,
        onkeyup: false,
        rules: {
            Subject: {
                required: true
            },
            HtmlBody: {
                required: true
            }
        },
        messages: "Field is required",
        errorClass: "input-error"
    })

    if (newsletterId != 0) {
        $("form.newsletter-create").attr("action", editAction);
    }

    

    var bodyEditor = new tinymce.Editor("HtmlBody", __editorConfig);
    bodyEditor.onChange.add(function (ed) { bodyEditor.save(); });
    bodyEditor.onClick.add(function (ed) { ShowMessage("#body", "Enter the body of your article."); });
    bodyEditor.render();
})