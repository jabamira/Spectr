using Spectr.Data;
using System.Windows;
using System.Windows.Controls;

namespace Spectr
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {

        public AuthPage()
        {


            InitializeComponent();
            typeUserComboBox.ItemsSource = Enum.GetValues(typeof(UserType)).Cast<UserType>();
            typeUserComboBox.SelectedIndex = 0;
            Login_box.Text = "admin_ivan";
            Password_box.Text = "pass_ivan123";



        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = Login_box.Text;
            string password = Password_box.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (typeUserComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип пользователя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UserType selectedUserType = (UserType)typeUserComboBox.SelectedItem;
            ApplicationContext context = new ApplicationContext();
            AuthService authService = new AuthService(context);
            object user = authService.AuthenticateUser(username, password, selectedUserType);

            if (user != null)
            {
                MessageBox.Show("Авторизация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);




                switch (selectedUserType)
                {
                    case UserType.Администратор:
                        this.NavigationService.Navigate(new AdministratorPage((Administrator)user));
                        break;
                    case UserType.Оператор:
                        //this.NavigationService.Navigate(new AdministratorPage());

                        break;
                    case UserType.Заказчик:
                        //  this.NavigationService.Navigate(new AdministratorPage());
                        break;
                    case UserType.Аналитик:
                        //this.NavigationService.Navigate(new AdministratorPage());
                        break;
                    default:
                        MessageBox.Show("Неизвестный тип пользователя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                }


            }
            else
            {
                MessageBox.Show("Ошибка входа! Проверьте логин и пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
