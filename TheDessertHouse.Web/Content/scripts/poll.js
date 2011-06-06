function PollClientObject() {
    this.pollCollection = null;
    this.pollObj = null;
    this.pollId = null;
    function renderPollResults(response) {

        var pollContent = [{ TotalVotes: response.TotalVotes}];
        $("#pollForm" + response.Id).remove();
        $("#voteTotalDiv-" + response.Id).tmpl(pollContent).appendTo($("#poll-" + response.Id));
        var options = response.Options;
        for (var idx in options) {
            var votePc = Math.round(options[idx].Votes / response.TotalVotes * 100);
            var optionObj = [{ OptionId: options[idx].Id, PollId: this.pollId, OptionText: options[idx].OptionText, Votes: options[idx].Votes, VotePercentage: votePc}];
            $("#pollResultOptions-" + response.Id).tmpl(optionObj).appendTo($(".poll-results-"+response.Id));
        }
    }
    this.setDisplay = function () {
        
        if ($.getCookie("poll_" + this.pollId) != null) {
            renderPollResults(this.pollObj);
        }
        else {
            var pollContent = [{ PollQuestion: this.pollObj.PollQuestion, PollId: this.pollId}];
            $("#pollForm-" + this.pollId).tmpl(pollContent).appendTo($("#poll-" + this.pollId));
            var options = this.pollObj.Options;
            for (var idx in options) {
                var optionObj = [{ OptionId: options[idx].Id, PollId: this.pollId, OptionText: options[idx].OptionText}];
                $("#pollOptions-" + this.pollId).tmpl(optionObj).appendTo($("#poll-options-" + this.pollId));
            }

            $("#pollForm" + this.pollId + " .vote-button").click(function () {

                var optId = $("li[id^=option] :checked").val();
                var pollId = $(this).attr("meta:id");
                $.ajax({
                    url: voteUrl,
                    data: JSON.stringify({ pollId: pollId, optionId: optId }),
                    success: function (response) {
                        //$(".poll-form").remove();
                        renderPollResults(response);

                    },
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "post",
                    error: function (errObj) {
                        alert("Your vote could not be recorded at this time. Please try again later");
                    }
                })
            });
        }

    }
}



