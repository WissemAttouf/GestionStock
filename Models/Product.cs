using System;
using System.ComponentModel.DataAnnotations;

namespace FirstProject.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [Display(Name = "Nom du produit")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }  // ← Ajoute ? pour accepter null

        [Required(ErrorMessage = "Le prix est obligatoire")]
        [Display(Name = "Prix")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La quantité est obligatoire")]
        [Display(Name = "Quantité")]
        public int Quantity { get; set; }

        [Display(Name = "Catégorie")]
        public string? Category { get; set; }  // ← Ajoute ? pour accepter null

        [Display(Name = "Date d'ajout")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}