﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CarRentingWebClient.AccessAPIs.Interfaces;
using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects;
using Microsoft.IdentityModel.Tokens;

namespace CarRentingWebClient.Controllers;

[Authorize(Roles = "Admin")]
[Route("Admin/{controller}/{action=Index}/{id?}")]
public class CarInformationsController : Controller
{
    private readonly ICarInformationAPIs _carAPIs;
    private readonly IManufacturerAPIs _manufacturerAPIs;
    private readonly ISupplierAPIs _supplierAPIs;
    private readonly IMapper _mapper;
    private readonly ISession session;

    public CarInformationsController(ICarInformationAPIs carAPIs,
                                           IManufacturerAPIs manufacturerAPIs,
                                           ISupplierAPIs supplierAPIs,
                                           IMapper mapper,
                                           IHttpContextAccessor httpContext)
    {
        _carAPIs = carAPIs;
        _manufacturerAPIs = manufacturerAPIs;
        _supplierAPIs = supplierAPIs;
        _mapper = mapper;
        session = httpContext.HttpContext!.Session;
    }

    [TempData]
    public string? Message { get; set; }

    // GET: CarInformations
    public async Task<IActionResult> Index()
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        ViewData["token"] = session.GetString("token");
        //var carList = await _carAPIs.GetCarInformationsAsync();       
        //return View(carList);
        return View();
    }

    // GET: CarInformations/Details/5
    public async Task<IActionResult> Details(int? id)
    {

        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        if (id == null)
        {
            return NotFound();
        }

        var carInformation = await _carAPIs.GetCarInformationAsync(id.Value);
        if (carInformation == null)
        {
            return NotFound();
        }

        return View(carInformation);
    }

    // GET: CarInformations/Create
    public async Task<IActionResult> Create()
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        var manufacturers = await _manufacturerAPIs.GetManufacturersAsync();
        var suppliers = await _supplierAPIs.GetSuppliersAsync();
        ViewData["ManufacturerId"] = new SelectList(manufacturers, "ManufacturerId", "ManufacturerName");
        ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "SupplierName");
        ViewData["Message"] = Message;
        return View();
    }

    // POST: CarInformations/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CarCreateDTO carInformation)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _carAPIs.CreateCarInformationAsync(carInformation);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
        var manufacturers = await _manufacturerAPIs.GetManufacturersAsync();
        var suppliers = await _supplierAPIs.GetSuppliersAsync();
        ViewData["ManufacturerId"] = new SelectList(manufacturers, "ManufacturerId", "ManufacturerName", carInformation.ManufacturerId);
        ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "SupplierName", carInformation.SupplierId);
        ViewData["Message"] = Message;
        return View(carInformation);
    }

    // GET: CarInformations/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        if (id == null)
        {
            return NotFound();
        }

        var carInformation = await _carAPIs.GetCarInformationAsync(id.Value);
        if (carInformation == null)
        {
            return NotFound();
        }
        var manufacturers = await _manufacturerAPIs.GetManufacturersAsync();
        var suppliers = await _supplierAPIs.GetSuppliersAsync();
        ViewData["ManufacturerId"] = new SelectList(manufacturers, "ManufacturerId", "ManufacturerName", carInformation.ManufacturerId);
        ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "SupplierName", carInformation.SupplierId);
        ViewData["Message"] = Message;
        return View(carInformation);
    }

    // POST: CarInformations/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CarCreateDTO carInformation)
    {
        if ((await _carAPIs.GetCarInformationAsync(id)) == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _carAPIs.UpdateCarInformationAsync(id, carInformation);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        var manufacturers = await _manufacturerAPIs.GetManufacturersAsync();
        var suppliers = await _supplierAPIs.GetSuppliersAsync();
        ViewData["ManufacturerId"] = new SelectList(manufacturers, "ManufacturerId", "ManufacturerName", carInformation.ManufacturerId);
        ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "SupplierName", carInformation.SupplierId);
        ViewData["Message"] = Message;

        return View(carInformation);
    }

    // GET: CarInformations/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var carInformation = await _carAPIs.GetCarInformationAsync(id.Value);
        if (carInformation == null)
        {
            return NotFound();
        }
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        ViewData["Message"] = Message;
        return View(carInformation);
    }

    // POST: CarInformations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {

        var carInformation = await _carAPIs.GetCarInformationAsync(id);
        if (carInformation != null)
        {
            try
            {
                await _carAPIs.DeleteCarInformationAsync(id);                
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return RedirectToAction(nameof(Delete), carInformation);
            }
        }
        return RedirectToAction(nameof(Index));

    }

    [HttpGet]
    public async Task<IActionResult> Search(string? searchValue)
    {
        ViewData["username"] = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        var carList = new List<CarInformation>();
        if (!searchValue.IsNullOrEmpty())
        {
            carList = await _carAPIs.SearchCarsAsync(searchValue);
        }
        return View("Index", carList);
    }
}
