$(document).ready(function () {
    loadBannerData();
});

let bannerDataTable;

function loadBannerData() {
    bannerDataTable = $('#bannerTbl').DataTable({
        processing: true,
        ajax: {
            url: '/admin/banner/getall',
            type: 'GET',
        },
        columns: [
            { data: 'bannerId', title: 'Banner ID', width: '5%' },
            {
                data: 'imageUrl',
                title: 'Banner Image',
                width: '15%',
                render: function (data) {
                    return `
                        <div class="p-1">
                            <img src="${data}" class="img-fluid" style="max-height: 60px;" />
                        </div>
                    `;
                }
            },
            { data: 'title', title: 'Title', width: '25%' },
            {
                data: 'bannerId',
                title: 'Action',
                width: '20%',
                render: function (data) {
                    return `
                        <button onClick="deleteItem('/admin/banner/delete/${data}', 'bannerTbl')" 
                            class="btn btn-sm btn-danger" title="Delete">
                            <i class="fa fa-trash"></i>
                        </button>
                    `;
                }
            }
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
                    if (data.success) {
                        $('#' + tableId).DataTable().ajax.reload();
                        toastr.success(data.message || "Banner deleted successfully.");
                    } else {
                        toastr.error(data.message || "Failed to delete banner.");
                    }
                },
                error: function () {
                    toastr.error("Something went wrong. Please try again.");
                }
            });
        }
    });
}
