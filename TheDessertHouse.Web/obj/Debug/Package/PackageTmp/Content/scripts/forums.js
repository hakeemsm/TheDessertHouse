$(function () {
    var postParser = new ForumPostParser();
    postParser.Posts = postCollection;
    if (postCollection.length > 0) {
        postParser.SetPostsInView();
    }
    if (referrer.substr(referrer.indexOf("=") + 1) == window.location.pathname) {
        displayPostForm();
    }
    $("#lnkCreatePost").click(function () {

        $.get(authenticateUrl, function (response) {

            if (!response) {
                window.location = loginUrl;
                return;
            }
            else {
                displayPostForm();
            }
        }, "json");

    })

})
function displayPostForm() {
    $("#createPostTemplate").tmpl([{ ForumId: forumId}]).appendTo($("#createPostContainer"));
    $("#createPostContainer").dialog({
        modal: true,
        resizable: false,
        draggable: false,
        dialogClass: "boxed content",
        width: 550,
        title: "New post",
        closeOnEscape: false,
        open: function (event, ui) { $(".ui-dialog-titlebar-close").hide(); }
    });
    $("#btnCancel").click(function () {
        closePostForm();
    })
    $("#btnCreatePost").click(function () {
        var title = $("#txtTitle").val();
        var body = $("#txtBody").val();
        if (title == '') {
            alert("Enter a title for the post");
            return;
        }
        if (body == '') {
            alert("Enter content for your post");
            return;
        }
        var data = { forumId: forumId, title: title, post: body };
        $.ajax({
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: createPostUrl,
            data: JSON.stringify(data),
            success: function (response) {
                closePostForm();
                var parserObj = new ForumPostParser();
                parserObj.AddPostToView(response);

            },
            error: function (xhr, stat) {
                closePostForm();
                alert("Your post could not be added at this time. Please try again later");
            }
        })
    })
}

function closePostForm() {
    $("#createPostContainer").dialog("close");
    $(".post-create").remove();
}

function ForumPostParser() {
    this.Posts = null;

    this.SetPostsInView = function () {
        for (var pollIdx in this.Posts) {
            this.AddPostToView(this.Posts[pollIdx]);
        }
    }

    this.AddPostToView = function (post) {

        var postObj = [{ PostId: post.Id, VoteCount: post.VoteCount, ReplyCount: post.ReplyCount, ViewCount: post.ViewCount, Path: post.Path, LastPostDate: post.LastPostDateInUTC, Avatar: post.Avatar, LastPostBy: post.LastPostBy, Title: post.Title}];
        var adminObj = [{ PostId: post.Id}];
        $("#forumCollectionTemplate").tmpl(postObj).appendTo($("#posts"));
        if (userIsEditor == 'true') {
            $("#adminTemplate").tmpl(adminObj).insertBefore($("#stats" + post.Id));
            if (post.Closed == 'true') {
                $("#admin" + post.Id).remove(".close-post");
            }
        }
        if (post.Closed == 'true') {
            $("#lnkViewPost" + post.Id).after("<span>[closed] </span>");
        }
    }
}