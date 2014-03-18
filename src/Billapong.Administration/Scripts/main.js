

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
    
    if ($("#mapscores").length > 0) {
        refreshMapScores();
    }

});

function initAjaxRequest(panelId) {
    $("#" + panelId + " .loading").show();
    $("#" + panelId + " .result").hide();
    $("#" + panelId + " .error").hide();
}

function success(panelId) {
    $("#" + panelId + " .loading").hide();
    $("#" + panelId + " .result").show();
    $("#" + panelId + " .error").hide();
}

function fail(panelId) {
    $("#" + panelId + " .loading").hide();
    $("#" + panelId + " .result").hide();
    $("#" + panelId + " .error").show();
}

function refreshMapScores() {
    initAjaxRequest("mapscores");

    var url = "/highscore/MapScores/" + $("#mapscores").attr("mapId");
    $.get(url, function (data) {
        $("#mapscores .result").html(data);
    }).done(function () {
        success("mapscores");
    }).fail(function () {
        fail("mapscores");
    });
}

function refreshMapHighScores() {
    initAjaxRequest("maphighscores");

    var url = "/highscore/maphighscores";
    $.get(url, function (data) {
        $("#maphighscores .result").html(data);
    }).done(function () {
        success("maphighscores");
    }).fail(function () {
        fail("maphighscores");
    });
}

function refreshGames() {
    initAjaxRequest("games");
    
    var url = "/game/games";
    $.get(url, function (data) {
        $("#games .result").html(data);
    }).done(function () {
        success("games");
    }).fail(function () {
        fail("games");
    });
}

function refreshLogEntries() {
    initAjaxRequest("tracing");

    var url = "/tracing/entries?logLevel=" + $("#LogLevelId").val() + "&component=" + $("#ComponentId").val() + "&numberOfEntries=" + $("#NumberOfEntriesId").val();
    $.get(url, function (data) {
        $("#tracing .result").html(data);
    }).done(function () {
        success("tracing");
    }).fail(function () {
        fail("tracing");
    });
}

function clearLog() {
    initAjaxRequest("tracing");

    var url = "/tracing/clear";
    $.get(url, function (data) {
        $("#tracing .result").html(data);
    }).done(function () {
        refreshLogEntries();
    }).fail(function () {
        fail("tracing");
    });
}

