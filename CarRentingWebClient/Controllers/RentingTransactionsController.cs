using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CarRentingWebClient.AccessAPIs.Interfaces;
using BusinessObjects.DTOs;
using AutoMapper;

namespace CarRentingWebClient.Controllers;

[Authorize(Roles = "Admin")]
[Route("Admin/{controller}/{action=Index}/{id?}")]
public class RentingTransactionsController : Controller
{
    private readonly IRentingTransactionAPIs _transactionAPIs;
    private readonly IRentingDetailAPIs _detailAPIs;
    private readonly ICustomerAPIs _customerAPIs;
    private readonly IMapper _mapper;
    private readonly ISession session;

    public RentingTransactionsController(IRentingTransactionAPIs transactionAPIs, 
                                                IRentingDetailAPIs detailAPIs,
                                                ICustomerAPIs customerAPIs,
                                                IMapper mapper,
                                                IHttpContextAccessor httpContext)
    {
        _transactionAPIs = transactionAPIs;
        _detailAPIs = detailAPIs;
        _customerAPIs = customerAPIs;
        _mapper = mapper;
        session = httpContext.HttpContext!.Session;
    }

    [TempData]
    public string? Message { get; set; }
    [TempData]
    public DateTime? StartDate { get; set; }
    [TempData]
    public DateTime? EndDate { get; set; }

    // GET: Admin/RentingTransactions
    [ActionName("TransactionHistory")]
    public async Task<IActionResult> Index()
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        ViewData["userid"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
        ViewData["token"] = session.GetString("token");
        return View("TransactionList", await _transactionAPIs.GetRentingTransactionsAsync());
        //return View("TransactionList");
    }

    // GET: Admin/RentingTransactions/TransactionDetails?transactionId=5
    [ActionName("TransactionDetails")]
    public async Task<IActionResult> Details(int? transactionId)
    {

        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        ViewData["userid"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
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

    // GET: Admin/RentingTransactions/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var rentingTransaction = await _transactionAPIs.GetRentingTransactionAsync(id.Value);
        if (rentingTransaction == null)
        {
            return NotFound();
        }
        var customers = await _customerAPIs.GetCustomersAsync();
        ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "CustomerName", rentingTransaction.CustomerId);
        ViewData["Message"] = Message;
        return View(rentingTransaction);
    }

    // POST: Admin/RentingTransactions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RentingCreateDTO rentingTransaction)
    {
        if ((await _transactionAPIs.GetRentingTransactionAsync(id)) == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _transactionAPIs.UpdateRentingTransactionAsync(id, rentingTransaction);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return RedirectToAction("TransactionHistory");
        }
        var customers = await _customerAPIs.GetCustomersAsync();
        ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "CustomerName", rentingTransaction.CustomerId);
        ViewData["Message"] = Message;
        return View(_mapper.Map<RentingTransaction>(rentingTransaction));
    }

    // GET: Admin/RentingTransactions/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var rentingTransaction = await _transactionAPIs.GetRentingTransactionAsync(id.Value);
        if (rentingTransaction == null)
        {
            return NotFound();
        }

        return View(rentingTransaction);
    }

    // POST: Admin/RentingTransactions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var rentingTransaction = await _transactionAPIs.GetRentingTransactionAsync(id);
        if (rentingTransaction != null)
        {
            try
            {
                await _transactionAPIs.DeleteRentingTransactionAsync(id);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return RedirectToAction(nameof(Delete), rentingTransaction);
            }
        }
        return RedirectToAction("TransactionHistory");
    }

    [HttpGet]
    public async Task<IActionResult> Search(DateTime startDate, DateTime endDate)
    {
        ViewData["username"] = HttpContext.User.Claims
            .Where(x => x.Type == ClaimTypes.Name)
            .FirstOrDefault()!.Value;

        var carRentalList = new List<RentingTransaction>();
        if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
        {
            carRentalList = (await _transactionAPIs.GetRentingTransactionsAsync())
                    .Where(x => x.RentingDate >= startDate && x.RentingDate <= endDate)
                    .OrderByDescending(x => x.RentingDate)
                    .ToList(); ;
            StartDate = startDate;
            EndDate = endDate;
            ViewData["startDate"] = StartDate;
            ViewData["endDate"] = EndDate;
        }

        return View("TransactionList", carRentalList);
    }
}
