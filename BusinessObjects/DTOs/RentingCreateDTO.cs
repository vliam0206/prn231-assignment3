namespace BusinessObjects.DTOs;

public class RentingCreateDTO
{
    public DateTime? RentingDate { get; set; } = DateTime.Now;
    public decimal? TotalPrice { get; set; } = 0;
    public int CustomerId { get; set; }
    public byte? RentingStatus { get; set; } = 1;
}
