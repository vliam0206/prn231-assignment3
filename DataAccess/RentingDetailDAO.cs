using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class RentingDetailDAO : BaseDAO<RentingDetail>
{
    public static async Task<RentingDetail?> FindRentingDetailByIdAsync(int RentingTransactionId, int carId)
    {
        var RentingDetail = new RentingDetail();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                RentingDetail = await context.RentingDetails.FirstOrDefaultAsync(x => x.RentingTransactionId == RentingTransactionId
                                                                                    && x.CarId == carId);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return RentingDetail;
    }
    public static async Task DeleteRentingDetailAsync(RentingDetail p)
    {
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                var RentingDetail = await context.RentingDetails.FirstOrDefaultAsync(x => x.RentingTransactionId == p.RentingTransactionId);
                if (RentingDetail != null)
                {
                    context.RentingDetails.Remove(RentingDetail);
                    await context.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static async Task<List<RentingDetail>> GetRentingDetailsByTransactionAsync(int transactionId)
    {
        var RentingDetail = new List<RentingDetail>();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                RentingDetail = await context.RentingDetails
                                             .Include(r => r.Car)
                                             .Include(r => r.RentingTransaction).ThenInclude(x => x.Customer)
                                             .Where(m => m.RentingTransactionId == transactionId)
                                             .ToListAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return RentingDetail;
    }
}
