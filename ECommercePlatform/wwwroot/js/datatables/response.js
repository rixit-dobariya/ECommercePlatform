$(document).ready(function () {
    loadResponseData();
});

var responseDataTable;

function loadResponseData() {
    responseDataTable = $('#responseTbl').DataTable({
        processing: true,
        ajax: {
            url: '/admin/response/getall'
        },
        columns: [
            { data: 'responseId', title: 'Response Id', width: "10%" },
            { data: 'name', title: 'Full Name', width: "20%" },
            { data: 'email', title: 'Email', width: "10%" },
            { data: 'phone', title: 'Phone', width: "10%" },
            { data: 'message', title: 'Message', width: "30%" },
            {
                data: 'responseId',
                render: function (data, type, row) {
                    return `
                            <button onClick="openReplyModal(${data}, '${row.name}', '${row.email}')" class="btn btn-link btn-primary">
                                <i class="fa fa-reply"></i> Reply
                            </button>
                            <button onClick="deleteItem('response/delete/${data}', 'responseTbl')" class="btn btn-link btn-danger">
                                <i class="fa fa-times"></i>
                            </button>
                        `;
                },
                title: 'Action',
                width: "20%"
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
                    responseDataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}
function openReplyModal(responseId, name, email) {
    $('#responseId').val(responseId);
    $('#userName').val(name);
    $('#userEmail').val(email);
    $('#replyMessage').val(''); // Clear previous message
    $('#replyModal').modal('show');
}
