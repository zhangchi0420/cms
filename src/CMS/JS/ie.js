$(document).ready(function(){
    $('.button').mousedown(function(){
        $(this).addClass('active');
    }).bind('mouseup mouseout', function(){
        $(this).removeClass('active');
    });
});


$(function() {
    $('table').attr('cellpadding', 0).attr('cellspacing', 0).attr('border', 0);
    $('H2,.login_main,.main,.button_bar input,.button_bar button,.button_bar a').each(function() {
        PIE.attach(this);
    });
});