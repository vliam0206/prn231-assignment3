using BusinessObjects;

namespace CarRentingWebClient.AccessAPIs.Interfaces;

public interface ISupplierAPIs
{
    public Task<List<Supplier>> GetSuppliersAsync();
}
