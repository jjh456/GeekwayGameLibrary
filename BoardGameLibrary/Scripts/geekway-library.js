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

function updateForm(data) {
    $('#check-out-form-wrapper').html(data);
}