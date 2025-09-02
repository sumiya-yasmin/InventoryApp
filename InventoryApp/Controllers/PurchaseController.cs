using InventoryApp.Models;
using InventoryApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryApp.Controllers;

public class PurchaseController : Controller
{
    private readonly IPurchaseService _purchaseService;
    private readonly IProductService _productService;
    private readonly ISupplierService _supplierService;

    public PurchaseController(IPurchaseService purchaseService, IProductService productService, ISupplierService supplierService)
    {
        _purchaseService = purchaseService;
        _productService = productService;
        _supplierService = supplierService;
    }

    public async Task<IActionResult> Index()
    {
        var purchases = await _purchaseService.GetAllPurchaseAsync();
        return View(purchases);
    }

    private async Task<Purchase?> GetPurchaseByID(int? id)
    {
        if (id == null)
        {
            return null;
        }
        return await _purchaseService.GetPurchaseByIdAsync(id.Value);
    }
    public async Task<IActionResult> Details(int? id)
    {
        var purchase = await GetPurchaseByID(id);
        if (purchase != null)
        {
            return NotFound();
        }
        return View(purchase);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create()
    {
        ViewData["ProductId"] = new SelectList(await _productService.GetAllProductAsync(), "ProductId", "Name");
        ViewData["SupplierId"] = new SelectList(await _supplierService.GetAllSupplierAsync(), "SupplierId", "Name");
        return View();
    }

    public async Task<IActionResult> Create(Purchase purchase)
    {
        var product = await _productService.GetProductByIdAsync(purchase.ProductId);
        if (product == null) return NotFound();
        if (ModelState.IsValid)
        {

            product.QuantityInStock += purchase.Quantity;
            product.PurchasePrice = purchase.PricePerUnit;
            await _productService.UpdateProductAsync(product);

            await _purchaseService.AddPurchaseAsync(purchase);
            TempData["SuccessMessage"] = "Purchase recorded successfully!";
            return RedirectToAction(nameof(Index));
        }
        ViewData["ProductId"] = new SelectList(await _productService.GetAllProductAsync(), "ProductId", "Name", purchase.ProductId);
        ViewData["SupplierId"] = new SelectList(await _supplierService.GetAllSupplierAsync(), "SupplierId", "Name", purchase.SupplierId);
        return View(purchase);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        var purchase = await GetPurchaseByID(id);
        if (purchase != null)
        {
            return NotFound();
        }
        ViewData["ProductId"] = new SelectList(await _productService.GetAllProductAsync(), "ProductId", "Name", purchase.ProductId);
        ViewData["SupplierId"] = new SelectList(await _supplierService.GetAllSupplierAsync(), "SupplierId", "Name", purchase.SupplierId);
        return View(purchase);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Purchase purchase)
    {
        if (id != purchase.PurchaseId) return NotFound();

        if (ModelState.IsValid)
        {
            await _purchaseService.UpdatePurchaseAsync(purchase);
            TempData["SuccessMessage"] = "Purchase updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        ViewData["ProductId"] = new SelectList(await _productService.GetAllProductAsync(), "ProductId", "Name", purchase.ProductId);
        ViewData["SupplierId"] = new SelectList(await _supplierService.GetAllSupplierAsync(), "SupplierId", "Name", purchase.SupplierId);
        return View(purchase);
    }
    public async Task<IActionResult> Delete(int id)
    {
        var purchase = await GetPurchaseByID(id);
        if (purchase != null)
        {
            return NotFound();
        }

        return View(purchase);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _purchaseService.DeletePurchaseAsync(id);
        TempData["SuccessMessage"] = "Purchase deleted successfully!";
        return RedirectToAction(nameof(Index));
    }
}