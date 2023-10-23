using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class CarInformationDAO : BaseDAO<CarInformation>
{
    public static async Task<CarInformation?> FindCarInformationByIdAsync(int CarId)
    {
        var CarInformation = new CarInformation();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                CarInformation = await context.CarInformations
                                                .Include(c => c.Manufacturer)
                                                .Include(c => c.Supplier)
                                                .Include(c => c.RentingDetails)
                                                .FirstOrDefaultAsync(x => x.CarId == CarId);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return CarInformation;
    }
    public static async Task DeleteCarInformationAsync(CarInformation p)
    {
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                var CarInformation = await context.CarInformations.FindAsync(p.CarId);
                if (CarInformation != null)
                {
                    context.CarInformations.Remove(CarInformation);
                    await context.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static async Task SoftDeleteCarInformationAsync(CarInformation p)
    {
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                var carInformation = await context.CarInformations.FindAsync(p.CarId);
                if (carInformation != null)
                {
                    carInformation.CarStatus = 0;
                    context.CarInformations.Entry(carInformation).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static async Task<List<CarInformation>> GetAllCarsAsync()
    {
        var carInformations = new List<CarInformation>();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                carInformations = await context.CarInformations
                                                .Include(c => c.Manufacturer)
                                                .Include(c => c.Supplier)
                                                .ToListAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return carInformations;
    }
    public static async Task<List<CarInformation>> FindAvailableCarsAsync(DateTime startDate, DateTime endDate)
    {
        var carInformations = new List<CarInformation>();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                var rentingCars = context.RentingDetails
                                         .Where(x => (x.StartDate >= startDate && x.StartDate <= endDate)
                                                    || (x.EndDate >= startDate && x.EndDate <= endDate))
                                         .Include(x => x.Car)
                                         .Select(x => x.Car);

                carInformations = await context.CarInformations
                                        .Except(rentingCars)
                                        .Where(x => x.CarStatus == 1)
                                        .ToListAsync();
                
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return carInformations;
    }
    public static async Task<List<CarInformation>> FindCarsByNameAsync(string key)
    {
        var cars = new List<CarInformation>();
        try
        {
            using (var context = new FucarRentingManagementContext())
            {
                cars = await context.CarInformations
                                            .Where(x => x.CarName.Contains(key))
                                            .ToListAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return cars;
    }
}
