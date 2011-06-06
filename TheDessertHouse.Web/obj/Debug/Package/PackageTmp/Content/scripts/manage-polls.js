$(function () {
    $(".set-current").click(function () {
        var pollId = $(this).attr("meta:id");
        $.post(setCurrentUrl, { pollId: pollId }, function (response) {
            $("#set-current-" + response).remove()
        }, "json");
    })

    $(".set-archived").click(function () {
        var pollId = $(this).attr("meta:id");
        $.post(setArchiveUrl, { pollId: pollId }, function (response) {
            var pollObj = $("#set-archived-" + response.PollId);
            pollObj.removeClass("set-archived");
            if (response.IsArchived) {
                pollObj.addClass("archived");
                pollObj.text("Allow Voting");
            }
        }, "json");
    })

    $(".remove-poll").click(function () {
        var confirmResp = confirm("Are you sure?");
        if (!confirmResp) {
            return;
        }
        var pollId = $(this).attr("meta:id");
        $.ajax({
            url: deletePollUrl,
            data: JSON.stringify({ pollId: pollId }),
            success: function (response) {
                var pollContainer = $("#poll-container-" + response);
                pollContainer.hide("slow");
                $("#polls").remove(pollContainer);
            },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "post",
            error: function (resp) {
                alert("Poll could not be removed at this time. Please try again later");
            }
        })

    })

    $(".edit-poll").click(function () {
        var pollId = $(this).attr("meta:id");
        $("#pollText-" + pollId).hide(); // replaceWith($("#pollEditContainer-" + pollId));
        $("#pollEditContainer-" + pollId).show();
        $("#pollEditContainer-" + pollId).removeClass("edit-poll-container-hidden");
        $("#pollEditContainer-" + pollId).addClass("edit-poll-container");
    })

    $(".update-poll-button").click(function () {
        $(this).parents("form").submit();
    })

    $("form.update-form").submit(function () {

        var optionsObj = new Object();
        optionsObj.Options = new Array();
        $(".option-text", this).each(function (index) {
            optionsObj.Options[index] = { Id: $(this).attr("metaId"), OptionText: $(this).val() };

        })

        var pollViewObj = { PollQuestion: $("#PollQuestion", this).val(), Id: $("#Id", this).val(), Options: optionsObj.Options };
        $.ajax({
            url: updatePollUrl,
            type: "post",
            data: JSON.stringify(pollViewObj),
            dataType: "html",
            contentType: "application/json; charset=utf-8",
            success: function (content) {
                var editContainer = $(".edit-poll-container").attr("id");
                $(".edit-poll-container").html(content);
                $(editContainer).removeClass("edit-poll-container");
                $(editContainer).addClass("edit-poll-container-hidden");

            },
            error: function (jqXHR, errStatus, msg) {
                if (jqXHR.status != 0) { //for some reason an error is trapped even for good responses with a stat code of 0
                    alert("Your changes could not be saved at this time. Please try again later");
                }
            }
        })
    })

    $(".delete-option-link").click(function () {
        var optId = $(this).attr("meta:id");
        $("#optionRow-" + optId).remove();
    })

    $(".update-cancel-button").click(function () {
        var pollId = $(this).attr("meta:id");
        $("#pollText-" + pollId).show();
        $("#pollEditContainer-" + pollId).hide();
        $("#pollEditContainer-" + pollId).removeClass("edit-poll-container");
        $("#pollEditContainer-" + pollId).addClass("edit-poll-container-hidden");
    })
})