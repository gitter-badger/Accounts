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



$('.unmask').on('click', function(){

    if($(this).prev('input').attr('type') == 'password')
        changeType($(this).prev('input'), 'text');

    else
        changeType($(this).prev('input'), 'password');

    return false;
});

function changeType(x, type) {
    if(x.prop('type') == type)
        return x;
    try {
        return x.prop('type', type);
    } catch(e) {

        var html = $("<div>").append(x.clone()).html();
        var regex = /type=(\")?([^\"\s]+)(\")?/;
        var tmp = $(html.match(regex) == null ?
            html.replace(">", ' type="' + type + '">') :
            html.replace(regex, 'type="' + type + '"') );
        tmp.data('type', x.data('type') );
        var events = x.data('events');
        var cb = function(events) {
            return function() {
                for(i in events)
                {
                    var y = events[i];
                    for(j in y)
                        tmp.bind(i, y[j].handler);
                }
            }
        }(events);
        x.replaceWith(tmp);
        setTimeout(cb, 10);
        return tmp;
    }
}
