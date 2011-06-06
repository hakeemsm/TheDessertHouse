$(function () {
    var postParser = new PostReplyParser();
    postParser.Replies = repliesObj.Replies;
    if (repliesObj.Replies.length > 0) {
        postParser.SetRepliesInView();
    }
    if (referrer.substr(referrer.indexOf("=") + 1) == window.location.pathname) {
        $("#replyPost").show();
        $("#lnkReply").hide();
    }
    $("#lnkReply").click(function () {

        $.get(authenticateUrl, function (response) {
            if (!response) {
                window.location = loginUrl;
                return;
            }
            else {
                $("#replyPost").show();
                $("#lnkReply").hide();

            }
        }, "json");

    })
    $("#btnPostReply").click(function () {
        var postId = $("#postId").val();
        var title = $("#postTitle").val();
        var reply = $("#txtReply").val();
        var replyObj = { postId: postId, title: title, post: reply };
        $.ajax({
            type: "post",
            url: postReplyUrl,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(replyObj),
            success: function (response) {
                var replyParser = new PostReplyParser();
                replyParser.AddReplyToView(response);
                $("#txtReply").val("");
            },
            error: function (xhr, msg) {
                alert("Your reply could not be posted at this time. Please try again later");
            }

        })
    })

    $("#posts .post .vote-button a").click(function () {
        var postId = $("#postId").val();
        var selection = $(this).attr("href");
        var direction = selection == "#up" ? 1 : -1;
        $.post(postVoteUrl, { postId: postId, vote: direction }, function (result) {
            if (result.error == "Not authenticated") {
                alert("You must be logged in to vote");
                return;
            }
            if (result.error == "notfound") {
                alert("No post found with the specified id");
                return;
            }
            if (result.error == "Already voted") {
                alert("Only one vote is allowed per post per user :)");
                return;
            }
            $("#posts .post .vote-button span").val(result.VoteCount);
            var button = $(".post .vote-" + (result.Direction > 0 ? "up" : "down"));

            $(".post .vote-button a").remove();
            $(".post .vote-button a").insertAfter(result.Direction > 0 ? "You liked this post" : "You disliked this post");

            //            $(".post .vote-button a").removeClass("selected");
            //            button.addClass("selected");

            //            $("#posts .post .vote-button a").mouseover(function () {
            //                $(this).attr("style", "cursor: pointer");
            //            });

        }, "json");
    })


})




function closePostForm() {
    $("#createPostContainer").dialog("close");
    $(".post-create").remove();
}

function PostReplyParser() {
    this.Replies = null;

    this.SetRepliesInView = function () {
        for (var idx in this.Replies) {
            this.AddReplyToView(this.Replies[idx]);
        }
    }

    this.AddReplyToView = function (reply) {
        var postObj = [{ PostId: reply.Id, LastPostDate: reply.LastPostDateInUTC, Avatar: reply.Avatar, LastPostBy: reply.LastPostBy, Body: reply.Body}];

        $("#replyTemplate").tmpl(postObj).appendTo($("#forum-post-replies"));
        if (userIsEditor == 'False') {
            $(".admin-reply").remove();
        }
        else {
            $(".remove-reply").click(function () {

                var postId = $(this).attr("meta:id");

                var msg = "Are you sure you want to delete this post?";
                if (confirm(msg)) {
                    $.post(removePostUrl, { postId: postId }, function (result) {
                        if (response.deleted) {
                            $("#forum-" + response.Id).hide("slow");
                            $("#forum-" + response.Id).remove();
                        }
                        else {
                            alert("The forum could not be deleted at this tume. Please try again later");
                        }
                    }, "json");


                }
            })
        }
    }
}