using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Практика04_02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string login;
        private string password1;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void regiztrationButton_Click(object sender, RoutedEventArgs e)
        {

            login = loginTextBox.Text;
            password1 = passwordBox1.Password;


            if (String.IsNullOrEmpty(login))
            {
                MessageBox.Show("логин пустой!");
                return;
            }

            if (login != null && password1 != null)
            {
                MessageBox.Show($"регистрация прошла успешно\n" +
                    $"ваш логин - {login}\n" +
                    $"ваш пароль - {password1}");
            }
            else
            {
                MessageBox.Show("Поля логина и/или пароля не могут быть пустыми");
                return;
            }
            if (login == "manager" && password1 == "1111")
            {
                new Manager().Show(); 
            }
            else if (login == "admin" && password1 == "0000")
            {
                new Manager().Show();
            }
            else {
                MessageBox.Show("Неверный логин или пароль!");
                return;
            }
            this.Close();

        }


    }
}