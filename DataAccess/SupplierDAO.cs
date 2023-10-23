using BusinessObjects;

namespace DataAccess;

public class SupplierDAO : BaseDAO<Supplier>
{
    public static async Task<Supplier?> FindSupplierByIdAsync(int supplierId)
    {
        var supplier = new Supplier();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                supplier = await context.Suppliers.FindAsync(supplierId);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return supplier;
    }
}
