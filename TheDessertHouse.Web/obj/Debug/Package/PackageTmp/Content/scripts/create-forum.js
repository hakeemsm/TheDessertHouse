$(function () {

    $("#frmForum").validate({
        onfocusout: false,
        onkeyup: false,
        rules: {
            Title: {
                required: true,
                minlength: 4

            },
            Description: {
                required: true
            }
        },
        messages: {
            Title: {
                required: "* Please enter a title for the forum",
                minlength: "* At least 4 characters are required for the forum title",
                remote: ""
            },
            Description: {
                required: "* Please enter a forum description"
            }
        },
        errorClass: "input-error",
        highlight: function (element, errorClass, validClass) {
            var id = $("#Title").attr("id");
            $("#Title").focus();
        }

    })



    if (forumExists === "False") {
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
                        if (resp.Exists === true) {
                            userResp = confirm("There is already a forum with the title " + resp.Title);
                        }
                        //$("#ForumExists").val(resp.categoryExists);
                        return userResp;

                    }
                }
            }

        })
    }

    $("#Importance").val("");
    if (forumExists == 'True') {
        $("#btnCreateForum").val("Update Forum");        
    }
})

