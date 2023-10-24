using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Repositories.Interfaces;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace CarRentingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CarInformationsController : ControllerBase
{
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public CarInformationsController(ICarRepository carRepository, IMapper mapper)
    {
        _carRepository = carRepository;
        _mapper = mapper;
    }

    // GET: api/CarInformations
    [HttpGet]
    public async Task<ActionResult<List<CarInformation>>> GetCarInformations()
    {
        return await _carRepository.GetCarInformationsAsync();
    }

    // GET: api/CarInformations/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CarInformation>> GetCarInformation(int id)
    {
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
        return await _carRepository.GetAvailableCarsAsync(dateDto.StartDate, dateDto.EndDate);
    }

    // GET: api/CarInformations/valid
    [HttpGet("valid")]
    public async Task<ActionResult<List<CarInformation>>> GetValidCarInformations()
    {
        return (await _carRepository.GetCarInformationsAsync())
            .Where(x => x.CarStatus ==1)
            .ToList();
    }

    // GET: api/CarInformations/search/abc
    [HttpGet("search/{keyword}")]
    public async Task<ActionResult<List<CarInformation>>> SearchCarInformations(string keyword)
    {
        return await _carRepository.SearchCarsByNameAsync(keyword.Trim());
    }
}
