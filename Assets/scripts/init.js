import $ from 'jquery';
import initOrderForm from 'web/order-form';

export default function() {
    initOrderForm();

    $(window).on('load', () => {
        $('.content-wrapper').addClass('loaded');
    });
}