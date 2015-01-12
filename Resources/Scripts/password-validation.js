/**
 * Created by Kiri-AnnEgington on 12/01/2015.
 */

//password icons changing colour
$(document).ready(function() {


    $('input[type=password]').keyup(function() {
        // set password variable
        var pswd = $(this).val();

        //validate the length
        if ( pswd.length < 8 ) {
            $('#length').removeClass('valid').addClass('invalid');
        } else {
            $('#length').removeClass('invalid').addClass('valid');
        }

        //validate capital letter
        if ( pswd.match(/[a-z].*[A-Z]|[A-Z].*[a-z]/) ) {
            $('#letter').removeClass('invalid').addClass('valid');
        } else {
            $('#letter').removeClass('valid').addClass('invalid');
        }

        //validate number
        if ( pswd.match(/\d/) ) {
            $('#number').removeClass('invalid').addClass('valid');
        } else {
            $('#number').removeClass('valid').addClass('invalid');
        }

        //validate special characters
        if ( pswd.match(/[-!$%^&*()_+|~=`{}\[\]:";'<>?,.\/]/) ) {
            $('#specialCharacter').removeClass('invalid').addClass('valid');
        } else {
            $('#specialCharacter').removeClass('valid').addClass('invalid');
        }

    })
});
