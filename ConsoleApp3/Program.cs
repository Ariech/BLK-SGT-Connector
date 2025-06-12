using MyBaseLinkerProject.Models;
using System;
using System.Collections.Generic;


class Program
{
    [STAThread]
    static void Main()
    {
        string apiToken = "";
        var client = new BaseLinkerClient(apiToken);
        var subiektService = new SubiektService();

        try
        {
            // Inicjalizacja Subiekta GT
            subiektService.Init();
            Console.WriteLine("Subiekt GT został uruchomiony pomyślnie.");

            // Pobierz zamówienia
            var orderIds = new List<int> { 14399900 };  // <-- lista zamówień z BLK do pobrania
            List<Order> orders = client.GetOrdersSync(orderIds);

            foreach (var order in orders)
            {
                Console.WriteLine(new string('=', 40));

                string symbol = "BL_" + order.order_id.ToString();
                subiektService.AddOrUpdateContractor(
                    symbol,
                    order.delivery_fullname ?? "",
                    order.email ?? "",
                    order.delivery_address ?? "",
                    order.delivery_city ?? "",
                    order.delivery_postcode ?? "",
                    order.delivery_country,
                    order.invoice_nip ?? ""
                );

                subiektService.AddOrder(order, symbol);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd podczas pobierania lub zapisu zamówień:");
            Console.WriteLine(ex.Message);
        }

        Console.ReadKey();
    }
}