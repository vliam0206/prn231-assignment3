using BusinessObjects;

namespace Repositories.Interfaces;

public interface ICarRepository
{
    Task SaveCarInformationAsync(CarInformation p);
    Task UpdateCarInformationAsync(CarInformation p);
    Task DeleteCarInformationAsync(CarInformation p);
    Task SoftDeleteCarInformationAsync(CarInformation p);
    Task<CarInformation?> GetCarByIdAsync(int id);
    Task<List<CarInformation>> GetCarInformationsAsync();
    Task<List<CarInformation>> SearchCarsByNameAsync(string key);
    Task<List<CarInformation>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate);
    Task<List<Manufacturer>> GetManufacturersAsync();
    Task<Manufacturer?> GetManufacturerByIdAsync(int id);
    Task<List<Supplier>> GetSuppliersAsync();
    Task<Supplier?> GetSupplierByIdAsync(int id);
}
