$(function () {

    $("#btnSrchUser").click(function () {
        if ($("#searchInput").val() === '') {
            alert("Enter a user name/email to search for");
            return;
        }
        var pageNum = parseInt($("input#pageNum").val());
        if (pageNum > 1) {
            $("input#pageNum").val(1);
        }
        $("#frmSearchUsers").submit();
    })

    $("#lnkNext").click(function () {
        var pageNum = parseInt($("input#pageNum").val());
        $("input#pageNum").val(pageNum + 1);
        $("#frmSearchUsers").submit();
    })

    $("#lnkPrevious").click(function () {
        var pageNum = parseInt($("input#pageNum").val());
        $("input#pageNum").val(pageNum - 1);
        $("#frmSearchUsers").submit();
    })

    $("a#lnkDeleteUser").each(function (i) {
        $(this).click(function () {
            var userId = $(this).attr("userId");
            if (confirm("Are you sure you want to delete " + userId + "?")) {
                $.ajax({
                    url: "/Account/DeleteUser?userId=" + userId,
                    contentType: "json",
                    type: "post",
                    success: function (response) {
                        if (response.deleted) {
                            $('tr#' + response.userId).remove();
                        }
                        else {
                            alert("The user could not be deleted. Please try again later");
                        }
                    },
                    error: function () {
                        alert("The user could not be deleted. Please try again later");
                    }
                });
            }
        })

        $(this).mouseover(function () {
            $(this).attr("style", "cursor: pointer");
        })
    })

})