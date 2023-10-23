using BusinessObjects;
using DataAccess;
using Repositories.Interfaces;

namespace Repositories.Repos;

public class CarRepository : ICarRepository
{
    public async Task DeleteCarInformationAsync(CarInformation p) 
        => await CarInformationDAO.DeleteCarInformationAsync(p);

    public async Task<List<CarInformation>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate)
        => await CarInformationDAO.FindAvailableCarsAsync(startDate, endDate);

    public async Task<CarInformation?> GetCarByIdAsync(int id) 
        => await CarInformationDAO.FindCarInformationByIdAsync(id);

    public async Task<List<CarInformation>> GetCarInformationsAsync() => await CarInformationDAO.GetAllCarsAsync();

    public async Task<Manufacturer?> GetManufacturerByIdAsync(int id)
        => await ManufacturerDAO.FindManufacturerByIdAsync(id);

    public async Task<List<Manufacturer>> GetManufacturersAsync() => await ManufacturerDAO.GetEntitiesAsync();

    public async Task<Supplier?> GetSupplierByIdAsync(int id)
        => await SupplierDAO.FindSupplierByIdAsync(id);

    public async Task<List<Supplier>> GetSuppliersAsync() => await SupplierDAO.GetEntitiesAsync();

    public async Task SaveCarInformationAsync(CarInformation p) => await CarInformationDAO.SaveEntityAsync(p);

    public async Task<List<CarInformation>> SearchCarsByNameAsync(string key)
        => await CarInformationDAO.FindCarsByNameAsync(key);

    public async Task SoftDeleteCarInformationAsync(CarInformation p)
    {
        await CarInformationDAO.SoftDeleteCarInformationAsync(p);
    }

    public async Task UpdateCarInformationAsync(CarInformation p) => await CarInformationDAO.UpdateEntityAsync(p);
}
