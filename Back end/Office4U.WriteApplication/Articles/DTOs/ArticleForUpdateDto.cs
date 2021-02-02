using System.ComponentModel.DataAnnotations;

namespace Office4U.WriteApplication.Articles.DTOs
{
    public class ArticleForUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name1 { get; set; }

        [Required]
        public string SupplierId { get; set; }

        [Required]
        [MaxLength(150)]

        public string SupplierReference { get; set; }
        [Required]
        public decimal PurchasePrice { get; set; }
    }
}