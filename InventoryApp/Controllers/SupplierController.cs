using InventoryApp.Models;
using InventoryApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers;

public class SupplierController : Controller
{
    private readonly ISupplierService _supplierService;
    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    public async Task<IActionResult> Index()
    {
        var suppliers = await _supplierService.GetAllSupplierAsync();
        return View(suppliers);
    }

    private async Task<IActionResult> GetSupplierById(int? id)
    {
           if (id == null)
        {
            return NotFound();
        }
        var supplier = await _supplierService.GetSupplierByIdAsync(id.Value);
        if (supplier == null)
        {
            return NotFound();
        }
        return View(supplier);
    }

    public async Task<IActionResult> Details(int? id)
    {

        if (id == null)
        {
            return NotFound();
        }
        var supplier = await _supplierService.GetSupplierByIdAsync(id.Value);
        if (supplier == null)
        {
            return NotFound();
        }
        return View(supplier);
    }

    public IActionResult Create()
    {
        return View();

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name", "Address", "Phone", "Email")] Supplier supplier)
    {
        if (ModelState.IsValid)
        {
            await _supplierService.AddSupplierAsync(supplier);
            TempData["SuccessMessage"] = $"Supplier '{supplier.Name}' created successfully.";
            return RedirectToAction(nameof(Index));
        }
          return View(supplier);
    }
}