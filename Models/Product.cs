using System.ComponentModel.DataAnnotations;

namespace FirstProject.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du produit est obligatoire.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères.")]
        [Display(Name = "Nom du produit")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Range(typeof(decimal), "0.01", "999999999.99", ErrorMessage = "Le prix doit être supérieur à 0 et ne pas dépasser 999 999 999,99.")]
        [Display(Name = "Prix")]
        public decimal Price { get; set; }

        [Range(0, 1000000000, ErrorMessage = "La quantité doit être un nombre entier positif ou nul.")]
        [Display(Name = "Quantité")]
        public int Quantity { get; set; }

        [StringLength(80, ErrorMessage = "La catégorie ne peut pas dépasser 80 caractères.")]
        [Display(Name = "Catégorie")]
        public string? Category { get; set; }

        [Display(Name = "Date d'ajout")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}