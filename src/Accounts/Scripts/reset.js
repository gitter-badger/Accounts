/**
 * Created by Kiri-AnnEgington on 22/01/2015.
 */
///**
// * Created by Kiri-AnnEgington on 12/01/2015.
// */
//$(document).ready(function(){
//
//    $('.radio.input').click(function(){
//        $(this).parent().toggleClass('selected');
//    });
//});


var $radios = $('input:radio');

$radios.change(function () {
    $radios.parent().removeClass('selected');
    $(this).parent().addClass('selected');
});

$radios.filter(':checked').parent().addClass('selected');
