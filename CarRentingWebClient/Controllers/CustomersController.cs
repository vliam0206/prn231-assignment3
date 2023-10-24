using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CarRentingWebClient.AccessAPIs.Interfaces;
using BusinessObjects.DTOs;
using AutoMapper;
using CarRentingWebClient.Models;
using BusinessObjects;
using Microsoft.IdentityModel.Tokens;

namespace CarRentingWebClient.Controllers;

[Authorize(Roles = "Admin")]
[Route("Admin/{controller}/{action=Index}/{id?}")]
public class CustomersController : Controller
{
    private readonly ICustomerAPIs _customerAPIs;
    private readonly IMapper _mapper;
    private readonly ISession session;
    public CustomersController(ICustomerAPIs customerAPIs, IMapper mapper,
        IHttpContextAccessor httpContext)
    {
        _customerAPIs = customerAPIs;
        _mapper = mapper;
        session = httpContext.HttpContext!.Session;
    }

    [TempData]
    public string? Message { get; set; }

    // GET: Admin/Customers
    public IActionResult Index()
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        ViewData["token"] = session.GetString("token");
        //return View(await _customerAPIs.GetCustomersAsync());
        return View();
    }

    // GET: Admin/Customers/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        if (id == null)
        {
            return NotFound();
        }

        var customer = await _customerAPIs.GetCustomerAsync(id.Value);
        if (customer == null)
        {
            return NotFound();
        }

        return View("CustomerDetails", customer);
    }

    // GET: Admin/Customers/Create    
    public IActionResult Create()
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        ViewData["Message"] = Message;
        return View();
    }

    // POST: Admin/Customers/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerCreateDTO customer)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _customerAPIs.CreateCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            } catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
        ViewData["Message"] = Message;
        return View(_mapper.Map<RegisterModel>(customer));
    }

    // GET: Admin/Customers/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        if (id == null)
        {
            return NotFound();
        }

        var customer = await _customerAPIs.GetCustomerAsync(id.Value);
        if (customer == null)
        {
            return NotFound();
        }
        ViewData["Message"] = Message;
        return View("EditCustomerDetails", customer);
    }

    // POST: Admin/Customers/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CustomerCreateDTO customer)
    {
        if ((await _customerAPIs.GetCustomerAsync(id)) == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _customerAPIs.UpdateCustomerAsync(id, customer);
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

    // GET: Admin/Customers/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {        
        if (id == null)
        {
            return NotFound();
        }

        var customer = await _customerAPIs.GetCustomerAsync(id.Value);
        if (customer == null)
        {
            return NotFound();
        }

        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        ViewData["Message"] = Message;
        return View(customer);
    }

    // POST: Admin/Customers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var customer = await _customerAPIs.GetCustomerAsync(id);
        if (customer != null)
        {
            try
            {
                await _customerAPIs.DeleteCustomerAsync(id);
            } catch (Exception ex)
            {
                Message = ex.Message;
                return RedirectToAction(nameof(Delete), customer);
            }
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? searchValue)
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        var cusList = new List<Customer>();
        if (!searchValue.IsNullOrEmpty())
        {
            cusList = await _customerAPIs.SearchCustomersAsync(searchValue);
        }
        return View("Index", cusList);
    }
}
