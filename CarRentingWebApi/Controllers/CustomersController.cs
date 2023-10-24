﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Repositories.Interfaces;
using AutoMapper;
using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarRentingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomersController(ICustomerRepository customerRepository, IMapper mapper, 
        IHttpContextAccessor httpContextAccessor)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET: api/Customers
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<Customer>>> GetCustomers()
    {
        return await _customerRepository.GetCustomersAsync();
    }

    // GET: api/Customers/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        var userIdClaim = _httpContextAccessor
                                .HttpContext?
                                .User?
                                .FindFirstValue(ClaimTypes.SerialNumber);
        if (int.Parse(userIdClaim) != -1 && int.Parse(userIdClaim) != id)
        {
            return Forbid("You are not allow to access this function.");
        }
        var customer = await _customerRepository.GetCustomerByIdAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        return customer;
    }

    // GET: api/get-by-email/{email}
    [HttpGet("get-by-email/{email}")]
    [Authorize]
    public async Task<ActionResult<Customer>> GetCustomerByEmail(string email)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.SerialNumber);
        var userEmailClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.Parse(userIdClaim) != -1 && !userEmailClaim.Equals(email))
        {
            return Forbid("You are not allow to access this function.");
        }

        var customer = await _customerRepository.GetCustomerAsync(email);

        if (customer == null)
        {
            return NotFound();
        }

        return customer;
    }
    
    // PUT: api/Customers/5    
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutCustomer(int id, CustomerCreateDTO customerModel)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.SerialNumber);
        if (int.Parse(userIdClaim) != -1 && int.Parse(userIdClaim) != id)
        {
            return Forbid("You are not allow to access this function.");
        }

        var cusTemp = await _customerRepository.GetCustomerByIdAsync(id);
        if (cusTemp == null)
        {
            return NotFound();
        }
        cusTemp = await _customerRepository.GetCustomerAsync(customerModel.Email);        
        if (cusTemp != null && cusTemp.CustomerId != id )
        {
            return BadRequest("Email is duplicated with other customers!");
        }
        try
        {
            var customer = _mapper.Map<Customer>(customerModel);
            customer.CustomerId = id;
            await _customerRepository.UpdateCustomerAsync(customer);
        }
        catch (DbUpdateConcurrencyException)
        {
            if ((await _customerRepository.GetCustomerByIdAsync(id)) == null)
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

    // POST: api/Customers    
    [HttpPost]
    public async Task<ActionResult<Customer>> PostCustomer(CustomerCreateDTO customerModel)
    {
        if ((await _customerRepository.GetCustomerAsync(customerModel.Email)) != null)
        {
            return BadRequest("Email is duplicated!");
        }
        var cusAge = (DateTime.Today - customerModel.CustomerBirthday).TotalDays / 365;
        if (cusAge < 18)
        {
            return BadRequest("Invalid Birthday! The age must be greater than 18.");
        }

        var customer = _mapper.Map<Customer>(customerModel);
        await _customerRepository.SaveCustomerAsync(customer);

        return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound();
        }

        await _customerRepository.DeleteCustomerAsync(customer);

        return NoContent();
    }

    // GET: api/Customers/search/abc
    [HttpGet("search/{keyword}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<Customer>>> SearchCustomers(string keyword)
    {
        return await _customerRepository.SearchCustomersByNameAsync(keyword.Trim());
    }
}
