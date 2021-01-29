namespace Office4U.Common
{
    public class ArticleParams : ParamsBase
    {
        public string Code { get; set; }
        public string SupplierId { get; set; }
        public string SupplierReference { get; set; }
        public string Name1 { get; set; }
        public string Unit { get; set; }
        public decimal? PurchasePriceMin { get; set; }
        public decimal? PurchasePriceMax { get; set; }
        public string OrderBy { get; set; } = "Code";
    }
}
