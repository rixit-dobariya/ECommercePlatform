$(document).ready(function () {
    loadDeletedProductData();
});

var productDataTable;

function loadDeletedProductData() {
    productDataTable = $('#deletedProductTbl').DataTable({
        processing: true,
        ajax: {
            url: '/admin/product/GetAllDeletedProducts',
        },
        columns: [
            { data: 'productId', title: 'Product Id', width: "5%" },
            {
                data: 'imageUrl',
                render: function (data) {
                    return `
                        <div class="p-1">
                            <img src="${data}" class="img-fluid" />
                        </div>
                    `;
                },
                title: 'Product Image',
                width: "15%"
            },
            { data: 'name', title: 'Product Name', width: "20%" },
            { data: 'category.name', title: 'Category Name', width: "20%" },
            { data: 'stock', title: 'Stock', width: "10%" },
            { data: 'sellPrice', title: 'Sell Price', width: "10%" },
            {
                data: 'productId',
                render: function (data) {
                    return `
                        <a href="Product/Edit/${data}" class="btn btn-link btn-primary btn-md">
                            <i class="fa fa-edit"></i>
                        </a>
                        <a href="Product/Details/${data}" class="btn btn-link btn-primary btn-md">
                            <i class="fa fa-eye"></i>
                        </a>
                        <button onClick="restoreItem('product/restore/${data}', 'productTbl')" class="btn btn-link btn-success">
                            <i class="fa fa-undo"></i>
                        </button>
                    `;
                },
                title: 'Action',
                width: "20%"
            },
        ]
    });
}

// Restore function
function restoreItem(url, tableId) {
    Swal.fire({
        title: "Restore this product?",
        text: "It will be available for customers again.",
        icon: "question",
        showCancelButton: true,
        confirmButtonColor: "#28a745",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, restore it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "POST",
                success: function (data) {
                    productDataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}
