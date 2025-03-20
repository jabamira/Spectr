using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;
using Spectr.Data;

namespace Spectr
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        static string servername = "DBSRV\\ag2024";
        static  string dbName = "LesnikovAA_2207g2_spectr3";
        public string servername_ = "ZALMAN\\SQLEXPRESS";
        public string connectionString = $"Server={servername};Database={dbName};Integrated Security=True;TrustServerCertificate=True;";
        public AuthPage()
        {
          
            InitializeComponent();
            typeUserComboBox.ItemsSource = Enum.GetValues(typeof(UserType)).Cast<UserType>();
            Login_box.Text = "admin_ivan";
            Password_box.Text = "pas2s_ivan123";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Подключение успешно установлено.");
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка подключения: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }


        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = Login_box.Text;
            string password = Password_box.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "";

                // Определяем тип пользователя в зависимости от выбора в комбо-боксе
                if (typeUserComboBox.SelectedItem.ToString() == "Оператор")
                {
                    query = "SELECT COUNT(1) FROM Operator WHERE OperatorLogin = @username AND OperatorPassword = @password";
                }
                else if (typeUserComboBox.SelectedItem.ToString() == "Аналитик")
                {
                    query = "SELECT COUNT(1) FROM Analyst WHERE AnalystLogin = @username AND AnalystPassword = @password";
                }
                else if (typeUserComboBox.SelectedItem.ToString() == "Администратор")
                {
                    query = "SELECT COUNT(1) FROM Administrator WHERE AdministratorLogin = @username AND AdministratorPassword = @password";
                }
                else if (typeUserComboBox.SelectedItem.ToString() == "Заказчик")
                {
                    query = "SELECT COUNT(1) FROM Customer WHERE CustomerLogin = @username AND CustomerPassword = @password";
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите тип пользователя. 😊");
                    return;
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    try
                    {
                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count == 1)
                        {
                            switch (typeUserComboBox.SelectedItem.ToString())
                            {
                                case "Оператор":
                                    NavigationService.Navigate(new AdministratorPage());
                                    break;
                                case "Аналитик":
                                    NavigationService.Navigate(new AdministratorPage());
                                    break;
                                case "Администратор":
                                    NavigationService.Navigate(new AdministratorPage());
                                    break;
                                case "Заказчик":
                                    NavigationService.Navigate(new AdministratorPage());
                                    break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль. Попробуйте еще раз. 😢");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при авторизации: {ex.Message}");
                    }
                }
            }
        }
       

    }
}
