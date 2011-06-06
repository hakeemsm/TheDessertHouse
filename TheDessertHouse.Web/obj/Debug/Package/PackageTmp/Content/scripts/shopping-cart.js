$(function () {
    if ($("#tblCart").length == 0) {
        $("#emptyCartMsgDiv").show();
    }

    $("#SelectedShippingMethod").change(function () {
        var id = $(this).val();
        $.get(updateShippingUrl, { shippingId: id }, function (response) {
            $("#shippingPrice").text(response.ShippingPrice);
            $("#total").text(response.Total);
        }, "json");
    })

    $(".delete-item-button").click(function () {
        var id = $(this).attr("meta:id");
        $.post(deleteItemUrl, { productId: id }, function (response) {
            if (response.Id == undefined) {
                alert("An error occurred. Please try again after a few minutes");
                return;
            }
            $("#item-" + response.Id).remove();
            if (response.Total == "0") {
                $("#tblCart").remove();
                $("#emptyCartMsgDiv").show();
            }
            else {
                $("#total").text(response.Total);
            }
        }, "json");
    })
})