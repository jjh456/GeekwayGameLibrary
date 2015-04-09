function clearCheckOutForm(){
    $('#check-out-form-wrapper input:text').each(function () {
        $(this).val("");
    });
}

function clearCheckInForm() {
    $('#check-in-form-wrapper input:text').each(function () {
        $(this).val("");
    });
}

function updateCheckOutForm(data) {
    $('#check-out-form-wrapper').html(data.responseText);
}

function updateCheckInForm(data)
{
    $('#check-in-form-wrapper').html(data.responseText);
}