using AutoMapper;
using BusinessObjects;
using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories.Interfaces;

namespace CarRentingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RentingDetailsController : ControllerBase
{
    private readonly IRentingDetailRepository _rentingDetailRepository;
    private readonly IRentingRepository _rentingRepository;
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public RentingDetailsController(IRentingDetailRepository rentingDetailRepository,
                                            IRentingRepository rentingRepository,
                                            ICarRepository carRepository,
                                            IMapper mapper)
    {
        _rentingDetailRepository = rentingDetailRepository;
        _rentingRepository = rentingRepository;
        _carRepository = carRepository;
        _mapper = mapper;
    }

    // GET: api/RentingDetails
    [HttpGet]
    public async Task<ActionResult<List<RentingDetail>>> GetRentingDetails()
    {
        return await _rentingDetailRepository.GetRentingDetailsAsync();
    }

    // GET: api/RentingDetails/1
    [HttpGet("{transactionId}")]
    public async Task<ActionResult<List<RentingDetail>>> GetRentingDetailsByTransaction(int transactionId)
    {
        return await _rentingDetailRepository.GetRentingDetailByTransactionAsync(transactionId);
    }

    // POST: api/RentingDetails    
    [HttpPost]
    public async Task<ActionResult<RentingDetail>> PostRentingDetail(List<RentingDetailCreateDTO> rentingModels)
    {
        string message = "";
        foreach(var model in rentingModels)
        {
            var transaction = await _rentingRepository.GetRentingTransactionByIdAsync(model.RentingTransactionId);
            var car = await _rentingRepository.GetRentingTransactionByIdAsync(model.CarId);
            if (transaction == null)
            {
                message = $"RentingTransactionId {model.RentingTransactionId} is not exist!\n";
            }
            if (car == null)
            {
                message += $"CarId {model.CarId} is not exist!";
            }
            if (!message.IsNullOrEmpty())
            {
                return BadRequest(message);
            }
        }        

        // Check duplicate primary keys        
        foreach (var model in rentingModels)
        {
            var detailTmp = await _rentingDetailRepository.GetRentingDetailByIdAsync(model.RentingTransactionId, model.CarId);
            if (detailTmp != null)
            {
                return BadRequest($"Primary keys (RentingTransactionId, CarId) = ({model.RentingTransactionId}, {model.CarId}) is duplicated!");
            }
        }

        // Create renting detail
        var rentingDetails = _mapper.Map<List<RentingDetail>>(rentingModels);
        try
        {
            await _rentingDetailRepository.SaveRentingDetailsAsync(rentingDetails);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Created("", rentingDetails);
    }


}
