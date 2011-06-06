$(function () {
    $("#btnUser").click(function () {
        var user = $("#user").val();
        $.get(getQuestion, { userName: user }, function (data) {
            $("#userNameDiv").hide();
            $("#answerDiv").show();
            $("#userName").val(data.userName);
            $("#spnQtn").text(data.SecretQuestion);
        }, "json");
    })

    $("#frmForgotPwd").validate({
        onfocusout: false,
        onkeyup: false,
        rules: {
            userName: {
                required: true
            },
            hintAnswer: {
                required: true
            }
        },
        messages: {
            userName: {
                required: "* Please enter a your user name"
            },
            hintAnswer: {
                required: "* Please enter the answer"
            }
        },
        errorClass: "input-error",
        highlight: function (element, errorClass, validClass) {
            $("#userName").focus();
        }
    })
})