//// OrderParser.cs
//using System.Collections.Generic;
//using Newtonsoft.Json.Linq;
//using SubiektConnector.Models;

//public static class OrderParser
//{
//    public static List<Order> Parse(string json)
//    {
//        var result = new List<Order>();
//        var root = JObject.Parse(json);

//        foreach (var orderJson in root["orders"])
//        {
//            var order = new Order
//            {
//                OrderId = (string)orderJson["order_id"],
//                Customer = new Customer
//                {
//                    FullName = (string)orderJson["delivery_fullname"],
//                    Address = (string)orderJson["delivery_address"],
//                    City = (string)orderJson["delivery_city"],
//                    PostalCode = (string)orderJson["delivery_postcode"],
//                    NIP = (string)orderJson["delivery_company_nip"]
//                },
//                Items = new List<OrderItem>()
//            };

//            foreach (var product in orderJson["products"])
//            {
//                order.Items.Add(new OrderItem
//                {
//                    Name = (string)product["name"],
//                    Quantity = (int)product["quantity"],
//                    Price = (decimal)product["price_brutto"]
//                });
//            }

//            result.Add(order);
//        }

//        return result;
//    }
//}
