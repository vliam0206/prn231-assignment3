using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BusinessObjects;

public partial class RentingTransaction
{
    [DisplayName("Transaction Id")]
    public int RentingTransationId { get; set; }

    [DisplayName("Renting Date")]
    public DateTime? RentingDate { get; set; } = DateTime.Now;

    [DisplayName("Total Price")]
    public decimal? TotalPrice { get; set; } = 0;

    [DisplayName("Customer Id")]
    public int CustomerId { get; set; }

    [DisplayName("Renting Status")]
    public byte? RentingStatus { get; set; } = 1;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<RentingDetail> RentingDetails { get; set; } = new List<RentingDetail>();
}
