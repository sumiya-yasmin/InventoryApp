using InventoryApp.Models;
using InventoryApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers;

public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;
    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<IActionResult> Index()
    {
        var customers = await _customerService.GetAllCustomerAsync();
        return View(customers);
    }

    private async Task<Customer?> GetCustomerById(int? id)
    {
        if (id == null)
        {
            return null;
        }
        return await _customerService.GetCustomerByIdAsync(id.Value);

    }

    public async Task<IActionResult> Details(int? id)
    {

        var customer = await GetCustomerById(id);
        if (customer == null)
        {
            return NotFound();
        }
        return View(customer);
    }

    public IActionResult Create()
    {
        return View();

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name, Address, Phone, Email")] Customer customer)
    {
        if (ModelState.IsValid)
        {
            await _customerService.AddCustomerAsync(customer);
            TempData["SuccessMessage"] = $"Customer '{customer.CustomerId}' created successfully.";
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        var customer = await GetCustomerById(id);
        if (customer == null)
        {
            return NotFound();
        }
        return View(customer);

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CustomerId, Name, Address, Phone, Email")] Customer customer)
    {
         if (id != customer.CustomerId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _customerService.UpdateCustomerAsync(customer);
            TempData["SuccessMessage"] = $"Customer '{customer.CustomerId}' updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        return View(customer);



    }

    public async Task<IActionResult> Delete(int? id)
    {
        var customer = await GetCustomerById(id);
        if (customer == null)
        {
            return NotFound();
        }
        return View(customer);

    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var customer = await _customerService.DeleteCustomerAsync(id);
          if (customer != null)
        {
            TempData["SuccessMessage"] = $"Customer '{customer.Name}' deleted successfully.";
        }
        return RedirectToAction(nameof(Index));
        
    }
}