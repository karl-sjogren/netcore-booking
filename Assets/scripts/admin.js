import $ from 'jquery';
import moment from 'moment';

export default function() {
    $(document).on('ready', () => {
        $.ajax({
            type: 'GET',
            url: '/admin/api/orders',
            dataType: 'json',
        }).then((result) => {
            let table = $('#orders');

            for(let i = 0; i < result.length; i++) {
                let data = result[i];
                
                let paidDate = '-';
                if(data.paid) {
                    paidDate = moment(data.paidDate).format('YYYY-MM-DD')
                }

                let row = $(`<tr data-order-id="${data.id}">
                                <td class="col-name">${data.name} (${data.ticketCount}st)</td>
                                <td>${data.phone}</td>
                                <td>${data.email}</td>
                                <td>${moment(data.orderDate).format('YYYY-MM-DD')}</td>
                                <td>${data.paid ? 'Japp' : 'Nepp'}</td>
                                <td>${paidDate}</td>
                                <td>
                                    <button class="set-paid">Markera som betald</button>
                                    <button class="remove">Radera</button>
                                </td>
                             </tr>`);

                row.appendTo(table);
            }
        });
    });

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
        }).then((result) => {

        });
    });
}