using Office4U.Domain.Model.Articles.Entities;
using Office4U.Tests.Builders;
using System.Collections.Generic;

namespace Office4U.Tests.TestData
{
    public class ArticleList
    {
        public static List<Article> GetDefaultList()
        {
            // sort by name1 : 10th article/11th article/12th article/1st article/2nd article/3rd article/4th article/5th article/6th article/7th article/8th article/9th article
            // sort by supref : sup1 ref 1/sup1 ref 2/sup2 ref 1/sup2 ref 2/sup3 ref 1/sup3 ref 2/sup4 ref 1/sup4 ref 2/sup5 ref 1/sup5 ref 2/sup6 ref 1/sup6 ref 2

            return new List<Article>() {
                new ArticleBuilder().WithCode("Article01").WithName1("1st article") .WithSupplierId("sup1") .WithSupplierReference("sup2 ref 1").WithUnit("ST").WithPurchasePrice(10.00M)
                    .WithPhotos(new List<ArticlePhoto>() { ArticlePhoto.Create("www.retail4u.be/article1.jpg", true) } )
                    .Build(),
                new ArticleBuilder().WithCode("Article02").WithName1("2nd article") .WithSupplierId("sup2") .WithSupplierReference("sup2 ref 2").WithUnit("ST").WithPurchasePrice(20.00M)
                    .WithPhotos(new List<ArticlePhoto>() { ArticlePhoto.Create("www.retail4u.be/article2.jpg", true) } )
                    .Build(),
                new ArticleBuilder().WithCode("Article03").WithName1("3rd article") .WithSupplierId("sup3") .WithSupplierReference("sup4 ref 1").WithUnit("ST").WithPurchasePrice(30.00M)
                    .WithPhotos(new List<ArticlePhoto>() { ArticlePhoto.Create("www.retail4u.be/article3.jpg", true) } )
                    .Build(),
                new ArticleBuilder().WithCode("Article04").WithName1("4th article") .WithSupplierId("sup4") .WithSupplierReference("sup4 ref 2").WithUnit("ST").WithPurchasePrice(40.00M).Build(),
                new ArticleBuilder().WithCode("Article05").WithName1("5th article") .WithSupplierId("sup5") .WithSupplierReference("sup6 ref 1").WithUnit("BS").WithPurchasePrice(50.00M).Build(),
                new ArticleBuilder().WithCode("Article06").WithName1("6th article") .WithSupplierId("sup6") .WithSupplierReference("sup6 ref 2").WithUnit("ST").WithPurchasePrice(60.00M).Build(),
                new ArticleBuilder().WithCode("Article07").WithName1("7th article") .WithSupplierId("sup7") .WithSupplierReference("sup1 ref 1").WithUnit("BS").WithPurchasePrice(70.00M).Build(),
                new ArticleBuilder().WithCode("Article08").WithName1("8th article") .WithSupplierId("sup8") .WithSupplierReference("sup1 ref 2").WithUnit("ST").WithPurchasePrice(80.00M).Build(),
                new ArticleBuilder().WithCode("Article09").WithName1("9th article") .WithSupplierId("sup9") .WithSupplierReference("sup3 ref 1").WithUnit("ST").WithPurchasePrice(90.00M).Build(),
                new ArticleBuilder().WithCode("Article10").WithName1("10th article").WithSupplierId("sup10").WithSupplierReference("sup3 ref 2").WithUnit("BX").WithPurchasePrice(100.00M).Build(),
                new ArticleBuilder().WithCode("Article11").WithName1("11th article").WithSupplierId("sup11").WithSupplierReference("sup5 ref 1").WithUnit("BM").WithPurchasePrice(110.00M).Build(),
                new ArticleBuilder().WithCode("Article12").WithName1("12th article").WithSupplierId("sup12").WithSupplierReference("sup5 ref 2").WithUnit("BX").WithPurchasePrice(120.00M).Build()
            };
        }

        public static List<Article> GetShortList()
        {
            return new List<Article>() {
                new ArticleBuilder().WithCode("Article1").WithName1("1st article").WithSupplierId("sup1").WithSupplierReference("sup1 ref 1").WithUnit("ST").WithPurchasePrice(10.00M).Build(),
                new ArticleBuilder().WithCode("Article2").WithName1("2nd article").WithSupplierId("sup2").WithSupplierReference("sup1 ref 2").WithUnit("ST").WithPurchasePrice(20.00M).Build(),
                new ArticleBuilder().WithCode("Article3").WithName1("3rd article").WithSupplierId("sup3").WithSupplierReference("sup2 ref 1").WithUnit("ST").WithPurchasePrice(30.00M).Build()
            };
        }
    }
}
