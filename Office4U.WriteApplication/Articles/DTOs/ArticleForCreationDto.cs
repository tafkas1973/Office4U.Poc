using System.ComponentModel.DataAnnotations;

namespace Office4U.WriteApplication.Article.DTOs
{
    public class ArticleForCreationDto
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string SupplierId { get; set; }

        [Required]
        [MaxLength(150)]
        public string SupplierReference { get; set; }
         [Required]
        public string Name1 { get; set; }
        [Required]
        public string Unit { get; set; }
        [Required]
        [Range(0.01, 99999.99)]
        public decimal PurchasePrice { get; set; }
    }
}