$(document).ready(function () {
    loadProductData();
});

var productDataTable;

function loadProductData() {
    productDataTable = $('#productTbl').DataTable({
        processing: true,
        ajax: {
            url: '/admin/product/getall',
        },
        columns: [
            { data: 'productId', title: 'Product Id', width:"5%"},
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
                width:"15%"
            },
            { data: 'name', title: 'Product Name', width:"20%" },
            { data: 'category.name', title: 'Category Name',width:"20%" },
            { data: 'stock', title: 'Stock', width:"10%" },
            { data: 'sellPrice', title: 'Sell Price', width:"10%" },
            {
                data: 'productId',
                render: function (data) {
                    return `
                            <a href="Product/Upsert/${data}" class="btn btn-link btn-primary btn-lg">
                                <i class="fa fa-edit"></i>
                            </a>
                            <button onClick="deleteItem('product/delete/${data}', 'productTbl')" class="btn btn-link btn-danger">
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
                    productDataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}
