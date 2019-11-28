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

var url = window.location.href;
console.log(url);
var value1 = url.substring(url.lastIndexOf('/') + 1);
console.log(value1);
document.getElementById("fxb_145adc60-11e2-4681-85a7-d2546a8c04aa_Fields_aa208da2-e3f9-4559-b44d-556cc40755ed__Value").value = value1; 