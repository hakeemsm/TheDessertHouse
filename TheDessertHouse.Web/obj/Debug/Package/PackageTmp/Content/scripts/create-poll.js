$(function () {
    if (existingPoll === 'True') {
        $("#btnCreateUpdatePoll").val("Update Poll");
    }

    var pollCount = 0;
    $("#btnAddOption").click(function () {
        pollCount = ++pollCount;
        var optionId = [{ optionId: pollCount}];
        $("#optionContainer").tmpl(optionId).appendTo("#tblPoll").slideDown("slow");

        //        $(".option-add-class").click(function () {
        //            pollCount = ++pollCount;
        //            var optionId = [{ optionId: pollCount}];
        //            $("#optionContainer").tmpl(optionId).appendTo("#optionsDiv").slideDown("slow");
        //            $(this).val("Remove Option");
        //            $(this).removeClass(".option-add-class");
        //            $(this).addClass(".option-remove-class");
        //        })


        $(".option-remove-class").click(function () {
            var id = $(this).attr("meta:id");
            var target = $("#divOption" + id);
            target.fadeOut("slow");
            $("#optionsDiv").detach(target);
            //            if (pollCount > parseInt(id)) {
            //                $(".option-label").each(function (index) {
            //                    $(this).text("Option " + index);
            //                })
            //            }

            pollCount = --pollCount;
        })
    })

    $("#btnCreateUpdatePoll").click(function () {
        if (pollCount === 0) {
            if (!confirm("Save poll without options")) {
                return;
            }
        }
        var optionsObj = new Object();
        optionsObj.Options = new Array();

        $(".poll-option-class").each(function (index) {
            optionsObj.Options[index] = { OptionText: $(this).val() };
        })
        var pollQuestion = $("#PollQuestion").val();
        var pollObj = { PollQuestion: pollQuestion, Options: optionsObj.Options }
        $.ajax({
            url: createPollUrl,
            data: JSON.stringify(pollObj),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "post",
            success: function (response) {
                window.location.replace(response);
            },
            error: function (err) {
                alert("Poll could not be saved at this time. Please try again later");
            }
        })
    })

    $(".edit-option-button").click(function () {
        var optionId = $(this).attr("meta:id");
        var optionItemContainer = $("#option-" + optionId);
        var optionText = optionItemContainer.find(".text").text();
        optionItemContainer.children().hide();
        var optionData = [{ optionId: optionId, optionText: optionText}];
        $("#optionEditForm").tmpl(optionData).appendTo(optionItemContainer);

        $(".update-option").click(function () {
            var optionId = $(this).attr("meta:id");
            var optionText = $("#text-" + optionId).val();
            $.post(optionEditUrl, { OptionId: optionId, OptionText: optionText }, function (response) {
                var updatedOptionId = response.optionId;
                var parent = $("#option-" + updatedOptionId);
                parent.children("form").remove();
                parent.children(".text").text(response.optionText);
                parent.children().show();

            }, "json");
        })

        $(".cancel-option").click(function () {
            var parent = $(this).parents(".option-item");
            parent.children("form").remove();
            parent.children().show();
        })
    })

    $(".delete-option-button").click(function () {
        var optionId = $(this).attr("meta:id");
        $.post(deleteOptionUrl, { optionId: optionId }, function (response) {
            $("#option-" + response).fadeOut("slow", function () { $(this).remove(); });
        }, "json");
    })

})