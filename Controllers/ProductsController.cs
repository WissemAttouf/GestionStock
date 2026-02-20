using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstProject.dta;
using FirstProject.Models;
using System.Linq;
using System.Threading.Tasks;
using System;

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
        public async Task<IActionResult> Index(string searchString)
        {
            try
            {
                ViewBag.CurrentFilter = searchString;
                
                // Récupérer tous les produits triés par ID
                var products = await _context.Products
                    .OrderByDescending(p => p.Id)  // Les plus récents d'abord
                    .ToListAsync();
                
                // Filtre de recherche
                if (!string.IsNullOrEmpty(searchString))
                {
                    products = products.Where(p => 
                        (!string.IsNullOrEmpty(p.Name) && p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(p.Description) && p.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(p.Category) && p.Category.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }
                
                return View(products);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erreur de chargement : " + ex.Message;
                return View(new List<Product>());
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                // Supprimer les erreurs de validation pour CreatedAt
                if (ModelState.ContainsKey("CreatedAt"))
                {
                    ModelState["CreatedAt"].Errors.Clear();
                    ModelState["CreatedAt"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                }

                if (ModelState.IsValid)
                {
                    product.CreatedAt = DateTime.Now;
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = $"✅ Produit '{product.Name}' créé avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                
                // Afficher les erreurs de validation
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state.Errors.Any())
                    {
                        ModelState.AddModelError(key, state.Errors.First().ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "❌ Erreur lors de la création : " + ex.Message);
            }
            
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"✅ Produit '{product.Name}' modifié avec succès !";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "❌ Erreur lors de la modification : " + ex.Message);
            }
            
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    string productName = product.Name;
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"✅ Produit '{productName}' supprimé avec succès !";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "❌ Erreur lors de la suppression : " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}