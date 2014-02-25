

$(document).ready(function () {
    
    $("#refreshgames").click(function (e) {
        e.preventDefault();
        refreshGames();
    });

    $("#refreshlogentries").click(function (e) {
        e.preventDefault();
        refreshLogEntries();
    });
    
    $("#clearlog").click(function (e) {
        e.preventDefault();
        clearLog();
    });
    
    if ($("#games").length > 0) {
        refreshGames();
    }
    
    if ($("#tracing").length > 0) {
        refreshLogEntries();
    }
    
    if ($("#maphighscores").length > 0) {
        refreshMapHighScores();
    }

});

function refreshMapHighScores() {
    $("#maphighscores .loading").show();
    $("#maphighscores .result").hide();
    $("#maphighscores .error").hide();

    var url = "/highscore/maphighscores";
    $.get(url, function (data) {
        $("#maphighscores .result").html(data);
    }).done(function () {
        $("#maphighscores .loading").hide();
        $("#maphighscores .error").hide();
        $("#maphighscores .result").show();
    }).fail(function () {
        $("#maphighscores .loading").hide();
        $("#maphighscores .error").show();
        $("#maphighscores .result").hide();
    });
}

function refreshGames() {
    $("#games .loading").show();
    $("#games .result").hide();
    $("#games .error").hide();
    
    var url = "/game/games";
    $.get(url, function (data) {
        $("#games .result").html(data);
    }).done(function () {
        $("#games .loading").hide();
        $("#games .error").hide();
        $("#games .result").show();
    }).fail(function () {
        $("#games .loading").hide();
        $("#games .error").show();
        $("#games .result").hide();
    });
}

function refreshLogEntries() {
    $("#tracing .loading").show();
    $("#tracing .result").hide();
    $("#tracing .error").hide();

    var url = "/tracing/entries?logLevel=" + $("#LogLevelId").val() + "&component=" + $("#ComponentId").val() + "&numberOfEntries=" + $("#NumberOfEntriesId").val();
    $.get(url, function (data) {
        $("#tracing .result").html(data);
    }).done(function() {
        $("#tracing .loading").hide();
        $("#tracing .error").hide();
        $("#tracing .result").show();
    }).fail(function () {
        $("#tracing .loading").hide();
        $("#tracing .error").show();
        $("#tracing .result").hide();
    });
}

function clearLog() {
    $("#tracing .loading").show();
    $("#tracing .result").hide();
    $("#tracing .error").hide();

    var url = "/tracing/clear";
    $.get(url, function (data) {
        $("#tracing .result").html(data);
    }).done(function () {
        refreshLogEntries();
    }).fail(function () {
        $("#tracing .loading").hide();
        $("#tracing .error").show();
        $("#tracing .result").hide();
    });
}

