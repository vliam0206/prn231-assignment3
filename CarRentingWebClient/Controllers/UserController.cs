using AutoMapper;
using BusinessObjects;
using BusinessObjects.DTOs;
using CarRentingWebClient.AccessAPIs.Interfaces;
using CarRentingWebClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRentingWebClient.Controllers;

[Authorize(Roles = "User")]
public class UserController : Controller
{
    private readonly ICustomerAPIs _customerAPIs;
    private readonly ICarInformationAPIs _carAPIs;
    private readonly IRentingTransactionAPIs _transactionAPIs;
    private readonly IRentingDetailAPIs _detailAPIs;
    private readonly IMapper _mapper;
    public UserController(ICustomerAPIs customerAPIs, 
                                ICarInformationAPIs carAPIs,
                                IMapper mapper, 
                                IRentingTransactionAPIs transactionAPIs, 
                                IRentingDetailAPIs detailAPIs)
    {
        _customerAPIs = customerAPIs;
        _carAPIs = carAPIs;
        _mapper = mapper;
        _transactionAPIs = transactionAPIs;
        _detailAPIs = detailAPIs;
    }

    [TempData]
    public string? Message { get; set; }

    public IActionResult Index()
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;        
        return View();
    }

    // Get: User/Renting
    public IActionResult Renting()
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        ViewData["Message"] = Message;
        return View("Renting");
    }

    // Get: User/CarList?startDate=31/1/2023&endDate=1/2/2023
    public async Task<IActionResult> CarList([FromQuery] RentingDate rentingDate)
    {
        var startDate = rentingDate.StartDate;
        var endDate = rentingDate.EndDate;

        if (startDate < DateTime.Now || endDate < DateTime.Now || startDate > endDate)
        {
            Message = "Invalid date! \n Valid date must be: Now < StartDate < EndDate";
            return RedirectToAction("Renting");
        }
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        ViewData["startdate"] = startDate;
        ViewData["enddate"] = endDate;
        var availableCars = await _carAPIs.GetAvailableCarsAsync(rentingDate);
        return View("CarList", availableCars);
    }

    // Get: user/Profile
    public async Task<IActionResult> Profile()
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        var id = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;

        if (id == null)
        {
            return NotFound();
        }

        var customer = await _customerAPIs.GetCustomerAsync(int.Parse(id));
        if (customer == null)
        {
            return NotFound();
        }

        return View("CustomerDetails", customer);
    }
    // Get: User/Edit
    [ActionName("Edit")]
    public async Task<IActionResult> EditProfile()
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        var userId = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
        if (userId == null)
        {
            return NotFound();
        }

        var customer = await _customerAPIs.GetCustomerAsync(int.Parse(userId));
        if (customer == null)
        {
            return NotFound();
        }
        return View("EditCustomerDetails", customer);
    }

    // POST: User/EditProfile/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Edit")]
    public async Task<IActionResult> EditProfile(CustomerCreateDTO customer)
    {
        var id = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
        if ((await _customerAPIs.GetCustomerAsync(int.Parse(id))) == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _customerAPIs.UpdateCustomerAsync(int.Parse(id), customer);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
        ViewData["Message"] = Message;
        return View("EditCustomerDetails", _mapper.Map<Customer>(customer));
    }

    // Get: User/TransactionHistory
    public async Task<IActionResult> TransactionHistory()
    {

        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        var userId = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
        return View("TransactionList", await _transactionAPIs.GetRentingTransactionByCustomerAsync(int.Parse(userId)));
    }
    // Get: User/TransactionDetails?transactionId=5    
    public async Task<IActionResult> TransactionDetails(int? transactionId)
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;        
        if (transactionId == null)
        {
            return NotFound();
        }

        var rentingDetail = await _detailAPIs.GetRentingDetailsByTransactionAsync(transactionId.Value);
        //if (rentingDetail == null || rentingDetail.Count == 0)
        //{
        //    return NotFound();
        //}

        return View("TransactionDetails", rentingDetail);
    }
}
