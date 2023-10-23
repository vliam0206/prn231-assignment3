using BusinessObjects.DTOs;
using BusinessObjects;
using CarRentingWebClient.Models;

namespace CarRentingWebClient.AccessAPIs.Interfaces;

public interface ICarInformationAPIs
{
    public Task<List<CarInformation>> GetCarInformationsAsync();
    public Task<List<CarInformation>> SearchCarsAsync(string keyword);
    public Task<List<CarInformation>> GetAvailableCarsAsync(RentingDate rentingDate);
    public Task<CarInformation?> GetCarInformationAsync(int id);
    public Task UpdateCarInformationAsync(int id, CarCreateDTO carDTO);
    public Task CreateCarInformationAsync(CarCreateDTO carDTO);
    public Task DeleteCarInformationAsync(int id);
}
