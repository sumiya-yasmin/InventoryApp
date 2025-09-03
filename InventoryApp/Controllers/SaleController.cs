using InventoryApp.Models;
using InventoryApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryApp.Controllers;

public class SaleController : Controller
{
    private readonly ISaleService _saleService;
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;

    public SaleController(ISaleService saleService, IProductService productService, ICustomerService customerService)
    {
        _saleService = saleService;
        _productService = productService;
        _customerService = customerService;
    }

    public async Task<IActionResult> Index()
    {
        var sales = await _saleService.GetAllSaleAsync();
        return View(sales);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var sale = await _saleService.GetSaleByIdAsync(id.Value);
        if (sale == null) return NotFound();

        return View(sale);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["ProductId"] = new SelectList(await _productService.GetAllProductAsync(), "ProductId", "Name");
        ViewData["CustomerId"] = new SelectList(await _customerService.GetAllCustomerAsync(), "CustomerId", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Sale sale)
    {
        if (ModelState.IsValid)
        {
            var product = await _productService.GetProductByIdAsync(sale.ProductId);
            if (product == null) return NotFound();

            if (product.QuantityInStock < sale.Quantity)
            {
                ModelState.AddModelError("", "Not enough stock available!");
            }
            else
            {
                product.QuantityInStock -= sale.Quantity;
                await _productService.UpdateProductAsync(product);

                await _saleService.AddSaleAsync(sale);
                TempData["SuccessMessage"] = "Sale recorded successfully!";
                return RedirectToAction(nameof(Index));
            }
        }

        ViewData["ProductId"] = new SelectList(await _productService.GetAllProductAsync(), "ProductId", "Name", sale.ProductId);
        ViewData["CustomerId"] = new SelectList(await _customerService.GetAllCustomerAsync(), "CustomerId", "Name", sale.CustomerId);
        return View(sale);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var sale = await _saleService.GetSaleByIdAsync(id.Value);
        if (sale == null) return NotFound();

        ViewData["ProductId"] = new SelectList(await _productService.GetAllProductAsync(), "ProductId", "Name", sale.ProductId);
        ViewData["CustomerId"] = new SelectList(await _customerService.GetAllCustomerAsync(), "CustomerId", "Name", sale.CustomerId);
        return View(sale);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Sale sale)
    {
        if (id != sale.SaleId) return NotFound();

        var oldSale = await _saleService.GetSaleByIdAsync(id);
        if (oldSale == null) return NotFound();

        var product = await _productService.GetProductByIdAsync(oldSale.ProductId);
        if (product == null) return NotFound();


        if (ModelState.IsValid)
        {
            int quantityDiff = sale.Quantity - oldSale.Quantity;
            product.QuantityInStock -= quantityDiff;

            if (product.QuantityInStock < 0)
            {
                ModelState.AddModelError("", "Not enough stock to update this sale.");
                return View(sale);
            }

            product.SellPrice = sale.PricePerUnit;

            await _productService.UpdateProductAsync(product);


            oldSale.Quantity = sale.Quantity;
            oldSale.PricePerUnit = sale.PricePerUnit;
            oldSale.ProductId = sale.ProductId;
            oldSale.CustomerId = sale.CustomerId;
            oldSale.SaleDate = sale.SaleDate;

            await _saleService.UpdateSaleAsync(oldSale);
            TempData["SuccessMessage"] = "Sale updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        ViewData["ProductId"] = new SelectList(await _productService.GetAllProductAsync(), "ProductId", "Name", sale.ProductId);
        ViewData["CustomerId"] = new SelectList(await _customerService.GetAllCustomerAsync(), "CustomerId", "Name", sale.CustomerId);
        return View(sale);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var sale = await _saleService.GetSaleByIdAsync(id.Value);
        if (sale == null) return NotFound();

        return View(sale);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
          var sale = await _saleService.GetSaleByIdAsync(id);
        if (sale == null) return NotFound();

        var product = await _productService.GetProductByIdAsync(sale.ProductId);
        if (product != null)
        {

            product.QuantityInStock += sale.Quantity;

            // if (product.QuantityInStock < 0)
            //     product.QuantityInStock = 0;

            await _productService.UpdateProductAsync(product);
        }
        await _saleService.DeleteSaleAsync(id);
        TempData["SuccessMessage"] = "Sale deleted successfully!";
        return RedirectToAction(nameof(Index));
    }
}
