$(document).ready(function () {

    $('.book-loader').click(function () {
        //e.preventDefault();
        var first_name = $('.your-name').val();
        var email = $('.your-email').val();
        var phone = $('.your-phone').val();
        var org = $('.your-organisation').val();

        $(".error").remove();

        if (first_name.length >= 1 && email.length >= 1 && phone.length >= 1 && org.length >= 1) {
            $('.gif-loader').show();
            $(".book-loader").css("background-color", "#3c3950");
          //  $(".book-loader").attr("disabled", true);
        }

    });
    return false;
});