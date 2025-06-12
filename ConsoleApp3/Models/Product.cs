namespace MyBaseLinkerProject.Models
{
    public class Product
    {
        public string storage { get; set; }
        public int storage_id { get; set; }
        public int order_product_id { get; set; }
        public string product_id { get; set; }
        public string variant_id { get; set; }
        public string name { get; set; }
        public string attributes { get; set; }
        public string sku { get; set; }
        public string ean { get; set; }
        public string location { get; set; }
        public int warehouse_id { get; set; }
        public string auction_id { get; set; }
        public decimal price_brutto { get; set; }
        public int tax_rate { get; set; }
        public int quantity { get; set; }
        public decimal weight { get; set; }
        public int bundle_id { get; set; }
    }
}
