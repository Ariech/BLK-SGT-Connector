using System.Collections.Generic;

namespace MyBaseLinkerProject.Models
{
    public class OrderResponse
    {
        public string status { get; set; }
        public List<Order> orders { get; set; }
    }
}
