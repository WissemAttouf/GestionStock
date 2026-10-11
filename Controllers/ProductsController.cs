using FirstProject.dta;
using FirstProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string? searchString)
        {
            ViewBag.CurrentFilter = searchString;

            IQueryable<Product> query = _context.Products
                .AsNoTracking()
                .OrderByDescending(p => p.Id);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var term = searchString.Trim();
                query = query.Where(p =>
                    EF.Functions.Like(p.Name, $"%{term}%") ||
                    (p.Description != null && EF.Functions.Like(p.Description, $"%{term}%")) ||
                    (p.Category != null && EF.Functions.Like(p.Category, $"%{term}%")));
            }

            return View(await query.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id <= 0)
            {
                return NotFound();
            }

            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return product is null ? NotFound() : View(product);
        }

        // GET: Products/Create
        public IActionResult Create() => View();

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Description,Price,Quantity,Category")] Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            product.Name = product.Name.Trim();
            product.Description = string.IsNullOrWhiteSpace(product.Description)
                ? null
                : product.Description.Trim();
            product.Category = string.IsNullOrWhiteSpace(product.Category)
                ? null
                : product.Category.Trim();
            product.CreatedAt = DateTime.Now;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Produit « {product.Name} » créé avec succès.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null || id <= 0)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            return product is null ? NotFound() : View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Description,Price,Quantity,Category")] Product input)
        {
            if (id != input.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            // Update only editable fields; preserve Id and CreatedAt from the database.
            product.Name = input.Name.Trim();
            product.Description = string.IsNullOrWhiteSpace(input.Description)
                ? null
                : input.Description.Trim();
            product.Price = input.Price;
            product.Quantity = input.Quantity;
            product.Category = string.IsNullOrWhiteSpace(input.Category)
                ? null
                : input.Category.Trim();

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Produit « {product.Name} » modifié avec succès.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0)
            {
                return NotFound();
            }

            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return product is null ? NotFound() : View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            var productName = product.Name;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Produit « {productName} » supprimé avec succès.";
            return RedirectToAction(nameof(Index));
        }
    }
}