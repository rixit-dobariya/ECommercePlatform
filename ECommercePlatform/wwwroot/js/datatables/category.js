//var dataTables = {}; // Store DataTables instances

//$(document).ready(function () {
//    $('[data-table]').each(function () {
//        let tableId = $(this).attr('id');  // Get the table ID
//        let url = $(this).data('url'); // API URL from data attribute
//        let columns = JSON.parse($(this).attr('data-columns')); // Parse column configuration

//        initializeDataTable(tableId, url, columns);
//    });
//});

//function initializeDataTable(tableId, url, columns) {
//    dataTables[tableId] = $('#' + tableId).DataTable({
//        processing: true,
//        //serverSide: true, // Enable server-side processing
//        ajax: { url: url, type: "GET", dataSrc: "data" }, // Load data from API
//        columns: columns
//    });
//}

//// Delete function with SweetAlert confirmation
//function deleteItem(url, tableId) {
//    Swal.fire({
//        title: "Are you sure?",
//        text: "You won't be able to revert this!",
//        icon: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#3085d6",
//        cancelButtonColor: "#d33",
//        confirmButtonText: "Yes, delete it!"
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: url,
//                type: "DELETE",
//                success: function (data) {
//                    dataTables[tableId].ajax.reload(); // Reload DataTable
//                    toastr.success(data.message);
//                }
//            });
//        }
//    });
//}

$(document).ready(function () {
    loadCategoryData();
});

var categoryDataTable;

function loadCategoryData() {
    categoryDataTable = $('#categoryTbl').DataTable({
        processing: true,
        serverSide: true, // Enable server-side processing
        ajax: { url: '/admin/category/getall', dataSrc: 'data' },
        columns: [
            { data: 'categoryId', title: 'Category Id', width: "15%" },
            { data: 'name', title: 'Category Name', width: "15%" },
            { data: 'parentCategory.name', title: 'Parent Category', defaultContent: 'None', width: "15%" },
            {
                data: 'categoryId',
                render: function (data) {
                    return `
                            <a href="Category/Upsert/${data}" class="btn btn-link btn-primary btn-lg">
                                <i class="fa fa-edit"></i>
                            </a>
                            <button onClick="deleteItem('category/delete/${data}', 'categoryTbl')" class="btn btn-link btn-danger">
                                <i class="fa fa-times"></i>
                            </button>
                        `;
                },
                title: 'Action',
                width: "10%"
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
                    categoryDataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}

