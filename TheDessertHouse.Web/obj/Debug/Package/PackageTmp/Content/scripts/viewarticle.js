$(function () {
    $("#rate-button").click(function () {
        var articleId = $("#articleId").val();
        var rating = $("#rating").val();
        $.post(rateArticleUrl, { articleId: articleId, rating: rating }, function (value) {

            var imagePosition = "5";

            if (value <= 1.0)
                imagePosition = "1";
            else if (value <= 2.0)
                imagePosition = "2";
            else if (value <= 3.0)
                imagePosition = "3";
            else if (value <= 4.0)
                imagePosition = "4";

            $("#article-rating-value").replaceWith("<img src=\"/Content/images/ratings/" + imagePosition + "dessert.jpg\" alt=\"" + value + "\" />");
            $("form.rate-article :input").attr("disabled", "true");
            $("form.rate-article").append("Your rating has been applied!")
        }, 'json');
    })

    $("#btnAddComment").click(function () {
        var articleId = $("#articleId").val();
        var comment = { AddedBy: $("#AddedBy").val(),
            AddedByEmail: $("#AddedByEmail").val(),
            Body: $("#Body").val()
        };
        var jsonedComment = JSON.stringify(comment);
        $.ajax({
            type: "POST",
            url: commentPostUrl + "?articleId=" + articleId,
            data: jsonedComment,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var comment = [{ PosterName: data.AddedBy, CommentText: data.Body}];
                $("#new-comment").tmpl(comment).appendTo("#article-comments").slideDown("slow");
                $("#Body").val("");
            },
            error: function (msg) {
                alert("Your comment could not be posted at this time. Please try again later");
            }
        })
    })
})