$(function () {
    $(".remove-newsletter").click(function () {
        var subject = $(this).attr("subject");
        var nlId = $(this).attr("meta:id");
        var delAff = confirm("Are you sure you want to delete Newsletter " + subject + "?");
        if (delAff == true) {
            $.ajax({
                type: "post",
                url: delNewsletterUrl,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ newsletterId: nlId }),
                success: function (response) {
                    if (response.IsDeleted) {
                        $("#newsletter-" + response.Id).remove();
                    }
                },
                error: function (xhr) {
                    alert("Newsletter could not be deleted at this time. Please try again later");
                }

            })
        }
    })
})
function UpdateStatus() {
    if (pendingUpdate == 'true') {
        setInterval(getNewsletter(), 4000);
    }

}

function getNewsletter() {
    $.ajax({
        type: "GET",
        url: updtStatusUrl,
        dataType: "html",
        sucess: function (result) {
            var domElement = $(result);
            $("#Newsletter_Status_Table").replaceWith(domElement);
            pendingUpdate = false;
        }
    });
}