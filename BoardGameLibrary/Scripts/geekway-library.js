function clearCheckOutForm() {
    $('#check-out-form-wrapper input:text').each(function () {
        $(this).val("");
    });
    $("#check-out-form-wrapper input:checkbox").removeAttr("checked");
    $('#check-out-form-wrapper #CopyLibraryID').focus();
}

function clearCheckInForm() {
    $('#check-in-form-wrapper input:text').each(function () {
        $(this).val("");
    });
    $('#check-in-form-wrapper #CopyLibraryID').focus();
}

function updateCheckOutForm(data) {
    $('#check-out-form-wrapper').html(data.responseText);
    var copyId = $("#CopyLibraryID").val();

    if (copyId.length < 5) {
        $('#CopyLibraryID').focus();
    }
    else {
        $('#AttendeeBadgeID').focus();
    }

    $('#CopyLibraryID').change(function () {
        var id = $("#CopyLibraryID").val();
        if (id.length >= 5) {
            $('#AttendeeBadgeID').focus();
        }
    });

    $("#CopyLibraryID").blur(function () {
        var id = $("#CopyLibraryID").val();
        if (id) {
            $.get('@Url.Action("GetCopyGameTitle", "Copies")', { copyId: id }, function (response) {
                $("#locatedGameTitle").text(response.title);
            });
        }
    });

    $('a:contains(Check Out)').on('click', function () {
        $('#check-out-form-wrapper #CopyLibraryID').focus();
    });
}

function updateCheckInForm(data) {
    $('#check-in-form-wrapper').html(data.responseText);
}