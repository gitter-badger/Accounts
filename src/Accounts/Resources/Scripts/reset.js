///**
// * Created by Kiri-AnnEgington on 12/01/2015.
// */
//$(document).ready(function(){
//
//    $('.radio.input').click(function(){
//        $(this).parent().toggleClass('selected');
//    });
//});


$(document).ready(function() {

    var foo = $('input.radio:checked');

    if ( foo.is(':checked') ) {
        foo.parent().addClass('selected');
    }
});