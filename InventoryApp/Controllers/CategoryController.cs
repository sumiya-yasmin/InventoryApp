using InventoryApp.Models;
using InventoryApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return View(categories);
    }
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var category = await _categoryService.GetCategoryByIdAsync(id.Value);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name, Description")] Category category)
    {
        if (ModelState.IsValid)
        {
            await _categoryService.AddCategoryAsync(category);
            TempData["SuccessMessage"] = $"Category '{category.Name}' created successfully.";
            return RedirectToAction(nameof(Index));
        }
        return View(category);


    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();

        }
        var category = await _categoryService.GetCategoryByIdAsync(id.Value);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name,Description")] Category category)
    {
        if (id != category.CategoryId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _categoryService.UpdateCategoryAsync(category);
                TempData["SuccessMessage"] = $"Category '{category.Name}' updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _categoryService.GetCategoryByIdAsync(category.CategoryId) == null)
                {
                    return NotFound();
                }
                else
                {

                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _categoryService.GetCategoryByIdAsync(id.Value);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
       var category = await _categoryService.DeleteCategoryAsync(id);
        if (category != null)
        {
            TempData["SuccessMessage"] = $"Category '{category.Name}' deleted successfully.";
        }
        return RedirectToAction(nameof(Index));
    }

}