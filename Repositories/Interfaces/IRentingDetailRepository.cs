using BusinessObjects;

namespace Repositories.Interfaces;

public interface IRentingDetailRepository
{
    Task SaveRentingDetailAsync(RentingDetail p);
    Task SaveRentingDetailsAsync(List<RentingDetail> ls);
    Task UpdateRentingDetailAsync(RentingDetail p);
    Task DeleteRentingDetailAsync(RentingDetail p);
    Task<RentingDetail?> GetRentingDetailByIdAsync(int transactionId, int carId);
    Task<List<RentingDetail>> GetRentingDetailByTransactionAsync(int transactionId);
    Task<List<RentingDetail>> GetRentingDetailsAsync();
}
