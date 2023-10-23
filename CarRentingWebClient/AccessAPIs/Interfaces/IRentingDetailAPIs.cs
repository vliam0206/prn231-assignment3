using BusinessObjects;
using BusinessObjects.DTOs;

namespace CarRentingWebClient.AccessAPIs.Interfaces;

public interface IRentingDetailAPIs
{
    public Task<List<RentingDetail>> GetRentingDetailsAsync();
    public Task<List<RentingDetail>> GetRentingDetailsByTransactionAsync(int transactionId);
    public Task CreateRentingDetailAsync(List<RentingDetailCreateDTO> rentingDTOs);
}
