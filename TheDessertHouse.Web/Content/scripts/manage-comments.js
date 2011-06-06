$(function () {

    $(".edit-comment").click(function () {
        var commentId = $(this).attr("meta:id");
        var commentBlock = $("#comment-" + commentId);
        var bodyText = commentBlock.find(".body").text();
        var nameText = commentBlock.find(".name").text();

        var tmplVal = [{ commentId: commentId, bodyText: bodyText}];
        $("#display-comment-" + commentId).hide("slow");
        $("#editArticleSection").tmpl(tmplVal).appendTo("#comment-" + commentId).slideDown("slow");


        $(".update").click(function () {
            var commentId = $(this).attr("meta:id");
            var commentText = $("#body-" + commentId).val();
            var parm = { commentId: commentId, body: commentText };
            var jsonedComment = JSON.stringify(parm);
            $.ajax({
                url: commentEditUrl,
                data: jsonedComment,
                success: function (response) {
                    $("#form-" + response.commentId).hide("slow");
                    $("#form-" + response.commentId).remove();
                    $("#display-comment-" + response.commentId).text(response.body);
                    $("#display-comment-" + response.commentId).show("slow");

                },
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                error: function (err) {
                    alert("Your changes could not be posted. Please try again later");
                }
            })

        })

        $(".cancel").click(function () {
            var commentId = $(this).attr("meta:id");
            $("#form-" + commentId).hide("slow");
            $("#form-" + commentId).remove();
            $("#display-comment-" + commentId).show("slow");
        })
    })

    $(".remove-comment").click(function () {
        var commentId = $(this).attr("meta:id");
        $.post(commentRemoveUrl, { commentId: commentId }, function (data) {
            $("#comment-" + data).fadeOut("slow", function () { $(this).remove(); });
            var commentsLeft = $("[div[id^='comment']").size();
            if ($("#lnkPrevious").size() > 0 && commentsLeft === 0) {
                //reload previous
                
            }
            else if (commentsLeft < 5 && $("#lnkNext").size() > 0 && $("#lnkPrevious").size() > 0) {
                //var pageNum = location.href.substr(location.href.slice(0,location.href.indexOf('#')).lastIndexOf('/')+1,1);
                
            }
            else if (commentsLeft < 5 && $("#lnkNext").size() > 0) {
                //load next
            }
        }, "json");
    })

})