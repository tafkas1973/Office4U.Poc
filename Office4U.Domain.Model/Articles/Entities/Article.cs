using System.Collections.Generic;

namespace Office4U.Domain.Model.Articles.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string SupplierId { get; set; }
        public string SupplierReference { get; set; }
        public string Unit { get; set; }
        public decimal PurchasePrice { get; set; }
        public ICollection<ArticlePhoto> Photos { get; set; }

        public static Article Create(
                string code,
                string name1,
                string supplierId,
                string supplierReference,
                decimal purchasePrice,
                string unit
            )
        {
            return new Article()
            {
                Code = code,
                Name1 = name1,
                SupplierId = supplierId,
                SupplierReference = supplierReference,
                PurchasePrice = purchasePrice,
                Unit = unit
            };
        }
    }
}