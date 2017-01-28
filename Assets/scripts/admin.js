import $ from 'jquery';

export default function() {
    $('body').on('click', '.admin-wrapper button.set-paid', (e) => {
        e.preventDefault();
        let button = $(e.currentTarget);
        let orderId = button.data('orderId');
        let orderName = button.data('orderName');

        let result = confirm(`Vill du markera bokningen från ${orderName} som betald?`);
        if(!result) {
            return;
        }

        $.ajax({
            type: 'PUT',
            url: `/admin/api/orders/${orderId}/set-paid`,
            dataType: 'json',
        }).then(() => {
            window.location.reload(true);
        });
    });

    $('body').on('click', '.admin-wrapper button.unset-paid', (e) => {
        e.preventDefault();
        let button = $(e.currentTarget);
        let orderId = button.data('orderId');
        let orderName = button.data('orderName');

        let result = confirm(`Vill du markera bokningen från ${orderName} som obetald?`);
        if(!result) {
            return;
        }

        $.ajax({
            type: 'PUT',
            url: `/admin/api/orders/${orderId}/unset-paid`,
            dataType: 'json',
        }).then(() => {
            window.location.reload(true);
        });
    });

    $('body').on('click', '.admin-wrapper button.remove-order', (e) => {
        e.preventDefault();
        let button = $(e.currentTarget);
        let orderId = button.data('orderId');
        let orderName = button.data('orderName');

        let result = confirm(`Vill du radera bokningen från ${orderName}? Detta går typ inte att ångra.`);
        if(!result) {
            return;
        }

        $.ajax({
            type: 'DELETE',
            url: `/admin/api/orders/${orderId}`,
            dataType: 'json',
        }).then(() => {
            window.location.reload(true);
        });
    });
}