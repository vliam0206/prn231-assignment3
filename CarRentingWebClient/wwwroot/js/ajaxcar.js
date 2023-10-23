$(() => {
    ShowAllCars();
});

// load all Cars
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
                appendElement.append($("<td>").html(value["carStatus"]));
                appendElement.append($("<td>").html(value["carRentingPricePerDay"]));
                appendElement.append($("<td>").html(value["manufacturer"].manufacturerName));
                appendElement.append($("<td>").html(value["supplier"].supplierName));
                appendElement.append($("<td>").html(`<a href="/Admin/CarInformations/edit/${value["carId"]}">Edit</a> | `
                    + `<a href="/Admin/CarInformations/details/${value["carId"]}">Details</a> | `
                    + `<a href="/Admin/CarInformations/delete/${value["carId"]}">Delete</a>`));
                appendElement.append($("<td>").html(`<a onclick="ExpandEditCar(${value["carId"]})" class="link-primary">Edit</a> | 
                <a onclick="ExpandDetailsCar(${value["carId"]})" class="link-primary">Details</a> 
                <a onclick="ExpandDeleteCar(${value["carId"]})" class="link-primary">Delete</a>`));
            });
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}

// search Cars
function SearchCar() {
    $("#car-table tbody").html("");

    $.ajax({
        url: "https://localhost:7248/api/CarInformations/search/" + $("#searchCarValue").val(),
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
                appendElement.append($("<td>").html(value["carStatus"]));
                appendElement.append($("<td>").html(value["carRentingPricePerDay"]));
                appendElement.append($("<td>").html(value["manufacturer"].manufacturerName));
                appendElement.append($("<td>").html(value["supplier"].supplierName));
                appendElement.append($("<td>").html(`<a href="/Admin/CarInformations/edit/${value["carId"]}">Edit</a> | `
                    + `<a href="/Admin/CarInformations/details/${value["carId"]}">Details</a> | `
                    + `<a href="/Admin/CarInformations/delete/${value["carId"]}">Delete</a>`));
                appendElement.append($("<td>").html(`<a onclick="ExpandEditCar(${value["carId"]})" class="link-primary">Edit</a> | 
                <a onclick="ExpandDetailsCar(${value["carId"]})" class="link-primary">Details</a> 
                <a onclick="ExpandDeleteCar(${value["carId"]})" class="link-primary">Delete</a>`));
            });
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}

// Expand create Car
function ExpandCreateCar() {
    $("#carCrud").html(`<h3>Create Car</h3>
    <div id="ErrorMessage"></div>
    <div class="row col-8 mb-2">
        <div class="col-6">
                <div class="form-group mb-3">
                    <label class="control-label">Car Name</label>
                    <input id="CarName" class="form-control" name="CarName"/>
                </div>
                <div class="form-group mb-3">
                    <label class="control-label">Description</label>
                    <input id="CarDescription" class="form-control" name="CarDescription" />                    
                </div>
                <div class="form-group mb-3">
                    <label class="control-label">Number Of Doors</label>
                    <input id="NumberOfDoors" class="form-control" name="NumberOfDoors" />
                </div>
                <div class="form-group mb-3">
                    <label class="control-label">Capacity</label>
                    <input id="SeatingCapacity" class="form-control" name="SeatingCapacity" />
                </div>
                <div class="form-group mb-3">
                    <label class="control-label">Fuel Type</label>
                    <input id="FuelType" class="form-control" name="FuelType" />
                </div>                                
        </div>
        <div class="col-6">
            <div class="form-group mb-3">
                <label class="control-label">Year</label>
                <input id="Year" class="form-control" name="Year" />
            </div>
            <div class="form-group mb-3">
                <label class="control-label">Manufacturer</label>
                <select id="ManufacturerId" class="form-control" name="ManufacturerId">
                    <option value="1">Toyota</option>
                    <option value="2">BMW</option>
                    <option value="3">Ford</option>
                    <option value="4">Volkswagen</option>
                    <option value="5">Honda</option>
                </select>
            </div>
            <div class="form-group mb-3">
                <label class="control-label">Supplier</label>
                <select id="SupplierId" class="form-control" name="SupplierId">
                    <option value="1">Aisin Asia Pacific Co., Ltd.</option>
                    <option value="2">Denso Vietnam Co., Ltd.</option>
                    <option value="3">Toyota Tsusho Vietnam Co., Ltd.</option>
                    <option value="4">Bosch Vietnam Co., Ltd.</option>
                    <option value="5">Lear Corporation Vietnam Co., Ltd.</option>
                    <option value="6">VinFast</option>
                    <option value="7">Schaeffler (Vietnam) Co., Ltd.</option>
                </select>
            </div>
            <div class="form-group mb-3">
                <label class="control-label">Car Status</label>
                <input id="CarStatus" class="form-control" name="CarStatus" />
            </div>
            <div class="form-group mb-3">
                <label class="control-label">Price Per Day</label>
                <input id="CarRentingPricePerDay" class="form-control" name="CarRentingPricePerDay" />
            </div>
        </div>
        <div class="form-group mb-3">
            <button type="submit" onclick="CreateCar()" class="btn btn-light btn-outline-primary col-12">Create</button>
        </div>  
    </div>`);
}

// Expand details Car
function ExpandDetailsCar(id) {
    $.ajax({
        url: "https://localhost:7248/api/CarInformations/" + id,
        type: "get",
        contentType: "application/json",
        success: function (result, status, xhr) {
            console.log(result);
            $("#carCrud").html(`
            <h3 class="mt-3">Details Car</h3>
            <div id="ErrorMessage"></div>
    <dl class="row mb-3">
        <dt class = "col-sm-2">
            Car Name
        </dt>
        <dd class = "col-sm-4">
            ${result["carName"]}
        </dd>
        <dt class = "col-sm-2">
            Description
        </dt>
        <dd class = "col-sm-4">
            ${result["carDescription"]}
        </dd>
        <dt class = "col-sm-2">
            Number Of Doors
        </dt>
        <dd class = "col-sm-4">
            ${result["numberOfDoors"]}
        </dd>
        <dt class = "col-sm-2">
            Seating Capacity
        </dt>
        <dd class = "col-sm-4">
            ${result["seatingCapacity"]}
        </dd>
        <dt class = "col-sm-2">
            Fuel Type
        </dt>
        <dd class = "col-sm-4">
            ${result["fuelType"]}
        </dd>
        <dt class = "col-sm-2">
            Year
        </dt>
        <dd class = "col-sm-4">
            ${result["year"]}
        </dd>
        <dt class = "col-sm-2">
            Car Status
        </dt>
        <dd class = "col-sm-4">
            ${result["carStatus"]}
        </dd>
        <dt class = "col-sm-2">
            Price Per Day
        </dt>
        <dd class = "col-sm-4">
            ${result["carRentingPricePerDay"]}
        </dd>
        <dt class = "col-sm-2">
            Manufacturer
        </dt>
        <dd class = "col-sm-4">
            ${result["manufacturer"].manufacturerName}
        </dd>
        <dt class = "col-sm-2">
            Supplier
        </dt>
        <dd class = "col-sm-4">
            ${result["supplier"].supplierName}
        </dd>
    </dl>
            `);
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}

// Expand edit Car
function ExpandEditCar(id) {
    $.ajax({
        url: "https://localhost:7248/api/CarInformations/" + id,
        type: "get",
        contentType: "application/json",
        success: function (result, status, xhr) {
            $("#carCrud").html(`
            <h3 class="mt-3">Edit Car</h3>
            <div id="ErrorMessage"></div>
    <div class="row col-8 mb-2">
        <div class="col-6">
                <div class="form-group mb-3">
                    <label class="control-label">Car Name</label>
                    <input id="CarName" class="form-control" name="CarName" value="${result["carName"]}"/>
                </div>
                <div class="form-group mb-3">
                    <label class="control-label">Description</label>
                    <input id="CarDescription" class="form-control" name="CarDescription" value="${result["carDescription"]}" />                    
                </div>
                <div class="form-group mb-3">
                    <label class="control-label">Number Of Doors</label>
                    <input id="NumberOfDoors" class="form-control" name="NumberOfDoors" value="${result["numberOfDoors"]}" />
                </div>
                <div class="form-group mb-3">
                    <label class="control-label">Capacity</label>
                    <input id="SeatingCapacity" class="form-control" name="SeatingCapacity" value="${result["seatingCapacity"]}" />
                </div>
                <div class="form-group mb-3">
                    <label class="control-label">Fuel Type</label>
                    <input id="FuelType" class="form-control" name="FuelType" value="${result["fuelType"]}" />
                </div>                                
        </div>
        <div class="col-6">
            <div class="form-group mb-3">
                <label class="control-label">Year</label>
                <input id="Year" class="form-control" name="Year" value="${result["year"]}" />
            </div>
            <div class="form-group mb-3">
                <label class="control-label">Manufacturer</label>
                <select id="ManufacturerId" class="form-control" name="ManufacturerId" value="${result["manufacturerId"]}">
                    <option value="1">Toyota</option>
                    <option value="2">BMW</option>
                    <option value="3">Ford</option>
                    <option value="4">Volkswagen</option>
                    <option value="5">Honda</option>
                </select>
            </div>
            <div class="form-group mb-3">
                <label class="control-label">Supplier</label>
                <select id="SupplierId" class="form-control" name="SupplierId" value="${result["supplierId"]}">
                    <option value="1">Aisin Asia Pacific Co., Ltd.</option>
                    <option value="2">Denso Vietnam Co., Ltd.</option>
                    <option value="3">Toyota Tsusho Vietnam Co., Ltd.</option>
                    <option value="4">Bosch Vietnam Co., Ltd.</option>
                    <option value="5">Lear Corporation Vietnam Co., Ltd.</option>
                    <option value="6">VinFast</option>
                    <option value="7">Schaeffler (Vietnam) Co., Ltd.</option>
                </select>
            </div>
            <div class="form-group mb-3">
                <label class="control-label">Car Status</label>
                <input id="CarStatus" class="form-control" name="CarStatus" value="${result["carStatus"]}" />
            </div>
            <div class="form-group mb-3">
                <label class="control-label">Price Per Day</label>
                <input id="CarRentingPricePerDay" class="form-control" name="CarRentingPricePerDay" value="${result["carRentingPricePerDay"]}" />
            </div>
        </div>
        <div class="form-group mb-3">
            <button type="submit" onclick="EditCar(${result["carId"]})" class="btn btn-light btn-outline-primary col-12">Update</button>
        </div>  
    </div>
            `);
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}

// Expand delete Car
function ExpandDeleteCar(id) {
    $.ajax({
        url: "https://localhost:7248/api/CarInformations/" + id,
        type: "get",
        contentType: "application/json",
        success: function (result, status, xhr) {
            console.log(result);
            $("#carCrud").html(`
            <h3 class="mt-3">Do you want to delete Car?</h3>
            <div id="ErrorMessage"></div>
    <dl class="row mb-3">
        <dt class = "col-sm-2">
            Car Name
        </dt>
        <dd class = "col-sm-4">
            ${result["carName"]}
        </dd>
        <dt class = "col-sm-2">
            Description
        </dt>
        <dd class = "col-sm-4">
            ${result["carDescription"]}
        </dd>
        <dt class = "col-sm-2">
            Number Of Doors
        </dt>
        <dd class = "col-sm-4">
            ${result["numberOfDoors"]}
        </dd>
        <dt class = "col-sm-2">
            Seating Capacity
        </dt>
        <dd class = "col-sm-4">
            ${result["seatingCapacity"]}
        </dd>
        <dt class = "col-sm-2">
            Fuel Type
        </dt>
        <dd class = "col-sm-4">
            ${result["fuelType"]}
        </dd>
        <dt class = "col-sm-2">
            Year
        </dt>
        <dd class = "col-sm-4">
            ${result["year"]}
        </dd>
        <dt class = "col-sm-2">
            Car Status
        </dt>
        <dd class = "col-sm-4">
            ${result["carStatus"]}
        </dd>
        <dt class = "col-sm-2">
            Price Per Day
        </dt>
        <dd class = "col-sm-4">
            ${result["carRentingPricePerDay"]}
        </dd>
        <dt class = "col-sm-2">
            Manufacturer
        </dt>
        <dd class = "col-sm-4">
            ${result["manufacturer"].manufacturerName}
        </dd>
        <dt class = "col-sm-2">
            Supplier
        </dt>
        <dd class = "col-sm-4">
            ${result["supplier"].supplierName}
        </dd>
    </dl>
    <div class=col-8>
        <div class="form-group mb-3">
            <button type="submit" onclick="DeleteCar(${id})" class="btn btn-danger btn-outline-light col-12">Delete</button>
        </div>
    </div>            
            `);
        },
        error: function (xhr, status, error) {
            console.log(xhr)
        }
    });
}

// Reset Car
function ResetCar() {
    $("#carCrud").html(``);
}

// Delete Car
function DeleteCar(id) {
    $.ajax({
        url: "https://localhost:7248/api/CarInformations/" + id,
        type: "delete",
        contentType: "application/json",
        success: function (result, status, xhr) {
            ShowAllCars();
            $("#carCrud").html(``);
        },
        error: function (xhr, status, error) {
            console.log(xhr);
        }
    });
}

// Create Car
function CreateCar() {
    $.ajax({
        url: "https://localhost:7248/api/CarInformations/",
        headers: {
            Key: "Secrect@123"
        },
        type: "post",
        contentType: "application/json",
        data: JSON.stringify({
            CarName: $("#CarName").val(),
            CarDescription: $("#CarDescription").val(),
            NumberOfDoors: $("#NumberOfDoors").val(),
            SeatingCapacity: $("#SeatingCapacity").val(),
            FuelType: $("#FuelType").val(),
            Year: $("#Year").val(),
            ManufacturerId: $("#ManufacturerId").val(),
            SupplierId: $("#SupplierId").val(),
            CarStatus: $("#CarStatus").val(),
            CarRentingPricePerDay: $("#CarRentingPricePerDay").val()
        }),
        success: function (result, status, xhr) {
            $("#carCrud").html(``);
            ShowAllCars();
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

// Edit Car
function EditCar(id) {
    $.ajax({
        url: "https://localhost:7248/api/CarInformations/" + id,
        headers: {
            Key: "Secrect@123"
        },
        type: "put",
        contentType: "application/json",
        data: JSON.stringify({
            CarName: $("#CarName").val(),
            CarDescription: $("#CarDescription").val(),
            NumberOfDoors: $("#NumberOfDoors").val(),
            SeatingCapacity: $("#SeatingCapacity").val(),
            FuelType: $("#FuelType").val(),
            Year: $("#Year").val(),
            ManufacturerId: $("#ManufacturerId").val(),
            SupplierId: $("#SupplierId").val(),
            CarStatus: $("#CarStatus").val(),
            CarRentingPricePerDay: $("#CarRentingPricePerDay").val()
        }),
        success: function (result, status, xhr) {
            $("#carCrud").html(``);
            ShowAllCars();
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