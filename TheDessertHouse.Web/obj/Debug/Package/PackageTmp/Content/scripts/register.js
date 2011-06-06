$("form").validate({
    onfocusout: false,
    onkeyup: false,
    rules: {
        UserName: { required: true },
        Password: { required: true },
        ConfirmPassword: { required: true },
        Email: { required: true },
        SecretQuestion: { required: true },
        HintAnswer: { required: true }
    },
    messages: {
        UserName: { required: "*" },
        Password: { required: "*" },
        ConfirmPassword: { required: "*" },
        Email: { required: "*" },
        SecretQuestion: { required: "*" },
        HintAnswer: { required: "*" }
    },
    errorClass: "input-validation-error",
    errorElement: "",
    highlight: function (element, errorClass, validClass) {
        $(element).addClass(errorClass).removeClass(validClass);
        $("div.errorClass").text("Please correct the highlighted fields and try again")
    }
})   

