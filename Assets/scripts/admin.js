import $ from 'jquery';

export default function() {
    $('body').on('click', 'button.set-paid', (e) => {
        e.preventDefault();
        let button = $(e.currentTarget);
        let orderId = button.parents('tr').first().data('orderId');
        let orderName = button.parents('tr').first().find('.col-name').text();

        let result = confirm(`Vill du markera bokningen från ${orderName} som betald?`);
        if(!result) {
            return;
        }

        $.ajax({
            type: 'PUT',
            url: `/admin/api/orders/${orderId}/payment`,
            dataType: 'json',
        }).then((/*result*/) => {

        });
    });
}