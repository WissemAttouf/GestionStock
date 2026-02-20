using Microsoft.EntityFrameworkCore;
using FirstProject.dta;
using FirstProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Utilise SQLite (pas besoin de serveur, ça crée un fichier .db)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=FirstProject.db"));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

// Crée la base de données et ajoute des données de test
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Crée la base de données si elle n'existe pas
    context.Database.EnsureCreated();
    
    // Ajoute des produits de test si la table est vide
    if (!context.Products.Any())
    {
        context.Products.AddRange(
            new Product { 
                Name = "Ordinateur Portable HP", 
                Description = "15 pouces, 16GB RAM, 512GB SSD",
                Price = 899.99m, 
                Quantity = 10, 
                Category = "Informatique", 
                CreatedAt = DateTime.Now 
            },
            new Product { 
                Name = "Souris Sans Fil Logitech", 
                Description = "Souris ergonomique Bluetooth",
                Price = 29.99m, 
                Quantity = 50, 
                Category = "Accessoires", 
                CreatedAt = DateTime.Now 
            },
            new Product { 
                Name = "Clavier Mécanique", 
                Description = "Clavier RGB avec switches Cherry MX",
                Price = 79.99m, 
                Quantity = 25, 
                Category = "Accessoires", 
                CreatedAt = DateTime.Now 
            },
            new Product { 
                Name = "Écran 24 pouces", 
                Description = "Écran Full HD IPS",
                Price = 159.99m, 
                Quantity = 15, 
                Category = "Informatique", 
                CreatedAt = DateTime.Now 
            }
        );
        context.SaveChanges();
        Console.WriteLine("✅ Produits de test ajoutés avec succès !");
    }
}

app.Run();