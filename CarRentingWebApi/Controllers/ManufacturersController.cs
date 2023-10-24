using Microsoft.AspNetCore.Mvc;
using BusinessObjects;
using Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CarRentingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ManufacturersController : ControllerBase
{
    private readonly ICarRepository _carRepository;

    public ManufacturersController(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    // GET: api/Manufacturers
    [HttpGet]
    public async Task<ActionResult<List<Manufacturer>>> GetManufacturers()
    {
        return await _carRepository.GetManufacturersAsync();
    }
}
