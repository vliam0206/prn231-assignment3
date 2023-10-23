using System.ComponentModel;

namespace BusinessObjects;

public partial class RentingDetail
{
    [DisplayName("Transaction Id")]
    public int RentingTransactionId { get; set; }

    [DisplayName("Car Id")]
    public int CarId { get; set; }

    [DisplayName("Start Date")]
    public DateTime StartDate { get; set; }

    [DisplayName("End Date")]
    public DateTime EndDate { get; set; }

    public decimal? Price { get; set; }

    public virtual CarInformation Car { get; set; } = null!;

    public virtual RentingTransaction RentingTransaction { get; set; } = null!;
}
