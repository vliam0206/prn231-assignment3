namespace BusinessObjects.DTOs;

public class RentingDetailCreateDTO
{
    public int RentingTransactionId { get; set; }
    public int CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal? Price { get; set; }
}
