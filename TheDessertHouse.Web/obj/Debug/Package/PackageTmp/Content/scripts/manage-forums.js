$(function () {
    

    $(".remove-forum").click(function () {

        var title = $(this).attr("forumTitle");
        var forumId = $(this).attr("forumId");

        var msg = "Are you sure you want to delete forum " + title + "?";
        if (confirm(msg)) {
            $.ajax({
                url: removeForumUrl,
                data: { forumId: forumId },
                type: "post",
                dataType: "json",
                success: function (response) {
                    if (response.deleted) {
                        $("#forum-" + response.Id).hide("slow");
                        $("#forum-" + response.Id).remove();
                    }
                    else {
                        alert("The forum could not be deleted at this tume. Please try again later");
                    }
                },
                error: function (errObj) {
                    alert("The forum could not be deleted at this tume. Please try again later");
                }


            });
        };


        //        $(this).mouseover(function () {
        //            $(this).attr("style", "cursor: pointer");
        //        });
    });

    $(".approve").click(function () {
        var postId = $(this).attr("meta:id");
        $.post(postApproveUrl, { postId: postId, approved: true }, handleResult, "json");
    })

    $(".remove").click(function () {
        var postId = $(this).attr("meta:id");
        $.post(postRemoveUrl, { postId: postId }, handleResult, "json");
    })

    function handleResult(result) {
        if (result.error != undefined) {
            alert(result.error);
            return;
        }
        $("#post-" + result.Id).fadeOut("slow", function () {
            $(this).remove();
        })
    }
})