$(() => {
    var connection = new signalR.HubConnectionBuilder().withUrl("/hubServer").build();
    connection.start();

    connection.on("LoadRentingCars", function () {
        ShowRentingCars();
    })

    connection.on("LoadCars", function () {
        ShowAllCars();
    })

    connection.on("LoadCustomers", function () {
        ShowAllCustomers();
    })

    //connection.on("LoadTransactions", function () {
    //    ShowAllAdminTransactions();
    //})
});

// load renting cars
function ShowRentingCars() {
    $("#renting-car-table tbody").html("");
    var startDate = $("#startdate").val();
    var endDate = $("#enddate").val();

    $.ajax({
        url: "https://localhost:7248/api/CarInformations/valid",
        type: "get",
        contentType: "application/json",
        success: function (result, status, xhr) {
            $.each(result, function (index, value) {
                $("#renting-car-table tbody").append($("<tr>"));
                appendElement = $("#renting-car-table tbody tr").last();
                appendElement.append($("<td>").html(value["carName"]));
                appendElement.append($("<td>").html(value["carDescription"]));
                appendElement.append($("<td>").html(value["numberOfDoors"]));
                appendElement.append($("<td>").html(value["seatingCapacity"]));
                appendElement.append($("<td>").html(value["fuelType"]));
                appendElement.append($("<td>").html(value["year"]));
                appendElement.append($("<td>").html(value["carRentingPricePerDay"]));
                appendElement.append($("<td>").html(value["manufacturer"].manufacturerName));
                appendElement.append($("<td>").html(value["supplier"].supplierName));
                appendElement.append($("<td>").html(`<a href="/Cart/AddToCart?carId=${value["carId"]}&startDate=${startDate}&endDate=${endDate}">Choose</a>`));
            });
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}

// load all cars
function ShowAllCars() {
    $("#car-table tbody").html("");

    $.ajax({
        url: "https://localhost:7248/api/CarInformations",
        type: "get",
        contentType: "application/json",
        success: function (result, status, xhr) {
            $.each(result, function (index, value) {
                $("#car-table tbody").append($("<tr>"));
                appendElement = $("#car-table tbody tr").last();
                appendElement.append($("<td>").html(value["carName"]));
                appendElement.append($("<td>").html(value["carDescription"]));
                appendElement.append($("<td>").html(value["numberOfDoors"]));
                appendElement.append($("<td>").html(value["seatingCapacity"]));
                appendElement.append($("<td>").html(value["fuelType"]));
                appendElement.append($("<td>").html(value["year"]));
                appendElement.append($("<td>").html(value["carRentingPricePerDay"]));
                appendElement.append($("<td>").html(value["manufacturer"].manufacturerName));
                appendElement.append($("<td>").html(value["supplier"].supplierName));
                appendElement.append($("<td>").html(`<a href="/Admin/CarInformations/edit/${value["carId"]}">Edit</a> | `
                    + `<a href="/Admin/CarInformations/details/${value["carId"]}">Details</a> | `
                    + `<a href="/Admin/CarInformations/delete/${value["carId"]}">Delete</a>`));
            });
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}

// load all customers
function ShowAllCustomers() {
    $("#customer-table tbody").html("");

    $.ajax({
        url: "https://localhost:7248/api/Customers",
        type: "get",
        contentType: "application/json",
        success: function (result, status, xhr) {
            $.each(result, function (index, value) {
                $("#customer-table tbody").append($("<tr>"));
                appendElement = $("#customer-table tbody tr").last();
                appendElement.append($("<td>").html(value["customerName"]));
                appendElement.append($("<td>").html(value["telephone"]));
                appendElement.append($("<td>").html(value["email"]));
                appendElement.append($("<td>").html(value["customerBirthday"]));
                appendElement.append($("<td>").html(value["customerStatus"]));
                appendElement.append($("<td>").html(`<a href="/Admin/Customers/edit/${value["customerId"]}">Edit</a> | `
                    + `<a href="/Admin/Customers/details/${value["customerId"]}">Details</a> | `
                    + `<a href="/Admin/Customers/delete/${value["customerId"]}">Delete</a>`));
            });
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}

// load all admin transactions
function ShowAllAdminTransactions() {
    $("#transaction-table tbody").html("");

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
                appendElement.append($("<td>").html(`<a href="/Admin/RentingTransactions/edit/${value["rentingTransationId"]}">Edit</a> | `
                    + `<a href="/Admin/RentingTransactions/details/${value["rentingTransationId"]}">Details</a> | `
                    + `<a href="/Admin/RentingTransactions/delete/${value["rentingTransationId"]}">Delete</a>`));
            });
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}
