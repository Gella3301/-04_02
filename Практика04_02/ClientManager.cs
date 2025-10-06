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
    public static class ClientManager
    {
        private static string clientsFileName = "clients.json";

        public static void SaveClientsData(List<Client> clients)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(clients, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(clientsFileName, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении клиентов: {ex.Message}");
            }
        }

        public static List<Client> LoadClientsData()
        {
            try
            {
                if (File.Exists(clientsFileName))
                {
                    string jsonString = File.ReadAllText(clientsFileName);
                    return JsonSerializer.Deserialize<List<Client>>(jsonString) ?? new List<Client>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}");
            }
            return new List<Client>();
        }
    }

}
