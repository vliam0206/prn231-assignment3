using AutoMapper;
using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using WebAPI;

namespace CarRentingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public AuthController(ICustomerRepository customerRepository, 
        IConfiguration config,
        IMapper mapper)
    {
        _customerRepository = customerRepository;
        _config = config;
        _mapper = mapper;
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

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        if (_customerRepository.AdminLogin(dto.Email, dto.Password))
        {
            var adminAccount = new Account { Id = -1, Email = dto.Email, Role = "Admin" };
            var token = adminAccount.GenerateJsonWebToken(_config["JwtConfiguration:SecretKey"]!, DateTime.Now);
            return Ok( new LoginResponse
            {
                Token = token,
                AccountId = adminAccount.Id,
                Email = adminAccount.Email,
                Name = "Admin",
                Role = adminAccount.Role
            });
        }

        var customer = await _customerRepository.GetCustomerByEmailAsync(dto.Email);
        if (customer == null || !customer.Password.Equals(dto.Password))
        {
            return BadRequest("Wrong username/password");
        }
        var customerAccount = _mapper.Map<Account>(customer);
        var cusToken = customerAccount.GenerateJsonWebToken(_config["JwtConfiguration:SecretKey"]!, DateTime.Now);
        return Ok(new LoginResponse
        {
            Token = cusToken,
            AccountId= customerAccount.Id,
            Email = customerAccount.Email,
            Name = customer.CustomerName,
            Role = customerAccount.Role,
            Status = customer.CustomerStatus.Value
        });
    }
}
