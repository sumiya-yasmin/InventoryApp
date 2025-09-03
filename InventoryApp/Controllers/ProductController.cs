using System.Runtime.InteropServices;
using InventoryApp.Models;
using InventoryApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryApp.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ISupplierService _supplierService;

    public ProductController(IProductService productService, ICategoryService categoryService, ISupplierService supplierService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _supplierService = supplierService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProductAsync();
        return View(products);
    }

    private async Task<Product?> GetProductById(int? id)
    {
        if (id == null)
        {
            return null;
        }
        return await _productService.GetProductByIdAsync(id.Value);
    }

    public async Task<IActionResult> Details(int? id)
    {

        var product = await GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);

    }

    public async Task<IActionResult> Create()
    {
        ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategoriesAsync(), "CategoryId", "Name");
        return View();

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name, PurchasePrice, SellPrice, CategoryId")] Product product)
    {
        if (ModelState.IsValid)
        {
            product.QuantityInStock = 0;
            await _productService.AddProductAsync(product);
            TempData["SuccessMessage"] = $"Product '{product.ProductId}' created successfully";
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategoriesAsync(), "CategoryId", "Name");
        return View(product);

    }


    public async Task<IActionResult> Edit(int? id)
    {
        var product = await GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategoriesAsync(), "CategoryId", "Name");
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProductId, Name, PurchasePrice, SellPrice, CategoryId, QuantityInStock")] Product product)
    {
        if (id != product.ProductId)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            await _productService.UpdateProductAsync(product);
            TempData["SuccessMessage"] = $"Product '{product.ProductId}' updated successfully";
            return RedirectToAction(nameof(Index));

        }
         ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllCategoriesAsync(), "CategoryId", "Name");
         return View(product);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        var product = await GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);

    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _productService.DeleteProductAsync(id);
        if (product != null)
        {
            TempData["SuccessMessage"] = $"Product '{product.ProductId}' deleted successfully";
        }
        return RedirectToAction(nameof(Index));
    }
}