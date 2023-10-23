using BusinessObjects;

namespace CarRentingWebClient.AccessAPIs.Interfaces;

public interface IManufacturerAPIs
{
    public Task<List<Manufacturer>> GetManufacturersAsync();
}
