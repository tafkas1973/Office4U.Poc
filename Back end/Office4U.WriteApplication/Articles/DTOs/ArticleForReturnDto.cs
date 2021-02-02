using Office4U.Domain.Model.Articles.Entities;
using System.Collections.Generic;

namespace Office4U.WriteApplication.Articles.DTOs
{
    public class ArticleForReturnDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string SupplierId { get; set; }       
        public string SupplierReference { get; set; }
        public string Name1 { get; set; }
        public string Unit { get; set; }
        public decimal PurchasePrice { get; set; }
        public ICollection<ArticlePhotoForReturnDto> Photos { get; set; }
    }
}