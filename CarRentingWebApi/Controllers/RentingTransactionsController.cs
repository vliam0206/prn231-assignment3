using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Repositories.Interfaces;
using AutoMapper;
using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace CarRentingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RentingTransactionsController : ControllerBase
{
    private readonly IRentingRepository _rentingRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly MyTokenHandler _tokenHandler;

    public RentingTransactionsController(IRentingRepository rentingRepository, 
                                            ICustomerRepository customerRepository,
                                            IMapper mapper,
                                            IDistributedCache cache,
                                            MyTokenHandler tokenHandler)
    {
        _rentingRepository = rentingRepository;
        _customerRepository = customerRepository;
        _mapper = mapper;
        _cache = cache;
        _tokenHandler = tokenHandler;
    }

    // GET: api/RentingTransactions
    [HttpGet]
    public async Task<ActionResult<List<RentingTransaction>>> GetRentingTransactions()
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        return await _rentingRepository.GetRentingTransactionsAsync();
    }

    // GET: api/RentingTransactions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RentingTransaction>> GetRentingTransaction(int id)
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        var rentingTransaction = await _rentingRepository.GetRentingTransactionByIdAsync(id);

        if (rentingTransaction == null)
        {
            return NotFound();
        }

        return rentingTransaction;
    }

    // PUT: api/RentingTransactions/5    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutRentingTransaction(int id, RentingCreateDTO rentingModel)
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        if ((await _rentingRepository.GetRentingTransactionByIdAsync(id)) == null)
        {
            return NotFound();
        }
        if ((await _customerRepository.GetCustomerByIdAsync(rentingModel.CustomerId)) == null)
        {
            return BadRequest("CustomerId is not exist!");
        }
        try
        {
            var rentingTransaction = _mapper.Map<RentingTransaction>(rentingModel);
            rentingTransaction.RentingTransationId = id;
            await _rentingRepository.UpdateRentingTransactionAsync(rentingTransaction);
        }
        catch (DbUpdateConcurrencyException)
        {
            if ((await _rentingRepository.GetRentingTransactionByIdAsync(id)) == null)
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/RentingTransactions    
    [HttpPost]
    public async Task<ActionResult<RentingTransaction>> PostRentingTransaction(RentingCreateDTO rentingModel)
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        if ((await _customerRepository.GetCustomerByIdAsync(rentingModel.CustomerId)) == null)
        {
            return BadRequest("CustomerId is not exist!");
        }
        var lastedTransaction = (await _rentingRepository.GetRentingTransactionsAsync())
                            .OrderBy(x => x.RentingTransationId)
                            .LastOrDefault();
        int lastedId = 1;
        if (lastedTransaction != null)
        {
            lastedId = lastedTransaction.RentingTransationId + 1;
        }
        var rentingTransaction = _mapper.Map<RentingTransaction>(rentingModel);
        rentingTransaction.RentingTransationId = lastedId;
        try
        {            
            await _rentingRepository.SaveRentingTransactionAsync(rentingTransaction);
        }
        catch (DbUpdateException)
        {
            if ((await _rentingRepository.GetRentingTransactionByIdAsync(rentingTransaction.RentingTransationId)) != null)
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetRentingTransaction", new { id = rentingTransaction.RentingTransationId }, rentingTransaction);
    }

    // DELETE: api/RentingTransactions/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRentingTransaction(int id)
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        var rentingTransaction = await _rentingRepository.GetRentingTransactionByIdAsync(id);
        if (rentingTransaction == null)
        {
            return NotFound();
        }

        await _rentingRepository.DeleteRentingTransactionAsync(rentingTransaction);

        return NoContent();
    }

    // GET: api/RentingTransactions/get-by-customer/5
    [HttpGet("get-by-customer/{customerId}")]
    public async Task<ActionResult<List<RentingTransaction>?>> GetRentingTransactionByCustomer(int customerId)
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        var rentingTransactions = await _rentingRepository.GetRentingTransactionsAsync(customerId);
        return rentingTransactions;
    }
}
