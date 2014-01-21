

$(document).ready(function () {
    
    $("#refreshlogentries").click(function (e) {
        e.preventDefault();
        refreshLogEntries();
    });
    
    $("#clearlog").click(function (e) {
        e.preventDefault();
        clearLog();
    });
    
    if ($("#tracing").length > 0) {
        refreshLogEntries();
    }

});

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

