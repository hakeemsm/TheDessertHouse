$(function () {
    $("[id=lnkRemoveCategory]").each(function (i) {
        $(this).click(function () {
            var title = $(this).attr("categoryTitle");
            var catId = $(this).attr("categoryId");

            var msg = "Are you sure you want to delete category " + title + "?";
            if (confirm(msg)) {
                $.ajax({
                    url: removeCategoryUrl,
                    data: { categoryId: catId },
                    type: "post",
                    dataType: "json",
                    success: function (response) {
                        if (response.deleted) {
                            $("#catDiv" + response.catId).hide("slow");
                            $("#catDiv" + response.catId).remove();
                        }
                        else {
                            alert("The category could not be deleted at this tume. Please try again later");
                        }
                    },
                    error: function (errObj) {
                        alert("The category could not be deleted at this tume. Please try again later");
                    }


                });
            };
        });

        $(this).mouseover(function () {
            $(this).attr("style", "cursor: pointer");
        });
    });
})