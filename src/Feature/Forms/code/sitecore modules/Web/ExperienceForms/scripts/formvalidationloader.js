$(document).ready(function () {

    $('#fxb_bafb34e8-3efc-42eb-8d3e-aaa3f6e77635_9edee103-de3c-45f7-970d-fc1d13d5d7d4').submit(function (e) {
        e.preventDefault();
        var first_name = $('.your-name').val();
        var email = $('.your-email').val();
        var phone = $('.your-phone').val();
        var org = $('.your-organisation').val();

        $(".error").remove();

        if (first_name.length > 1 && email.length > 1 && phone.length > 1 && org.length > 1) {
            $('.gif-loader').show();
            console.log("waiting");
        }

    });

});