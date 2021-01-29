using Office4U.Articles.Domain.Model.Entities.Articles;
using System.Collections.Generic;

namespace Retail4U.Office4U.WebApi.Tools.Data
{
    public class ArticleBuilder
    {
        private int _id;
        private string _code;
        private string _supplierId;
        private string _supplierReference;
        private string _name1;
        private string _unit;
        private decimal _purchasePrice;
        private ICollection<ArticlePhoto> _photos;

        public ArticleBuilder()
        {
            WithDefault();
        }

        public ArticleBuilder WithDefault()
        {
            _id = 1;
            _name1 = "name";
            _purchasePrice = 10;
            _photos = new List<ArticlePhoto>();
            return this;
        }

        public ArticleBuilder WithId(int value)
        {
            _id = value;
            return this;
        }


        public ArticleBuilder WithCode(string value)
        {
            _code = value;
            return this;
        }

        public ArticleBuilder WithSupplierId(string value)
        {
            _supplierId = value;
            return this;
        }

        public ArticleBuilder WithSupplierReference(string value)
        {
            _supplierReference = value;
            return this;
        }

        public ArticleBuilder WithName1(string value)
        {
            _name1 = value;
            return this;
        }

        public ArticleBuilder WithUnit(string value)
        {
            _unit = value;
            return this;
        }

        public ArticleBuilder WithPurchasePrice(decimal value)
        {
            _purchasePrice = value;
            return this;
        }

        public Article Build()
        {
            var _article = new Article()
            {
                Id = _id,
                Code = _code,
                SupplierId = _supplierId,
                SupplierReference = _supplierReference,
                Name1 = _name1,
                Unit = _unit,
                PurchasePrice = _purchasePrice,
                Photos = _photos
            };
            return _article;
        }
    }
}
