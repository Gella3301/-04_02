using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace Практика04_02
{
    public partial class Manager : Window
    {
        private List<Order> orders = new List<Order>();
        private List<Client> clients = new List<Client>();
        private Order selectedOrder;
        private Client selectedClient;

        public Manager()
        {
            InitializeComponent();
            LoadDataFromFile();
            LoadClientsData();
            RefreshDataGrid();
        }

        private void LoadDataFromFile()
        {
            orders = OrderManager.LoadOrdersData();

            if (orders == null || orders.Count == 0)
            {
                LoadSampleData();
                OrderManager.SaveOrdersData(orders);
            }
        }

        private void LoadClientsData()
        {
            try
            {
                clients = ClientManager.LoadClientsData();
                ClientComboBox.ItemsSource = clients;

                if (clients != null && clients.Count > 0)
                {
                    ClientComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке клиентов: {ex.Message}");
            }
        }

        private void LoadSampleData()
        {
           
        }

        private void RefreshDataGrid()
        {
            TablesGrid.ItemsSource = null;
            TablesGrid.ItemsSource = orders;
        }

        private void ClearInputFields()
        {
            FIOBox.Text = "";
            IDBox.Text = "";
            StatusBox.SelectedIndex = -1;
            DescBox.Text = "";
            selectedOrder = null;
            ClientComboBox.SelectedIndex = -1;
        }

        private void ClientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientComboBox.SelectedItem != null && ClientComboBox.SelectedItem is Client)
            {
                selectedClient = (Client)ClientComboBox.SelectedItem;
                FIOBox.Text = selectedClient.FullName;

              
                if (string.IsNullOrEmpty(IDBox.Text))
                {
                    IDBox.Text = selectedClient.OrderCode;
                }

            }
        }

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TablesGrid.SelectedItem != null && TablesGrid.SelectedItem is Order)
            {
                selectedOrder = (Order)TablesGrid.SelectedItem;
                FIOBox.Text = selectedOrder.FullName;
                IDBox.Text = selectedOrder.OrderID;

                for (int i = 0; i < StatusBox.Items.Count; i++)
                {
                    ComboBoxItem item = (ComboBoxItem)StatusBox.Items[i];
                    if (item.Content.ToString() == selectedOrder.Status)
                    {
                        StatusBox.SelectedIndex = i;
                        break;
                    }
                }

                DescBox.Text = selectedOrder.Description;

                foreach (Client client in clients)
                {
                    if (client.FullName == selectedOrder.FullName)
                    {
                        ClientComboBox.SelectedItem = client;
                        break;
                    }
                }
            }
            else
            {
                ClearInputFields();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FIOBox.Text) ||
                    string.IsNullOrWhiteSpace(IDBox.Text) ||
                    StatusBox.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все обязательные поля (ФИО, ID заказа, Статус)");
                    return;
                }

                
                foreach (Order order in orders)
                {
                    if (order.OrderID == IDBox.Text)
                    {
                        MessageBox.Show("Заказ с таким ID уже существует!");
                        return;
                    }
                }

                ComboBoxItem selectedStatus = (ComboBoxItem)StatusBox.SelectedItem;

                Order newOrder = new Order
                {
                    FullName =FIOBox.Text,
                    OrderID = IDBox.Text,
                    Status = selectedStatus.Content.ToString(),
                    Description = DescBox.Text,
                    Discount = selectedClient?.Discount ?? 0.0 
                };

                orders.Add(newOrder);
                RefreshDataGrid();
                ClearInputFields();
                OrderManager.SaveOrdersData(orders);

                MessageBox.Show("Заказ успешно добавлен!" +
                    (selectedClient?.IsRegularCustomer == true ?
                    $"\nПрименена скидка: {selectedClient.Discount}%" : ""));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении заказа: {ex.Message}");
            }
        }
       

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedOrder == null)
                {
                    MessageBox.Show("Выберите заказ для редактирования!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(FIOBox.Text) ||
                    string.IsNullOrWhiteSpace(IDBox.Text) ||
                    StatusBox.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все обязательные поля");
                    return;
                }

                ComboBoxItem selectedStatus = (ComboBoxItem)StatusBox.SelectedItem;

                selectedOrder.FullName= FIOBox.Text;
                selectedOrder.OrderID = IDBox.Text;
                selectedOrder.Status = selectedStatus.Content.ToString();
                selectedOrder.Description = DescBox.Text;

                RefreshDataGrid();
                ClearInputFields();
                OrderManager.SaveOrdersData(orders);

                MessageBox.Show("Изменения сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении заказа: {ex.Message}");
            }
        }

      private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedOrder == null)
                {
                    MessageBox.Show("Выберите заказ для удаления!");
                    return;
                }

                MessageBoxResult result = MessageBox.Show(
                    "Вы уверены, что хотите удалить этот заказ?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    orders.Remove(selectedOrder);
                    RefreshDataGrid();
                    ClearInputFields();
                    OrderManager.SaveOrdersData(orders);

                    MessageBox.Show("Заказ удален!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении заказа: {ex.Message}");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            OrderManager.SaveOrdersData(orders);
            base.OnClosed(e);
        }
    }
}