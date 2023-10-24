using CarRentingWebClient.AccessAPIs.Interfaces;
using CarRentingWebClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRentingWebClient.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ICarInformationAPIs _carAPIs;
    private readonly ISession session;
    public AdminController(ICarInformationAPIs carAPIs, IHttpContextAccessor httpContext)
    {
        _carAPIs = carAPIs;
        session = httpContext.HttpContext!.Session;
    }

    public IActionResult Index()
    {

        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        ViewData["token"] = session.GetString("token");
        return View();
    }

    [TempData]
    public string? Message { get; set; }

    // Get: Admin/Renting
    public IActionResult Renting()
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        ViewData["Message"] = Message;
        return View("Renting");
    }

    // Get: Admin/CarList?startDate=31/1/2023&endDate=1/2/2023
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
}
