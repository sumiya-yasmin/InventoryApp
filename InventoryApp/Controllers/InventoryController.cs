using InventoryApp.Services.Interfaces;
using InventoryApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class InventoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly ISupplierService _supplierService;
        private readonly ISaleService _saleService;
        private readonly IPurchaseService _purchaseService;

        public InventoryController(
            ICategoryService categoryService,
            IProductService productService,
           ICustomerService customerService,
            ISupplierService supplierService,
            ISaleService saleService,
            IPurchaseService purchaseService
        )
        {
            _categoryService = categoryService;
            _productService = productService;
            _customerService = customerService;
            _supplierService = supplierService;
            _saleService = saleService;
            _purchaseService = purchaseService;
        }


        public async Task<IActionResult> Dashboard()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var products = await _productService.GetAllProductAsync();
            var customers = await _customerService.GetAllCustomerAsync();
            var suppliers = await _supplierService.GetAllSupplierAsync();
            var sales = await _saleService.GetAllSaleAsync();
            var purchases = await _purchaseService.GetAllPurchaseAsync();
            var model = new DashboardViewModel
            {
                Categories = categories,
                Products = products,
                Customers = customers,
                Suppliers = suppliers,
                Sales = sales,
                Purchases = purchases

            };
            return View(model);
        }
        

        // public ActionResult Dashboard()
        // {
        //     return View();
        // }

    }
}
