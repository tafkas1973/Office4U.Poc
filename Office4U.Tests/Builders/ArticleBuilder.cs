using Office4U.Domain.Model.Articles.Entities;
using System.Collections.Generic;

namespace Office4U.Tests.Builders
{
    public class ArticleBuilder
    {
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
            _name1 = "name";
            _purchasePrice = 10;
            _photos = new List<ArticlePhoto>();
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

        public ArticleBuilder WithPhotos(ICollection<ArticlePhoto> photos)
        {
            _photos = photos;
            return this;
        }

        public Article Build()
        {
            var article = Article.Create(_code, _name1, _supplierId, _supplierReference, _purchasePrice, _unit, _photos);
            article.AddPhotos(_photos);
            return article;
        }
    }
}
