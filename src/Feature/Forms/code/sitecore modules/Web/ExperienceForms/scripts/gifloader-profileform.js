$(document).ready(function () {

    $('.profile-loader-button').click(function () {
        //e.preventDefault();
        var profile_name = $('.career-field-name').val();
        var profile_email = $('.career-field-email').val();
        var profile_phone = $('.career-field-phone').val();
        var profile_dropdown = $('.career-field-jobapplied').val() == 0;


        $(".error").remove();

        if (profile_name.length >= 1 && profile_email.length >= 1 && profile_phone.length >= 1 &&
            profile_dropdown.length >=1 )
        {
            $('.gif-loader').show();
            $(".profile-loader-button").css("background-color", "#3c3950");
            $(".profile-loader-button").attr("disabled", true); 
        }
    });

});