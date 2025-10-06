using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace Практика04_02
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private List<Client> clients = new List<Client>();
        private Client selectedClient;

        public AdminWindow()
        {
            InitializeComponent();
            LoadClientsData();
            RefreshClientsGrid();
        }

        private void LoadClientsData()
        {
            try
            {
                clients = ClientManager.LoadClientsData();
                if (clients == null || clients.Count == 0)
                {
                    LoadSampleData();
                    ClientManager.SaveClientsData(clients);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}");
                LoadSampleData();
            }
        }

        private void LoadSampleData()
        {
            clients.Add(new Client
            {
                FullName = "Иванов Иван Иванович",
                OrderCode = "ORD001",
                Phone = "+7 (912) 345-67-89",
                IsRegularCustomer = true,
                OrderCount = 5,
                Discount = 10.0
            });

            clients.Add(new Client
            {
                FullName = "Петрова Анна Сергеевна",
                OrderCode = "ORD002",
                Phone = "+7 (923) 456-78-90",
                IsRegularCustomer = false,
                OrderCount = 1,
                Discount = 0.0
            });
        }

        private void RefreshClientsGrid()
        {
            ClientsGrid.ItemsSource = null;
            ClientsGrid.ItemsSource = clients;
        }

        private void ClearInputFields()
        {
            FullNameBox.Text = "";
            OrderCodeBox.Text = "";
            PhoneBox.Text = "";
            RegularCustomerCheckBox.IsChecked = false;
            OrderCountBox.Text = "";
            DiscountBox.Text = "";
            selectedClient = null;
        }

        private void RegularCustomerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            OrderCountBox.IsEnabled = true;
            DiscountBox.IsEnabled = true;

            if (string.IsNullOrEmpty(OrderCountBox.Text))
                OrderCountBox.Text = "1";
            if (string.IsNullOrEmpty(DiscountBox.Text))
                DiscountBox.Text = "5";
        }

        private void RegularCustomerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            OrderCountBox.IsEnabled = false;
            DiscountBox.IsEnabled = false;
            OrderCountBox.Text = "0";
            DiscountBox.Text = "0";
        }

        private void ClientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsGrid.SelectedItem != null && ClientsGrid.SelectedItem is Client)
            {
                selectedClient = (Client)ClientsGrid.SelectedItem;
                FullNameBox.Text = selectedClient.FullName;
                OrderCodeBox.Text = selectedClient.OrderCode;
                PhoneBox.Text = selectedClient.Phone;
                RegularCustomerCheckBox.IsChecked = selectedClient.IsRegularCustomer;
                OrderCountBox.Text = selectedClient.OrderCount.ToString();
                DiscountBox.Text = selectedClient.Discount.ToString();
            }
            else
            {
                ClearInputFields();
            }
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FullNameBox.Text) ||
                    string.IsNullOrWhiteSpace(OrderCodeBox.Text) ||
                    string.IsNullOrWhiteSpace(PhoneBox.Text))
                {
                    MessageBox.Show("Заполните все обязательные поля!");
                    return;
                }

              
                foreach (Client client in clients)
                {
                    if (client.OrderCode == OrderCodeBox.Text)
                    {
                        MessageBox.Show("Клиент с таким кодом заказа уже существует!");
                        return;
                    }
                }

                Client newClient = new Client
                {
                    FullName = FullNameBox.Text,
                    OrderCode = OrderCodeBox.Text,
                    Phone = PhoneBox.Text,
                    IsRegularCustomer = RegularCustomerCheckBox.IsChecked ?? false,
                    OrderCount = int.TryParse(OrderCountBox.Text, out int orderCount) ? orderCount : 0,
                    Discount = double.TryParse(DiscountBox.Text, out double discount) ? discount : 0.0
                };

                clients.Add(newClient);
                RefreshClientsGrid();
                ClearInputFields();
                ClientManager.SaveClientsData(clients);

                MessageBox.Show("Клиент успешно добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}");
            }
        }

        private void UpdateClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedClient == null)
                {
                    MessageBox.Show("Выберите клиента для редактирования!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(FullNameBox.Text) ||
                    string.IsNullOrWhiteSpace(OrderCodeBox.Text) ||
                    string.IsNullOrWhiteSpace(PhoneBox.Text))
                {
                    MessageBox.Show("Заполните все обязательные поля!");
                    return;
                }

                selectedClient.FullName = FullNameBox.Text;
                selectedClient.OrderCode = OrderCodeBox.Text;
                selectedClient.Phone = PhoneBox.Text;
                selectedClient.IsRegularCustomer = RegularCustomerCheckBox.IsChecked ?? false;
                selectedClient.OrderCount = int.TryParse(OrderCountBox.Text, out int orderCount) ? orderCount : 0;
                selectedClient.Discount = double.TryParse(DiscountBox.Text, out double discount) ? discount : 0.0;

                RefreshClientsGrid();
                ClearInputFields();
                ClientManager.SaveClientsData(clients);

                MessageBox.Show("Изменения сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении клиента: {ex.Message}");
            }
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedClient == null)
                {
                    MessageBox.Show("Выберите клиента для удаления!");
                    return;
                }

                MessageBoxResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить клиента {selectedClient.FullName}?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    clients.Remove(selectedClient);
                    RefreshClientsGrid();
                    ClearInputFields();
                    ClientManager.SaveClientsData(clients);

                    MessageBox.Show("Клиент удален!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}");
            }
        }

        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
            ClearInputFields();
        }

        protected override void OnClosed(EventArgs e)
        {
            ClientManager.SaveClientsData(clients);
            base.OnClosed(e);
        }
    }
}