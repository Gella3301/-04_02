using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace Практика04_02
{
    public static class OrderManager
    {
        private static string ordersFileName = "orders.json";

        public static void SaveOrdersData(List<Order> orders)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ordersFileName, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заказов: {ex.Message}");
            }
        }

        public static List<Order> LoadOrdersData()
        {
            try
            {
                if (File.Exists(ordersFileName))
                {
                    string jsonString = File.ReadAllText(ordersFileName);
                    return JsonSerializer.Deserialize<List<Order>>(jsonString) ?? new List<Order>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}");
            }
            return new List<Order>();
        }
    }
}
