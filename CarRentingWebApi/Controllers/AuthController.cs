using AutoMapper;
using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using WebAPI;

namespace CarRentingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly MyTokenHandler _tokenHandler;
    public AuthController(ICustomerRepository customerRepository, 
        IConfiguration config,
        IMapper mapper,
        IDistributedCache cache,
        MyTokenHandler tokenHandler)
    {
        _customerRepository = customerRepository;
        _config = config;
        _mapper = mapper;
        _cache = cache;
        _tokenHandler = tokenHandler;
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
            
            // check if user have using token -> revoke it
            var key = $"{adminAccount.Id}_using_tokens";
            var usingTokenId = _cache.GetString(key);
            if (!string.IsNullOrEmpty(usingTokenId))
            { // user have using token
                _tokenHandler.RevokeToken(adminAccount.Id, usingTokenId, _cache);
                _cache.SetString(key, "");
            }

            var token = adminAccount.GenerateJsonWebToken(_config["JwtConfiguration:SecretKey"]!, DateTime.Now);

            // Parse the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            // Retrieve the Jti claim
            string jti = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti)?.Value;
            // add using token id in cache              
            _cache.SetString(key, jti!);

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

        // check if user have using token -> revoke it
        var cuskey = $"{customerAccount.Id}_using_tokens";
        var cususingTokenId = _cache.GetString(cuskey);
        if (!string.IsNullOrEmpty(cususingTokenId))
        { // user have using token
            _tokenHandler.RevokeToken(customerAccount.Id, cususingTokenId, _cache);
            _cache.SetString(cuskey, "");
        }

        var cusToken = customerAccount.GenerateJsonWebToken(_config["JwtConfiguration:SecretKey"]!, DateTime.Now);

        // Parse the token
        var custokenHandler = new JwtSecurityTokenHandler();
        var cusjwtToken = custokenHandler.ReadToken(cusToken) as JwtSecurityToken;
        // Retrieve the Jti claim
        string cusjti = cusjwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti)?.Value;
        // add using token id in cache              
        _cache.SetString(cuskey, cusjti!);

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
