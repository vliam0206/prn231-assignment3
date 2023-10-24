using BusinessObjects.DTOs;
using CarRentingWebClient.AccessAPIs.Interfaces;
using CarRentingWebClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;

namespace CarRentingWebClient.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly IRentingTransactionAPIs _transactionAPIs;
    private readonly IRentingDetailAPIs _detailAPIs;
    private readonly ICarInformationAPIs _carAPIs;
    private readonly ICustomerAPIs _customerAPIs;
    private readonly ISession session;

    public CartController(IRentingTransactionAPIs transactionAPIs,
                                IRentingDetailAPIs detailAPIs,
                                ICarInformationAPIs carAPIs,
                                ICustomerAPIs customerAPIs,
                                IHttpContextAccessor httpContext)
    {
        _transactionAPIs = transactionAPIs;
        _detailAPIs = detailAPIs;
        _carAPIs = carAPIs;
        _customerAPIs = customerAPIs;
        session = httpContext.HttpContext!.Session;
    }

    [TempData]
    public string? Message { get; set; }
    [TempData]
    public string? ErrorMessage { get; set; }

    #region session utilize methods
    // Lấy cart từ Session (danh sách CartItem)
    private List<CartItem> GetCartItems()
    {
        var jsoncart = session.GetString(AppConstants.CARTKEY);
        if (jsoncart != null)
        {
            return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
        }
        return new List<CartItem>();
    }

    // Xóa cart khỏi session
    private void ClearCart()
    {
        session.Remove(AppConstants.CARTKEY);
    }

    // Lưu Cart (Danh sách CartItem) vào session
    private void SaveCartSession(List<CartItem> ls)
    {
        string jsoncart = JsonConvert.SerializeObject(ls);
        session.SetString(AppConstants.CARTKEY, jsoncart);
    }
    private void AddTotal(decimal price)
    {
        var totalS = session.GetString(AppConstants.TOTAL_PRICE);
        decimal total = 0;
        if (totalS != null)
        {
            total = decimal.Parse(totalS);            
        }
        total += price;
        session.SetString(AppConstants.TOTAL_PRICE, total.ToString());
    }
    private void SubstractTotal(decimal price)
    {
        var totalS = session.GetString(AppConstants.TOTAL_PRICE);
        decimal total = 0;
        if (totalS != null)
        {
            total = decimal.Parse(totalS);
        }
        total -= price;
        session.SetString(AppConstants.TOTAL_PRICE, total.ToString());
    }
    #endregion

    public IActionResult Index()
    {
        var total = session.GetString(AppConstants.TOTAL_PRICE);
        ViewData["total"] = total != null ? total : "0";
        ViewData["Message"] = Message;
        ViewData["ErrorMessage"] = ErrorMessage;
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        ViewData["userId"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
        return View(GetCartItems());
    }

    // Thêm sản phẩm vào cart
    // Get: Cart/AddToCart?carId=1&startDate=...&endDate=...
    public async Task<IActionResult> AddToCart([FromQuery] int carId, DateTime startDate, DateTime endDate)
    {

        var carInfo = await _carAPIs.GetCarInformationAsync(carId);
        if (carInfo == null)
            return NotFound("Car does not exist.");

        // Xử lý đưa vào Cart
        var cart = GetCartItems();
        var cartitem = cart.Find(x => x.CarInfo.CarId == carId);              
        if (cartitem != null)
        {
            ErrorMessage = $"Failed! You have choose the car {carInfo.CarName} for renting from {cartitem.RentingDateInfo.First().Value.StartDate} " +
                $"to {cartitem.RentingDateInfo.First().Value.EndDate}";
            return RedirectToAction("Index");
        }
        else
        {  //  Thêm mới
            var rentingDate = new RentingDate { StartDate = startDate, EndDate = endDate };
            cart.Add(new CartItem()
            {
                CarInfo = carInfo,
                RentingDateInfo = new SortedList<DateTime, RentingDate>
                {    //key     , value
                    { startDate, rentingDate }
                }
            });
        }

        // Lưu cart vào Session
        SaveCartSession(cart);
        // Update total
        var numOfDates = (endDate - startDate).TotalDays + 1;
        AddTotal(carInfo.CarRentingPricePerDay.Value * decimal.Parse(numOfDates.ToString()));
        // Chuyển đến trang hiện thị Cart
        Message = "Add new renting successfully!";        
        return RedirectToAction("Index");
    }

    // Xóa sản phẩm ra khỏi cart
    public async Task<IActionResult> RemoveFromCart(int carId)
    {
        var carInfo = await _carAPIs.GetCarInformationAsync(carId);
        if (carInfo == null)
            return NotFound("Car does not exist.");
        var cart = GetCartItems();
        var cartitem = cart.Find(x => x.CarInfo.CarId == carId);
        if (cartitem != null)
        {
            cart.Remove(cartitem);
            // Lưu cart vào Session
            SaveCartSession(cart);

            // Update total
            var numOfDates = (cartitem.RentingDateInfo.FirstOrDefault().Value.EndDate - cartitem.RentingDateInfo.FirstOrDefault().Value.StartDate)
                            .TotalDays + 1;
            SubstractTotal(carInfo.CarRentingPricePerDay!.Value * decimal.Parse(numOfDates.ToString()));

            Message = $"Remove car {carInfo.CarName} successfully!";
        }
        // Chuyển đến trang hiện thị Cart        
        return RedirectToAction("Index");
    }

    // Get:User/Cart/Checkout
    [Authorize(Roles = "User")]
    public IActionResult UserCheckoutView()
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;        
        var total = session.GetString(AppConstants.TOTAL_PRICE);        
        ViewData["total"] = total != null ? total : "0"; 
        ViewData["UserId"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
        return View("Checkout", GetCartItems());
    }

    // Get: Admin/Cart/Checkout
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminCheckoutView()
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        var total = session.GetString(AppConstants.TOTAL_PRICE);
        ViewData["total"] = total != null ? total : "0";
        ViewData["UserId"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
        var customers = await _customerAPIs.GetCustomersAsync();
        ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "CustomerName", customers.FirstOrDefault());
        return View("Checkout", GetCartItems());
    }

    // Post: Cart/Checkout
    [HttpPost]
    [ActionName("Checkout")]
    public async Task<IActionResult> CheckoutConfirmed(int userId)
    {
        var cart = GetCartItems();
        if (cart.Count < 1)
        {
            ViewData["Message"] = "Checkout failed! There is no item in cart.";
            return View("Checkout");
        }

        var total = session.GetString(AppConstants.TOTAL_PRICE);
        // Save transaction
        var newTransaction = new RentingCreateDTO 
        { 
            CustomerId = userId,
            TotalPrice = decimal.Parse(total)
        };
        try
        {
            // create new transaction
            var createdTransaction = await _transactionAPIs.CreateRentingTransactionAsync(newTransaction);
            // create transaction details
            var rentingDetails = new List<RentingDetailCreateDTO>();
            foreach (var item in cart)
            {
                var detailDto = new RentingDetailCreateDTO
                {
                    RentingTransactionId = createdTransaction.RentingTransationId,
                    CarId = item.CarInfo.CarId,
                    Price = item.CarInfo.CarRentingPricePerDay,
                    StartDate = item.RentingDateInfo.FirstOrDefault().Value.StartDate,
                    EndDate = item.RentingDateInfo.FirstOrDefault().Value.EndDate
                };
                rentingDetails.Add(detailDto);
            }
            await _detailAPIs.CreateRentingDetailAsync(rentingDetails);
        } catch (Exception ex)
        {
            Message = ex.Message;
            return View("Checkout");
        }

        // Reset cart
        ClearCart();
        Message = "Checkout successfully!";
        ViewData["Message"] = Message;
        return View("Checkout");
    }
}
