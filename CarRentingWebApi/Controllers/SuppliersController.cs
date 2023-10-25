using Microsoft.AspNetCore.Mvc;
using BusinessObjects;
using Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace CarRentingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly ICarRepository _carRepository;
    private readonly IDistributedCache _cache;
    private readonly MyTokenHandler _tokenHandler;

    public SuppliersController(ICarRepository carRepository,
                                IDistributedCache cache,
                                MyTokenHandler tokenHandler)
    {
        _carRepository = carRepository;
        _cache = cache;
        _tokenHandler = tokenHandler;
    }

    // GET: api/Suppliers
    [HttpGet]
    public async Task<ActionResult<List<Supplier>>> GetSuppliers()
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        return await _carRepository.GetSuppliersAsync();
    }
}
