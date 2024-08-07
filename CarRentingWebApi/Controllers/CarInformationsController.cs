﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Repositories.Interfaces;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace CarRentingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CarInformationsController : ControllerBase
{
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly MyTokenHandler _tokenHandler;

    public CarInformationsController(ICarRepository carRepository, IMapper mapper,
        IDistributedCache cache, MyTokenHandler tokenHandler)
    {
        _carRepository = carRepository;
        _mapper = mapper;
        _cache = cache;
        _tokenHandler = tokenHandler;
    }

    // GET: api/CarInformations
    [HttpGet]
    public async Task<ActionResult<List<CarInformation>>> GetCarInformations()
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        return await _carRepository.GetCarInformationsAsync();
    }

    // GET: api/CarInformations/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CarInformation>> GetCarInformation(int id)
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        var carInformation = await _carRepository.GetCarByIdAsync(id);

        if (carInformation == null)
        {
            return NotFound();
        }

        return carInformation;
    }

    // PUT: api/CarInformations/5    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutCarInformation(int id, CarCreateDTO carModel)
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        if ((await _carRepository.GetCarByIdAsync(id)) == null)
        {
            return NotFound();
        }
        string message = await CheckValidateCarModelAsync(carModel);
        if (!message.IsNullOrEmpty())
        {
            return BadRequest(message);
        }

        try
        {
            var carInformation = _mapper.Map<CarInformation>(carModel);
            carInformation.CarId = id;
            await _carRepository.UpdateCarInformationAsync(carInformation);
        }
        catch (DbUpdateConcurrencyException)
        {
            if ((await _carRepository.GetCarByIdAsync(id)) == null)
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

    // POST: api/CarInformations    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CarInformation>> PostCarInformation(CarCreateDTO carModel)
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        string message = await CheckValidateCarModelAsync(carModel);
        if (!message.IsNullOrEmpty())
        {
            return BadRequest(message);
        }

        var carInformation = _mapper.Map<CarInformation>(carModel);
        await _carRepository.SaveCarInformationAsync(carInformation);

        return CreatedAtAction("GetCarInformation", new { id = carInformation.CarId }, carInformation);
    }
    
    private async Task<string> CheckValidateCarModelAsync(CarCreateDTO carModel)
    {
        string message = "";
        if ((await _carRepository.GetManufacturerByIdAsync(carModel.ManufacturerId)) == null)
        {
            message = "ManufactureId is not exist!\n";
        }
        if ((await _carRepository.GetSupplierByIdAsync(carModel.SupplierId)) == null)
        {
            message += "SupplierId is not exist!";
        }
        return message;
    }

    // DELETE: api/CarInformations/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCarInformation(int id)
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        var carInformation = await _carRepository.GetCarByIdAsync(id);
        if (carInformation == null)
        {
            return NotFound();
        }
        if (carInformation.RentingDetails.Count > 0)
        {
            await _carRepository.SoftDeleteCarInformationAsync(carInformation);
        } else
        {
            await _carRepository.DeleteCarInformationAsync(carInformation);
        }        

        return NoContent();
    }

    // GET: api/CarInformations/available
    [HttpPost("available")]
    public async Task<ActionResult<List<CarInformation>>> GetAvailableCarInformations(RentingDateDTO dateDto)
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        return await _carRepository.GetAvailableCarsAsync(dateDto.StartDate, dateDto.EndDate);
    }

    // GET: api/CarInformations/valid
    [HttpGet("valid")]
    public async Task<ActionResult<List<CarInformation>>> GetValidCarInformations()
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        return (await _carRepository.GetCarInformationsAsync())
            .Where(x => x.CarStatus ==1)
            .ToList();
    }

    // GET: api/CarInformations/search/abc
    [HttpGet("search/{keyword}")]
    public async Task<ActionResult<List<CarInformation>>> SearchCarInformations(string keyword)
    {
        var currentTokenId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        var userId = int.Parse(userIdClaim!); // Get the user's ID;

        if (_tokenHandler.IsTokenRevoked(userId, currentTokenId!, _cache))
        {
            return Unauthorized("Token is revoked or invalid.");
        }

        return await _carRepository.SearchCarsByNameAsync(keyword.Trim());
    }

}
