$(function () {

    $(".delete-department-button").click(function () {
        var deptId = $(this).attr("meta:id");
        $.post(deleteUrl, { departmentId: deptId }, function (response) {
            if (response.Error != undefined) {
                alert("The department could not be deleted at this time. Please try again later");
                return;
            }
            $("#admin-" + response.Id).remove();
        }, "json");
    })

    $(".delete-product-button").click(function () {
        var productId = $(this).attr("meta:id");
        $.post(deleteUrl, { productId: productId }, function (response) {
            if (response.Error != undefined) {
                alert("The product could not be deleted at this time. Please try again later");
                return;
            }
            $("#admin-" + response.Id).remove();
        }, "json");
    })


    $(".delete-shipping-method-button").click(function () {
        var shippingId = $(this).attr("meta:id");
        $.post(deleteShippingUrl, { shippingId: shippingId }, function (response) {
            if (response.Error != undefined) {
                alert("The shipping method could not be deleted at this time. Please try again later");
                return;
            }
            $("#shipping-method-" + response.Id).remove();
        }, "json");
    })

    $("#btnShippingMethodAdd").click(function () {
        var method = $("#txtShippingTitle").val();
        var price = $("#txtShippingPrice").val();

        if (method == '' || price == '') {
            alert("Fill in all details");
            return;
        }
        $.post(addShippingUrl, { shippingMethod: method, price: price }, function (response) {
            if (response.Error != undefined) {
                alert("The shipping method could not be added at this time. Please try again later");
                return;
            }
            
            $("#shippingRowTmpl").tmpl(response).appendTo("#methods");
            $("#txtShippingTitle").val("");
            $("#txtShippingPrice").val("");
        }, "json");
    })
    
})