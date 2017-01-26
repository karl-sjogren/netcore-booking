import $ from 'jquery';

export default function() {
    $('button.order').on('click', (e) => {
        e.preventDefault();

        let target = $(e.currentTarget);
        let ticketType = target.data('ticketType');

        $('#ticketType').val(ticketType);
        $('div.form.hidden').removeClass('hidden');

        $('html, body').animate({
            scrollTop: $('div.form').offset().top
        }, 1000);
    });
}