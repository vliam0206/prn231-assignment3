using BusinessObjects;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories.Repos;

public class RentingDetailRepository : IRentingDetailRepository
{
    public async Task DeleteRentingDetailAsync(RentingDetail p)
        => await RentingDetailDAO.DeleteRentingDetailAsync(p);
    public async Task<RentingDetail?> GetRentingDetailByIdAsync(int transactionId, int carId)
        => await RentingDetailDAO.FindRentingDetailByIdAsync(transactionId, carId);

    public async Task<List<RentingDetail>> GetRentingDetailByTransactionAsync(int transactionId)
        => await RentingDetailDAO.GetRentingDetailsByTransactionAsync(transactionId);

    public async Task<List<RentingDetail>> GetRentingDetailsAsync() => await RentingDetailDAO.GetEntitiesAsync();
    public async Task SaveRentingDetailAsync(RentingDetail p) => await RentingDetailDAO.SaveEntityAsync(p);

    public async Task SaveRentingDetailsAsync(List<RentingDetail> ls)
        => await RentingDetailDAO.SaveRangeAsync(ls);

    public async Task UpdateRentingDetailAsync(RentingDetail p) => await RentingDetailDAO.UpdateEntityAsync(p);
}
