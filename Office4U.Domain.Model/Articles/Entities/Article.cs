using System.Collections.Generic;
using System.Linq;

namespace Office4U.Domain.Model.Articles.Entities
{
    public class Article
    {
        public int Id { get; private set; }
        public string Code { get; private set; }
        public string Name1 { get; private set; }
        public string SupplierId { get; private set; }
        public string SupplierReference { get; private set; }
        public string Unit { get; private set; }
        public decimal PurchasePrice { get; private set; }
        public ICollection<ArticlePhoto> Photos { get; private set; }

        public static Article Create(
                string code,
                string name1,
                string supplierId,
                string supplierReference,
                decimal purchasePrice,
                string unit
            )
        {
            var article = new Article()
            {
                Code = code,
                Name1 = name1,
                SupplierId = supplierId,
                SupplierReference = supplierReference,
                PurchasePrice = purchasePrice,
                Unit = unit,
                Photos = new List<ArticlePhoto>()
            };
            return article;
        }

        public void SetCode(string code)
        {
            Code = code;
        }


        public void AddPhotos(ICollection<ArticlePhoto> photos)
        {
            Photos.ToList().AddRange(photos);
        }

        public void AddPhoto(ArticlePhoto photo)
        {
            Photos.Add(photo);
        }
    }
}