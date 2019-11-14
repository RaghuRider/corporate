$(document).ready(function () {

    $('.send-loader').click(function () {
        //e.preventDefault();
        var contact_name = $('.contact-name').val();
        var contact_email = $('.contact-email').val();
        var contact_message = $('.contact-message').val();

        $(".error").remove();

        if (contact_name.length >= 1 && contact_email.length >= 1 && contact_message.length >= 1) {
            $('.gif-loader').show();            
        }
    });
});