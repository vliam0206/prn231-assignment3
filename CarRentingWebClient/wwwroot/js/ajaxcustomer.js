$(() => {
    ShowAllCustomers();
});

// load all customers
function ShowAllCustomers() {
    $("#customer-table tbody").html("");
    $.ajax({
        url: "https://localhost:7248/api/Customers",
        type: "get",
        headers: {
            Authorization: `Bearer ${localStorage.getItem("token") }`
        },
        contentType: "application/json",
        success: function (result, status, xhr) {
            $.each(result, function (index, value) {
                $("#customer-table tbody").append($("<tr>"));
                appendElement = $("#customer-table tbody tr").last();
                appendElement.append($("<td>").html(value["customerId"]));
                appendElement.append($("<td>").html(value["customerName"]));
                appendElement.append($("<td>").html(value["telephone"]));
                appendElement.append($("<td>").html(value["email"]));
                appendElement.append($("<td>").html(value["customerBirthday"]));
                appendElement.append($("<td>").html(value["customerStatus"]));
                appendElement.append($("<td>").html(`<a href="/Admin/Customers/edit/${value["customerId"]}">Edit</a> | `
                    + `<a href="/Admin/Customers/details/${value["customerId"]}">Details</a> | `
                    + `<a href="/Admin/Customers/delete/${value["customerId"]}">Delete</a>`));
                appendElement.append($("<td>").html(`<a onclick="ExpandEditCustomer(${value["customerId"]})" class="link-primary">Edit</a> | 
                <a onclick="ExpandDetailsCustomer(${value["customerId"]})" class="link-primary">Details</a> 
                <a onclick="ExpandDeleteCustomer(${value["customerId"]})" class="link-primary">Delete</a>`));
            });
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}

// search customers
function SearchCustomer() {
    $("#customer-table tbody").html("");

    $.ajax({
        url: "https://localhost:7248/api/Customers/search/" + $("#searchCustomerValue").val(),
        type: "get",
        contentType: "application/json",
        headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`
        },
        success: function (result, status, xhr) {
            $.each(result, function (index, value) {
                $("#customer-table tbody").append($("<tr>"));
                appendElement = $("#customer-table tbody tr").last();
                appendElement.append($("<td>").html(value["customerId"]));
                appendElement.append($("<td>").html(value["customerName"]));
                appendElement.append($("<td>").html(value["telephone"]));
                appendElement.append($("<td>").html(value["email"]));
                appendElement.append($("<td>").html(value["customerBirthday"]));
                appendElement.append($("<td>").html(value["customerStatus"]));
                appendElement.append($("<td>").html(`<a href="/Admin/Customers/edit/${value["customerId"]}">Edit</a> | `
                    + `<a href="/Admin/Customers/details/${value["customerId"]}">Details</a> | `
                    + `<a href="/Admin/Customers/delete/${value["customerId"]}">Delete</a>`));
                appendElement.append($("<td>").html(`<a onclick="ExpandEditCustomer(${value["customerId"]}))" class="link-primary">Edit</a> | 
                <a onclick="ExpandDetailsCustomer(${value["customerId"]})" class="link-primary">Details</a> 
                <a onclick="ExpandDeleteCustomer(${value["customerId"]})" class="link-primary">Delete</a>`));
            });
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}

// Expand create customer
function ExpandCreateCustomer() {
    $("#customerCrud").html(`<h3>Create Customer</h3>
    <div id="ErrorMessage"></div>
    <div class="row col-8 mb-2">
        <div class="col-6">
            <div class="form-group mb-3">
                <label class="control-label">Email</label>
                <input id="Email" class="form-control" type="email" />
            </div>
            <div class="form-group mb-3">
                <label class="control-label">Password</label>
                <input id="Password" class="form-control" type="password" />
            </div>
            <div class="form-group mb-3">
                <label class="control-label">ConfirmPassword</label>
                <input id="ConfirmPassword" class="form-control" type="password" />
            </div>
        </div>
        <div class="col-6">
            <div class="form-group mb-3">
                <label class="control-label">Customer Name</label>
                <input id="CustomerName" class="form-control" />
            </div>
            <div class="form-group mb-3">
                <label class="control-label">Telephone</label>
                <input id="Telephone" class="form-control" type="tel" />
            </div>
            <div class="form-group mb-3">
                <label class="control-label">Birthday</label>
                <input id="CustomerBirthday" class="form-control" type="date"/>
            </div>
        </div>
        <div class="form-group mb-3">
            <button type="submit" onclick="ValidateCreateCustomer()" class="btn btn-light btn-outline-primary col-12">Create</button>
        </div>
    </div>`);
}

// Expand details customer
function ExpandDetailsCustomer(id) {    
    $.ajax({
        url: "https://localhost:7248/api/Customers/" + id,
        type: "get",
        contentType: "application/json",
        success: function (result, status, xhr) {
            console.log(result);
            $("#customerCrud").html(`
            <h3 class="mt-3">Details Customer</h3>
            <div id="ErrorMessage"></div>
    <dl class="row mb-3">
        <dt class="col-sm-2">
            Name
        </dt>
        <dd class="col-sm-4">
            ${result["customerName"]}
        </dd>
        <dt class="col-sm-2">
            Telephone
        </dt>
        <dd class="col-sm-4">
            ${result["telephone"]}
        </dd>
        <dt class="col-sm-2">
            Email
        </dt>
        <dd class="col-sm-4">
            ${result["email"]}
        </dd>
        <dt class="col-sm-2">
            Birthday
        </dt>
        <dd class="col-sm-4">
            ${result["customerBirthday"]}
        </dd>
        <dt class="col-sm-2">
            Status
        </dt>
        <dd class="col-sm-4">
            ${result["customerStatus"]}
        </dd>
    </dl>
            `);
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}

// Expand edit customer
function ExpandEditCustomer(id) {
    $.ajax({
        url: "https://localhost:7248/api/Customers/" + id,
        type: "get",
        contentType: "application/json",
        success: function (result, status, xhr) {
            console.log(result);
            var birthday = new Date(result["customerBirthday"]);
            console.log("Birthday: " + birthday);

            $("#customerCrud").html(`
            <h3 class="mt-3">Edit Customer</h3>
            <div id="ErrorMessage"></div>
    <div class="row col-8 mb-2">
        <div class="col-6">
        <div class="form-group mb-3">
                <label class="control-label">Customer Name</label>
                <input id="CustomerName" class="form-control" value="${result["customerName"]}" />
            </div>
            <div class="form-group mb-3">
                <label class="control-label">Email</label>
                <input id="Email" class="form-control" type="email" value="${result["email"]}"/>
            </div>
            <div class="form-group mb-3">
                <input id="Password" class="form-control" type="hidden" value="${result["password"]}" />
            </div>
        </div>
        <div class="col-6">            
            <div class="form-group mb-3">
                <label class="control-label">Telephone</label>
                <input id="Telephone" class="form-control" type="tel" value="${result["telephone"]}" />
            </div>
            <div class="form-group mb-3">
                <label class="control-label">Birthday</label>
                <input id="CustomerBirthday" class="form-control" type="date" value=${birthday}/>
            </div>
        </div>
        <div class="form-group mb-3">
            <button type="submit" onclick="EditCustomer(${id})" class="btn btn-light btn-outline-primary col-12">Update</button>
        </div>
    </div>
            `);
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}

// Expand delete customer
function ExpandDeleteCustomer(id) {
    $.ajax({
        url: "https://localhost:7248/api/Customers/" + id,
        type: "get",
        contentType: "application/json",
        success: function (result, status, xhr) {
            console.log(result);
            $("#customerCrud").html(`
            <h3 class="mt-3">Do you want to delete customer?</h3>
            <div id="ErrorMessage"></div>
    <dl class="row mb-3">
        <dt class="col-sm-2">
            Name
        </dt>
        <dd class="col-sm-4">
            ${result["customerName"]}
        </dd>
        <dt class="col-sm-2">
            Telephone
        </dt>
        <dd class="col-sm-4">
            ${result["telephone"]}
        </dd>
        <dt class="col-sm-2">
            Email
        </dt>
        <dd class="col-sm-4">
            ${result["email"]}
        </dd>
        <dt class="col-sm-2">
            Birthday
        </dt>
        <dd class="col-sm-4">
            ${result["customerBirthday"]}
        </dd>
        <dt class="col-sm-2">
            Status
        </dt>
        <dd class="col-sm-4">
            ${result["customerStatus"]}
        </dd>
    </dl>
    <div class=col-8>
        <div class="form-group mb-3">
            <button type="submit" onclick="DeleteCustomer(${id})" class="btn btn-danger btn-outline-light col-12">Delete</button>
        </div>
    </div>            
            `);
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}

// Reset customer
function ResetCustomer() {
    $("#customerCrud").html(``);
}

// Delete customer
function DeleteCustomer(id) {
    $.ajax({
        url: "https://localhost:7248/api/Customers/" + id,
        type: "delete",
        contentType: "application/json",
        headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`
        },
        success: function (result, status, xhr) {
            ShowAllCustomers();
            $("#customerCrud").html(``);
        },
        error: function (xhr, status, error) {
            console.log(xhr);
        }
    });
}

// validate create customer
function ValidateCreateCustomer() {
    var password = $("#Password").val();
    var confirmPassword = $("#ConfirmPassword").val();
    var date = $("#CustomerBirthday").val();
    if (date == "") {
        $("#ErrorMessage").html(`<h3 class="text-danger">Error</h3><p class="text-danger">Customer birthday is required.</p>`);
    } else if (password === confirmPassword) {
        CreateCustomer();
    } else {
        $("#ErrorMessage").html(`<h3 class="text-danger">Error</h3><p class="text-danger">The confirm password not match the password.</p>`);
    }
}

// Create customer
function CreateCustomer() {
    $.ajax({
        url: "https://localhost:7248/api/Customers/",
        headers: {
            Key: "Secrect@123"
        },
        type: "post",
        contentType: "application/json",
        headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`
        },
        data: JSON.stringify({
            CustomerName: $("#CustomerName").val(),
            Telephone: $("#Telephone").val(),
            Email: $("#Email").val(),
            CustomerBirthday: $("#CustomerBirthday").val(),
            CustomerStatus: $("#CustomerStatus").val(),
            Password: $("#Password").val()
        }),
        success: function (result, status, xhr) {
            $("#customerCrud").html(``);
            ShowAllCustomers();
        },
        error: function (xhr, status, error) {
            if (xhr.responseJSON && xhr.responseJSON.errors) {
                // Handle the JSON error response
                var errors = xhr.responseJSON.errors;
                var errorHtml = '';

                for (var key in errors) {
                    if (errors.hasOwnProperty(key)) {
                        var errorMessages = errors[key];
                        errorHtml += `<strong>${key}:</strong> ${errorMessages.join(', ')}<br>`;
                    }
                }

                $("#ErrorMessage").html(`<h3 class="text-danger">Error</h3><p class="text-danger">${errorHtml}</p>`);
            } else {
                // Fallback to responseText if no JSON error response
                $("#ErrorMessage").html(`<h3 class="text-danger">Error</h3><p class="text-danger">${xhr.responseText}</p>`);
            }
            console.log(xhr);
        }
    });
}

// Edit customer
function EditCustomer(id) {
    $.ajax({
        url: "https://localhost:7248/api/Customers/" + id,
        headers: {
            Key: "Secrect@123"
        },
        type: "put",
        contentType: "application/json",
        headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`
        },
        data: JSON.stringify({
            CustomerName: $("#CustomerName").val(),
            Telephone: $("#Telephone").val(),
            Email: $("#Email").val(),
            CustomerBirthday: $("#CustomerBirthday").val(),
            CustomerStatus: $("#CustomerStatus").val(),
            Password: $("#Password").val()
        }),
        success: function (result, status, xhr) {
            $("#customerCrud").html(``);
            ShowAllCustomers();
        },
        error: function (xhr, status, error) {
            if (xhr.responseJSON && xhr.responseJSON.errors) {
                // Handle the JSON error response
                var errors = xhr.responseJSON.errors;
                var errorHtml = '';

                for (var key in errors) {
                    if (errors.hasOwnProperty(key)) {
                        var errorMessages = errors[key];
                        errorHtml += `<strong>${key}:</strong> ${errorMessages.join(', ')}<br>`;
                    }
                }

                $("#ErrorMessage").html(`<h3 class="text-danger">Error</h3><p class="text-danger">${errorHtml}</p>`);
            } else {
                // Fallback to responseText if no JSON error response
                $("#ErrorMessage").html(`<h3 class="text-danger">Error</h3><p class="text-danger">${xhr.responseText}</p>`);
            }
            console.log(xhr);
        }
    });
}