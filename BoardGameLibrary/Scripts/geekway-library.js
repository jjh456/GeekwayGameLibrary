function clearCheckOutForm(){
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
}

function updateCheckInForm(data)
{
    $('#check-in-form-wrapper').html(data.responseText);
}