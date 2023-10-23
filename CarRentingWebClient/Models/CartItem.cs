using BusinessObjects;

namespace CarRentingWebClient.Models;

public class CartItem
{
    public CarInformation CarInfo { get; set; } = default!;
    public SortedList<DateTime,RentingDate> RentingDateInfo { get; set; } = default!;
}
