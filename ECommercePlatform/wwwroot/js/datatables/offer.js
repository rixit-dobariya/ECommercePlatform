$(document).ready(function () {
    loadOfferData();
});

var offerDataTable;

function loadOfferData() {
    offerDataTable = $('#offerTbl').DataTable({
        processing: true,
        ajax: {
            url: '/admin/offer/getall'
        },
        columns: [
            { data: 'offerId', title: 'Offer Id', width: "10%" },
            { data: 'offerCode', title: 'Offer Code', width: "15%" },
            { data: 'minimumAmount', title: 'Minimum amount', width: "15%" },
            { data: 'discount', title: 'Discount', width: "15%" },
            { data: 'startDate', title: 'Start Date', width: "15%" },
            { data: 'endDate', title: 'End Date', width: "15%" },
            {
                data: 'offerId',
                render: function (data) {
                    return `
                            <a href="Offer/Edit/${data}" class="btn btn-link btn-primary btn-lg">
                                <i class="fa fa-edit"></i>
                            </a>
                            <button onClick="deleteItem('offer/delete/${data}', 'offerTbl')" class="btn btn-link btn-danger">
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
                    offerDataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}
