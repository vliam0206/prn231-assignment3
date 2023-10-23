using BusinessObjects.DTOs;
using BusinessObjects;

namespace CarRentingWebClient.AccessAPIs.Interfaces;

public interface IRentingTransactionAPIs
{
    public Task<List<RentingTransaction>> GetRentingTransactionsAsync();
    public Task<RentingTransaction?> GetRentingTransactionAsync(int id);
    public Task<List<RentingTransaction>> GetRentingTransactionByCustomerAsync(int customerId);
    public Task UpdateRentingTransactionAsync(int id, RentingCreateDTO rentingDTO);
    public Task<RentingTransaction> CreateRentingTransactionAsync(RentingCreateDTO rentingDTO);
    public Task DeleteRentingTransactionAsync(int id);
}
