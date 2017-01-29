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

    $('#order-form').on('submit', (e) => {
        e.preventDefault();
        
        try {
            let isValid = e.currentTarget.reportValidity();
            if(!isValid) {
                return;
            }
        } catch(err) {
            // This happens in Safari since ut sucks.
            console.warn('Failed to validate form', err);
        }

        let request = {
            name: $('#name').val(),
            address: $('#address').val(),
            zipCode: $('#zipcode').val(),
            city: $('#city').val(),
            phone: $('#phone').val(),
            email: $('#email').val(),
            ticketType: $('#ticketType').val(),
            tickets: []
        };

        request.tickets.push({
            name: request.name
        });

        let count = parseInt($('#ticket-count').val());
        for(var i = 1; i < count; i++) {
            let name = $('#extra-name-' + i).val();
            request.tickets.push({
                name: name
            });
        }

        request.ticketCount = request.tickets.length;

        $.ajax({
            type: 'POST',
            url: '/api/orders' + window.location.search,
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify(request)
        }).then(() => {
            window.location = '/confirmation';
        }).then(null, () => {
            window.location = '/whopsie';
        });
    });

    $('#ticket-count').on('change', (e) => {
        let count = parseInt($(e.currentTarget).val());
        
        $('#extra-names-header').toggleClass('hidden', count <= 1);
        $('.extra-name').addClass('hidden');
        $('.extra-name input').removeAttr('required');

        for(var i = 1; i < count; i++) {
            $('#extra-name-' + i).parent().removeClass('hidden');
            $('#extra-name-' + i).attr('required', true);
        }
    });
}