
$(function () {
    var bodyEditor;
    $("form").validate({

        onfocusout: false,
        onkeyup: false,
        rules: {
            Title: {
                required: true,
                minlength: 4
            },
            Abstract: {
                required: true
            },
            Body: {
                required: true
            },
            ReleaseDate: {
                required: true
            },
            ExpireDate: {
                required: true
            }
        },
        messages: "Field is required",
        errorClass: "input-error"

    })

    if (userIsEditor === "False") {
        $("#Title").rules("add", {
            remote: function () {
                return {
                    url: validationUrl,
                    type: "get",
                    data: {
                        Title: function () {
                            return $("#Title").val();
                        }
                    },
                    dataFilter: function (response) {
                        var userResp = true;
                        var resp = JSON.parse(response);
                        if (resp.categoryExists === true) {
                            userResp = confirm("Title already exists, replace?");
                        }
                        //                    $("#CategoryExists").val(resp.categoryExists);
                        return userResp;

                    }
                }
            }

        })
    }




    $("#ReleaseDateField").datepicker({ defaultDate: "+1", maxDate: "10" });
    $("#ExpireDateField").datepicker({ defaultDate: "+90", maxDate: "+180" });


    bodyEditor = new tinymce.Editor("Body", __editorConfig);
    bodyEditor.onChange.add(function (ed) { bodyEditor.save(); });
    bodyEditor.onClick.add(function (ed) { ShowMessage("#body", "Enter the body of your article."); });
    bodyEditor.render();
})