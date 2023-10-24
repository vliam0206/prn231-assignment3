using CarRentingWebClient.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using CarRentingWebClient.AccessAPIs.Interfaces;
using BusinessObjects.DTOs;

namespace CarRentingWebClient.Controllers;

public class HomeController : Controller
{
    private IAuthAPIs _authAPIs;
    private ICustomerAPIs _customerAPIs;
    private IMapper _mapper;
    private readonly ISession session;
    public HomeController(IAuthAPIs authAPIs, ICustomerAPIs customerAPIs, 
                            IMapper mapper, IHttpContextAccessor httpContext)
    {
        _authAPIs = authAPIs;
        _customerAPIs = customerAPIs;
        _mapper = mapper;
        session = httpContext.HttpContext!.Session;
    }

    [TempData]
    public string? Message { get; set; }

    public IActionResult Index()
    {
        ClaimsPrincipal claimUser = HttpContext.User;
        if (!claimUser.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login");
        }
        else
        {
            var role = claimUser.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault();
            if (role!.Value.Equals(AppConstants.ADMIN_ROLE))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    public IActionResult Login()
    {
        ClaimsPrincipal claimUser = HttpContext.User;
        if (claimUser.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (ModelState.IsValid)
        {
            var loginDto = _mapper.Map<LoginDTO>(loginModel);
            if (await _authAPIs.AdminLoginAsync(loginDto))
            {
                await WriteClaimsToCookiesAsync(-1, loginDto.Email, "Admin", 
                                 AppConstants.ADMIN_ROLE, loginModel.IsRemember);

                var loginResponse = await _authAPIs.LoginAsync(loginDto);
                session.SetString("token", loginResponse.Token);

                return RedirectToAction("Index", "Admin");
            }
            else if (await _authAPIs.CustomerLoginAsync(loginDto))
            {
                try
                {
                    var loginResponse = await _authAPIs.LoginAsync(loginDto);
                    if (loginResponse.Status == 1)
                    {
                        await WriteClaimsToCookiesAsync(loginResponse.AccountId, loginResponse.Email,
                                    loginResponse.Name, AppConstants.CUSTOMER_ROLE, loginModel.IsRemember);

                        session.SetString("token", loginResponse.Token);

                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        Message = "Your account has been inactive.";
                    }
                } catch (Exception ex)
                {
                    Message = ex.Message;
                }
            }
            else
            {
                Message = "Wrong username / password!";
            }
        }
        else
        {
            Message = "Login failed!";
        }
        ViewData["Message"] = Message;
        return View(loginModel);
    }   

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        session.Remove("token");
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    public IActionResult Register()
    {
        ClaimsPrincipal claimUser = HttpContext.User;
        if (claimUser.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel registerModel)
    {
        if (ModelState.IsValid)
        {
            var account = await _customerAPIs.GetCustomerByEmailAsync(registerModel.Email);
            if (account != null)
            {
                Message = "Register failed! Email is duplicated.";
                ViewData["ErrorMessage"] = Message;
            }
            else
            {
                var newCustomer = _mapper.Map<CustomerCreateDTO>(registerModel);
                await _customerAPIs.CreateCustomerAsync(newCustomer);
                Message = "Register successfully! Please login.";
                ViewData["Message"] = Message;
            }
        }
        return View(registerModel);
    }

    private async Task WriteClaimsToCookiesAsync(int id, string email, string name, string role, bool isRemeber)
    {
        var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role),
                };

        //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
        var principal = new ClaimsPrincipal(identity);

        //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
        {
            IsPersistent = false
        });
    }
}