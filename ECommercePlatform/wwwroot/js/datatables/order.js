﻿$(document).ready(function () {
    loadOrderData();
});

var orderDataTable;
/*
 {
      "orderId": 1,
      "userFullName": "Rixit Dobariya",
      "orderDate":"date",
      "orderStatus": "Pending",
      "paymentMode": "UPI"
      "shippingCharge": 5,
      "subtotal": 100,
      "total": 105,
    },
*/

function loadOrderData() {
    orderDataTable = $('#orderTbl').DataTable({
        processing: true,
        ajax: {
            url: '/admin/order/getall',
        },
        columns: [
            { data: 'orderId', title: 'Order Id', width:"5%"},
            { data: 'userFullName', title: 'User Name', width:"20%" },
            { data: 'orderDate', title: 'Order Date', width:"10%" },
            { data: 'orderStatus', title: 'Order Status', width:"10%" },
            { data: 'paymentMode', title: 'Payment Mode', width:"10%" },
            { data: 'shippingCharge', title: 'Shipping Charge', width:"10%" },
            { data: 'subtotal', title: 'Sub Total', width:"10%" },
            { data: 'total', title: 'Total', width: "10%" },
            {
                data: 'orderId',
                render: function (data) {
                    return `
                            <a href="/Admin/Order/Details/${data}" class="btn btn-link btn-primary btn-md">
                                <i class="fa fa-eye"></i>
                            </a>
                            <button onClick="deleteItem('order/delete/${data}', 'orderTbl')" class="btn btn-link btn-danger">
                                <i class="fa fa-times"></i>
                            </button>
                        `;
                },
                title: 'Action',
                width: "15%"
            },
        ]
    });
}

function deleteItem(url, tableId) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    orderDataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}
