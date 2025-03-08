$(document).ready(function () {
    loadReviewData();
});

var reviewDataTable;

function loadReviewData() {
    reviewDataTable = $('#reviewTbl').DataTable({
        processing: true,
        ajax: {
            url: '/Admin/Review/GetAllForProduct', // Adjust the controller/action as needed
            type: 'GET',
            data: function (d) {
                d.id = productId; // Pass ProductId to filter reviews
            }
        },
        columns: [
            { data: 'reviewId', title: 'Review ID', width: "10%" },
            { data: 'reviewText', title: 'Review Text', width: "30%" },
            { data: 'rating', title: 'Rating', width: "10%" },
            { data: 'userName', title: 'User Name', width: "20%" },
            { data: 'replyText', title: 'Reply Text', width: "20%" },
            {
                data: 'reviewId',
                render: function (data, type, row) {
                    let htmlContent = "";
                    if (row.replyText === null) {
                        htmlContent = `<a href="/Admin/Review/Reply/${data}" class="btn btn-link btn-primary btn-sm">
                            <i class="fa fa-reply"></i>
                        </a>`;
                    }
                    htmlContent+=`
                        <button onClick="deleteReview(${data})" class="btn btn-link btn-danger btn-sm">
                            <i class="fa fa-times"></i>
                        </button>
                    `;
                    return htmlContent;
                },
                title: 'Action',
                width: "20%"
            }
        ]
    });
}

function deleteReview(reviewId) {
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
                url: `/Admin/Review/Delete/${reviewId}`,
                type: "DELETE",
                success: function (data) {
                    reviewDataTable.ajax.reload();
                    toastr.success(data.message);
                },
                error: function (data) {
                    toastr.error(data.message);
                }
            });
        }
    });
}
