$("form").validate({
    onfocusout: false,
    onkeyup: false,
    rules: {        
        Password: { required: true },
        ChangePassword: { required: true },
        ConfirmPassword: { required: true }
    },
    messages: {        
        Password: { required: "*" },
        ChangePassword: { required: "*" },
        ConfirmPassword: { required: "*" }
    },
    errorClass: "input-validation-error",
    errorElement: "",
    highlight: function (element, errorClass, validClass) {
        $(element).addClass(errorClass).removeClass(validClass);
        $("div.errorClass").text("Please correct the highlighted fields and try again")
    }

})