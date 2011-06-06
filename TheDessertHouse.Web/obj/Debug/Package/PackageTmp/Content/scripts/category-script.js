$(function () {

    $("#frmCreateCategory").validate({
        onfocusout: false,
        onkeyup: false,
        rules: {
            Title: {
                required: true,
                minlength: 4

            },
            ImageUrl: {
                required: true
            },
            Description: {
                required: true
            }
        },
        messages: {
            Title: {
                required: "* Please enter a title for the category",
                minlength: "* At least 4 characters are required for the category",
                remote: ""
            },
            ImageUrl: {
                required: "* Please enter a URL for the image"
            },
            Description: {
                required: "* Please enter the category description"
            }
        },
        errorClass: "input-error",
        highlight: function (element, errorClass, validClass) {
            var id = $("#Title").attr("id");
            $("#Title").focus();
        }

    })



    if (categoryExists === "False") {
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
                        if (resp === true) {
                            userResp = confirm("Title already exists, replace?");
                        }
                        //$("#CategoryExists").val(resp.categoryExists);
                        return userResp;

                    }
                }
            }

        })
    }

    $("#Importance").val("");

})


