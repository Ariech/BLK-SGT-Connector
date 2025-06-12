using System.Collections.Generic;

namespace MyBaseLinkerProject.Models
{
    public class Order
    {
        public int order_id { get; set; }
        public string delivery_fullname { get; set; }
        public string email { get; set; }
        public string delivery_address { get; set; }
        public string delivery_city { get; set; }
        public string delivery_postcode { get; set; }
        public string delivery_country { get; set; }
        public string invoice_fullname { get; set; }
        public string invoice_address { get; set; }
        public string invoice_city { get; set; }
        public string invoice_postcode { get; set; }
        public string invoice_country { get; set; }
        public string invoice_nip { get; set; }
        public List<Product> products { get; set; }
    }
}
