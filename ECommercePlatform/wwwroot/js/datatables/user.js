$(document).ready(function () {
    loadUserData();
});

var userDataTable;

function loadUserData() {
    userDataTable = $('#userTbl').DataTable({
        processing: true,
        ajax: {
            url: '/admin/user/getall',
        },
        columns: [
            { data: 'userId', title: 'User Id', width:"5%"},
            {
                data: 'profilePicture',
                render: function (data) {
                    return `
                        <div class="p-1">
                            <img src="${data}" class="img-fluid p-2" />
                        </div>
                        `;
                },
                title: 'User Profile Picture',
                width:"15%"
            },
            { data: 'fullName', title: 'User Name', width:"20%" },
            { data: 'email', title: 'Email',width:"17%" },
            { data: 'phone', title: 'Phone', width:"15%" },
            {
                data: 'isEmailVerified',
                render: function (data) {
                    return data ? "Done" : "Pending";
                },
                title: 'Email Verification',
                width: "5%"
            },
            {
                data: null,
                render: function (data) {
                    var htmlContent = `
                            <a href="User/Edit/${data.userId}" class="btn btn-link btn-primary btn-lg">
                                <i class="fa fa-edit"></i>
                            </a>
                        `;
                    if (!data.isDeleted) {
                        htmlContent += `<button onClick="deleteItem('user/delete/${data.userId}', 'userTbl')" class="btn btn-link btn-danger">
                                <i class="fa fa-times"></i>
                            </button>`;
                    }
                    if (data.isActive) {
                        htmlContent += `<button onClick="deactivateUser('user/deactivate/${data.userId}', 'userTbl')" class="btn btn-link btn-dark">
                                <i class="fa fa-toggle-off"></i>
                            </button>`;
                    }
                    else {
                        htmlContent += `<button onClick="activateUser('user/activate/${data.userId}', 'userTbl')" class="btn btn-link">
                                <i class="fa fa-toggle-on"></i>
                            </button>`;
                    }
                    return htmlContent;
                },
                title: 'Action',
                width: "23%"
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
                    userDataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}

function deactivateUser(url, tableId) {
    Swal.fire({
        title: "Are you sure?",
        text: "This user will be deactivated!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Yes, deactivate it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "POST", // Use POST instead of DELETE if deactivating
                success: function (data) {
                    $("#" + tableId).DataTable().ajax.reload();
                    toastr.success(data.message);
                },
                error: function (xhr) {
                    toastr.error("Failed to deactivate user.");
                }
            });
        }
    });
}

function activateUser(url, tableId) {
    Swal.fire({
        title: "Are you sure?",
        text: "This user will be activated!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, activate it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "POST", // Use POST instead of DELETE if activating
                success: function (data) {
                    $("#" + tableId).DataTable().ajax.reload();
                    toastr.success(data.message);
                },
                error: function (xhr) {
                    toastr.error("Failed to activate user.");
                }
            });
        }
    });
}

