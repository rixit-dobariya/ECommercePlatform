$(document).ready(function () {
    loadCategoryData();
});

var categoryDataTable;

function loadCategoryData() {
    categoryDataTable = $('#categoryTbl').DataTable({
        processing: true,
        ajax: {
            url: '/admin/category/getall'
        },
        columns: [
            { data: 'categoryId', title: 'Category Id', width: "15%" },
            { data: 'name', title: 'Category Name', width: "15%" },
            { data: 'parentCategory.name', title: 'Parent Category', defaultContent: '-', width: "15%" },
            {
                data: null,
                render: function (data) {
                    var htmlContent =`
                            <a href="Category/Edit/${data.categoryId}" class="btn btn-link btn-primary btn-lg">
                                <i class="fa fa-edit"></i>
                            </a>
                        `;
                    if (data.childCount == 0) {
                        htmlContent += `
                        <button onClick="deleteItem('category/delete/${data.categoryId}', 'categoryTbl')" class="btn btn-link btn-danger">
                                <i class="fa fa-times"></i>
                            </button>`;
                    }
                    else {
                        htmlContent += `
                        <button onClick="displayMessage()" class="btn btn-link btn-danger">
                                <i class="fa fa-times"></i>
                            </button>`;
                    }
                    return htmlContent;
                },
                title: 'Action',
                width: "10%"
            },
        ]
    });
}

function displayMessage() {
    Swal.fire({
        text: "Parent category with child categories cannot be deleted!",
        icon: "error"
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
                    categoryDataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}
