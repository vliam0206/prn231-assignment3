using BusinessObjects.DTOs;
using CarRentingWebClient.Models;

namespace CarRentingWebClient.AccessAPIs.Interfaces;

public interface IAuthAPIs
{
    public Task<bool> CustomerLoginAsync(LoginDTO loginDTO);
    public Task<bool> AdminLoginAsync(LoginDTO loginDTO);
}
