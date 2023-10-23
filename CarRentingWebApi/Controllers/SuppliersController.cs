﻿using Microsoft.AspNetCore.Mvc;
using BusinessObjects;
using Repositories.Interfaces;

namespace CarRentingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuppliersController : ControllerBase
{
    private readonly ICarRepository _carRepository;

    public SuppliersController(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    // GET: api/Suppliers
    [HttpGet]
    public async Task<ActionResult<List<Supplier>>> GetSuppliers()
    {
        return await _carRepository.GetSuppliersAsync();
    }
}
