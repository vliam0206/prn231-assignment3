using Microsoft.AspNetCore.Mvc;
using BusinessObjects;
using Repositories.Interfaces;

namespace CarRentingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
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
