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

    private async Task<Supplier?> GetSupplierById(int? id)
    {
        if (id == null)
        {
            return null;
        }
        return await _supplierService.GetSupplierByIdAsync(id.Value);

    }

    public async Task<IActionResult> Details(int? id)
    {

        var supplier = await GetSupplierById(id);
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
    public async Task<IActionResult> Create([Bind("Name, Address, Phone, Email")] Supplier supplier)
    {
        if (ModelState.IsValid)
        {
            await _supplierService.AddSupplierAsync(supplier);
            TempData["SuccessMessage"] = $"Supplier '{supplier.SupplierId}' created successfully.";
            return RedirectToAction(nameof(Index));
        }
        return View(supplier);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        var supplier = await GetSupplierById(id);
        if (supplier == null)
        {
            return NotFound();
        }
        return View(supplier);

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("SupplierId, Name, Address, Phone, Email")] Supplier supplier)
    {
         if (id != supplier.SupplierId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _supplierService.UpdateSupplierAsync(supplier);
            TempData["SuccessMessage"] = $"Supplier '{supplier.SupplierId}' updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        return View(supplier);



    }

    public async Task<IActionResult> Delete(int? id)
    {
        var supplier = await GetSupplierById(id);
        if (supplier == null)
        {
            return NotFound();
        }
        return View(supplier);

    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var supplier = await _supplierService.DeleteSupplierAsync(id);
          if (supplier != null)
        {
            TempData["SuccessMessage"] = $"Supplier '{supplier.Name}' deleted successfully.";
        }
        return RedirectToAction(nameof(Index));
        
    }
}