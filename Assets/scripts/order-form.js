import $ from 'jquery';

export default function() {
    $('#order-form').on('submit', (e) => {
        e.preventDefault();
        
        let isValid = e.currentTarget.reportValidity();
        if(!isValid) {
            return;
        }

        let request = {
            name: $('#name').val(),
            address: $('#address').val(),
            zipCode: $('#zipcode').val(),
            city: $('#city').val(),
            phone: $('#phone').val(),
            email: $('#email').val(),
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
            alert('whopsie');
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