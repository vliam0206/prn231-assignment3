using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects;

public partial class CarInformation
{
    public int CarId { get; set; }

    [DisplayName("Car Name")]
    [Required(ErrorMessage = "Car name is required!")]
    public string CarName { get; set; } = null!;

    [DisplayName("Car Description")]
    [Required(ErrorMessage = "Car Description is required!")]
    public string? CarDescription { get; set; }

    [DisplayName("Doors")]
    [Required(ErrorMessage = "Number Of Doors is required!")]
    [Range(1, 5, ErrorMessage = "Number Of Doors must be from 1 to 5")]
    public int? NumberOfDoors { get; set; }

    [DisplayName("Capacity")]
    [Required(ErrorMessage = "Seating Capacity is required!")]
    [Range(1, int.MaxValue, ErrorMessage = "Seating Capacity must be greater than 0.")]
    public int? SeatingCapacity { get; set; }

    [DisplayName("FuelType")]
    [Required(ErrorMessage = "FuelType is required!")]
    public string? FuelType { get; set; }

    [DisplayName("Year")]
    [Required(ErrorMessage = "Relase Year is required!")]
    [Range(1870, 2023, ErrorMessage = "Relase Year must from 1870 to 2023")]
    public int? Year { get; set; }

    [DisplayName("Manufacturer")]
    [Required(ErrorMessage = "Manufacturer is required!")]
    public int ManufacturerId { get; set; }

    [DisplayName("Supplier")]
    [Required(ErrorMessage = "Supplier is required!")]
    public int SupplierId { get; set; }

    [DisplayName("Status")]
    public byte? CarStatus { get; set; } = 1;

    [DisplayName("Price Per Day ")]
    [Range(0, double.MaxValue, ErrorMessage = "The price must be greater than 0.")]
    public decimal? CarRentingPricePerDay { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<RentingDetail> RentingDetails { get; set; } = new List<RentingDetail>();

    public virtual Supplier Supplier { get; set; } = null!;
}
