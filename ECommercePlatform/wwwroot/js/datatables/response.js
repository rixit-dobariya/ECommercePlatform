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
                    let htmlContent = "";
                    if (row.reply === null || row.reply === "") {
                        htmlContent += `<a href="Response/Reply/${data}" class="btn btn-link btn-primary btn-lg">
                                <i class="fas fa-reply"></i>
                            </a>`;
                    }
                    htmlContent+= `
                            <button onClick="deleteItem('response/delete/${data}', 'responseTbl')" class="btn btn-link btn-danger">
                                <i class="fa fa-times"></i>
                            </button>
                        `;
                    return htmlContent;
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