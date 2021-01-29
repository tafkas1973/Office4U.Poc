using System.Collections.Generic;

namespace Office4U.Domain.Model.Entities.Articles
{
    public class Article
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string SupplierId { get; set; }
        public string SupplierReference { get; set; }
        public string Name1 { get; set; }
        public string Unit { get; set; }
        public decimal PurchasePrice { get; set; }
        public ICollection<ArticlePhoto> Photos { get; set; }

        public static Article Create(
                string code,
                string supplierId,
                string supplierReference,
                string name1,
                decimal purchasePrice,
                string unit
            )
        {
            return new Article()
            {
                Code = code,
                SupplierId = supplierId,
                SupplierReference = supplierReference,
                Name1 = name1,
                PurchasePrice = purchasePrice,
                Unit = unit
            };
        }
    }
}