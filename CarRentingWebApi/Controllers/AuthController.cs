using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace CarRentingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public AuthController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    // GET: api/Auth/customer-login
    [HttpPost("customer-login")]
    public async Task<bool> CustomerLogin([FromBody]LoginDTO loginModel)
    {
        return await _customerRepository.CustomerLoginAsync(loginModel.Email, loginModel.Password);        
    }

    // GET: api/Auth/admin-login
    [HttpPost("admin-login")]
    public bool AdminLogin([FromBody]LoginDTO loginModel)
    {
        return _customerRepository.AdminLogin(loginModel.Email, loginModel.Password);
    }
}
