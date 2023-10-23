using BusinessObjects;

namespace DataAccess;

public class ManufacturerDAO : BaseDAO<Manufacturer>
{
    public static async Task<Manufacturer?> FindManufacturerByIdAsync(int manufacturerId)
    {
        var manufacture = new Manufacturer();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                manufacture = await context.Manufacturers.FindAsync(manufacturerId);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return manufacture;
    }
}
