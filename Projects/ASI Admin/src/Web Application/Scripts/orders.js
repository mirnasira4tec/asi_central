$('input.command').click(function (e) {
    $('#ActionName').val(e.target.value);
    if ($('#ActionName').val() == 'Accept') {
        ////make sure we have Timms value
        //if (!$('#ExternalReference').val()) {
        //    asi.modal.confirm('Error', 'You need to pass a Timms ID number to approve order.', null, "#hide", "Close");
        //    $("#ExternalReference").attr("class", "text-box single-line input-validation-error span2");
        //    $("#timmsErrorMessage").text("You need to pass a Timms ID number to approve order.");
        //    e.preventDefault();
        //}
        if ($('#ExternalReference').val()) {
            var isnum = /^\d+$/.test($('#ExternalReference').val());
            if (!isnum) {
                asi.modal.confirm('Error', 'TIMMS ID must contain only numbers.', null, "#hide", "Close");
                $("#ExternalReference").attr("class", "text-box single-line input-validation-error span2");
                $("#timmsErrorMessage").text("TIMMS ID must contain only numbers.");
                e.preventDefault();
            }
            else
            {
                $(this).attr('disabled', 'disabled');
                $(this).closest('form')[0].submit();
            }
        }

        if ($('#hasPendingTerms').length > 0) {
            $("#termsWarningMessage").text('This order has pending Terms and Conditions.');
            var confirm = window.confirm('This order has pending Terms and Conditions. Are you sure to accept the order?');
            if (!confirm) {
                e.preventDefault();
            }
        }
    }
});
$('[rel*="isprimary"]').change(function () {
    $('[rel*="isprimary"]').attr("checked", false);
    this.checked = true;
});
