$(() => {
    ShowAllTransactions();
});

// load all transactions
function ShowAllTransactions() {
    $("#transaction-table tbody").html("");
    var startDate = $("#startdate").val();
    var endDate = $("#enddate").val();

    $.ajax({
        url: "https://localhost:7248/api/RentingTransactions",
        type: "get",
        contentType: "application/json",
        success: function (result, status, xhr) {
            $.each(result, function (index, value) {
                $("#transaction-table tbody").append($("<tr>"));
                appendElement = $("#transaction-table tbody tr").last();
                appendElement.append($("<td>").html(value["rentingDate"]));
                appendElement.append($("<td>").html(value["customer"].customerName));
                appendElement.append($("<td>").html(value["totalPrice"]));
                appendElement.append($("<td>").html(value["rentingStatus"]));
                appendElement.append($("<td>").html(`<a asp-action="TransactionDetails" asp-route-transactionId="@item.RentingTransationId">Details</a>
                    <a asp-action="Edit" asp-route-id="@item.RentingTransationId">| Edit</a>
                    <a asp-action="Delete" asp-route-id="@item.RentingTransationId">| Delete</a>`));
            });
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}
