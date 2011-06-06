$(function () {

    $("#frmCreateProduct").validate({
        onfocusout: false,
        onkeyup: false,
        rules: {
            Title: {
                required: true,
                minlength: 4

            },
            SKU: {
                required: true,
                minlength: 4
            },
            UnitPrice: {
                required: true

            }
        },
        messages: {
            Title: {
                required: "* Please enter a name for the product",
                minlength: "* At least 4 characters are required for the product name",
                remote: ""
            },
            SKU: {
                required: "* Please enter the SKU for the product",
                minlength: "* At least 4 characters are required for the product SKU",
                remote: ""
            },
            UnitPrice: {
                required: "* Please enter the unit price for the product",
                remote: ""
            }
        },
        errorClass: "input-error",
        highlight: function (element, errorClass, validClass) {
            var id = $("#Title").attr("id");
            $("#Title").focus();
        }

    })



    if (productExists == "False") {
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
                            userResp = confirm("A product with that name already exists. replace?");
                            if (userResp) {
                                $("#ProductExists").val(true);
                            }
                        }

                        return userResp;

                    }
                }
            }

        })
        $("#UnitPrice").val("");
        $("#DiscountPercentage").val("");
        $("#UnitsInStock").val("");
    }
    else {
        $("#btnSave").val("Update Product");
        $("#frmCreateProduct").attr("action", updateUrl);
    }

    

})