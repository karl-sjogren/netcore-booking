import $ from 'jquery';
import initOrderForm from 'web/order-form';
import initAdmin from 'web/admin';
import moment from 'moment';
import momentSv from 'moment-sv'; // jshint ignore:line

export default function() {
    moment.locale('sv');

    initOrderForm();
    initAdmin();

    $(window).on('load', () => {
        $('.content-wrapper').addClass('loaded');
    });
}