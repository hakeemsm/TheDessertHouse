$(function () {

    $("#frmDept").validate({
        onfocusout: false,
        onkeyup: false,
        rules: {
            Title: {
                required: true,
                minlength: 4

            }
        },
        messages: {
            Title: {
                required: "* Please enter a name for the department",
                minlength: "* At least 4 characters are required for the department name",
                remote: ""
            }
        },
        errorClass: "input-error",
        highlight: function (element, errorClass, validClass) {
            var id = $("#Title").attr("id");
            $("#Title").focus();
        }

    })



    if (departmentExists == "False") {
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
                            userResp = confirm("A department with the specified name already exists, replace?");
                        }

                        return userResp;

                    }
                }
            }

        })
    }

    $("#Importance").val("");    

})