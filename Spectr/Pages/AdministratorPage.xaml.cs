using Microsoft.EntityFrameworkCore;
using Spectr.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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
using Contract = Spectr.Data.Contract;

namespace Spectr
{
    /// <summary>
    /// Логика взаимодействия для AdministratorPage.xaml
    /// </summary>
    public partial class AdministratorPage : Page
    {
        Administrator Administrator;

        public AdministratorPage(Administrator administrator)
        {
            Administrator = administrator;
         
            InitializeComponent();
            LabelLogin.Content = administrator.AdministratorLogin;
            LoadContracts();
        }
        private void LoadContracts()
        {
            using (ApplicationContext _context = new ApplicationContext())
            {
                var contracts = _context.Contracts
                    .Include(c => c.Customer)
                    .Include(c => c.Administrator)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.AreaCoordinates) // Включаем координаты зон
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.ProfileCoordinates) // Включаем координаты профилей
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.Pickets) // Включаем пикеты
                                .ThenInclude(pk => pk.Operator)
                    .Include(c => c.ContractAnalysts) // Включаем связи контракт-аналитик
                        .ThenInclude(ca => ca.Analyst) // Включаем сами объекты аналитиков
                    .ToList();

                treeView.ItemsSource = contracts;
            }




            // Устанавливаем контракты в ItemsSource для TreeView

        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.NavigationService.Navigate(new AuthPage());
            }
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnGraphik_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSchem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void treeView_SelectedItemChanged_1(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeView.SelectedItem is Contract selectedContract)
            {
                listViewArea.ItemsSource = selectedContract.Areas;

                listViewAnalysts.ItemsSource = selectedContract.ContractAnalysts
               .Select(ca => ca.Analyst)
               .Distinct()
               .ToList();

                List<Operator> allOperators = selectedContract.Areas
              .SelectMany(a => a.Profiles)
              .SelectMany(p => p.Pickets)
              .Select(pk => pk.Operator)
              .Distinct()
              .ToList();

                listViewOperators.ItemsSource = allOperators;


                ResetVisibility();

                labelAnalystHeader.Visibility = Visibility.Visible;
                labelAnalystHeader.Content = $"Аналитики Контракта:{selectedContract.ContractID}";
                labelAreaHeader.Visibility = Visibility.Visible;
                labelAreaHeader.Content = $"Площади Контракта:{selectedContract.ContractID}";
                listViewArea.Visibility = Visibility.Visible;
                listViewAnalysts.Visibility = Visibility.Visible;
                labelOperatorsHeader.Content = $"Операторы задействованные в выполнении Контракта:{selectedContract.ContractID}";

                listViewAnalysts.Items.Refresh();
                listViewArea.Items.Refresh();
                listViewOperators.Items.Refresh();
            }
            if (treeView.SelectedItem is Area selecteArea)
            {
                listViewProfile.ItemsSource = selecteArea.Profiles;
                listViewAreaCoordinates.ItemsSource = selecteArea.AreaCoordinates;
                List<Operator> allOperators = selecteArea.Profiles
               .SelectMany(p => p.Pickets)
               .Select(pk => pk.Operator)
               .Distinct()
               .ToList();

                listViewOperators.ItemsSource = allOperators;

                ResetVisibility();
                
                listViewProfile.Visibility = Visibility.Visible;
                labelProfilesHeader.Visibility = Visibility.Visible;
                labelProfilesHeader.Content = $"Профили Площади:{selecteArea.AreaName},{selecteArea.AreaID}";
                labelAreaCoordinatesHeader.Visibility = Visibility.Visible;
                labelAreaCoordinatesHeader.Content = $"Координаты Площади:{selecteArea.AreaName},{selecteArea.AreaID}";
                listViewAreaCoordinates.Visibility = Visibility.Visible;
                labelOperatorsHeader.Content = $"Операторы задействованные на Площади:{selecteArea.AreaName},{selecteArea.AreaID}";

                listViewProfile.Items.Refresh();
                listViewAreaCoordinates.Items.Refresh();
                listViewOperators.Items.Refresh();
            }
            if (treeView.SelectedItem is Profile selectedProfile)
            {
                
                listViewPickets.ItemsSource = selectedProfile.Pickets;
                listViewProfileCoordinates.ItemsSource = selectedProfile.ProfileCoordinates;
                List<Operator> allOperators = selectedProfile.Pickets
                .Select(pk => pk.Operator)
                .Distinct()
                .ToList();

                listViewOperators.ItemsSource = allOperators;

                ResetVisibility();

                listViewPickets.Visibility = Visibility.Visible;
                listViewProfileCoordinates.Visibility = Visibility.Visible;
                labelPicketsHeader.Visibility = Visibility.Visible;
                labelPicketsHeader.Content = $"Пикеты Профиля:{selectedProfile.ProfileName},{selectedProfile.ProfileID}";
                labelProfilesHeader.Visibility =Visibility.Visible;
                labelProfilesHeader.Content = $"Координаты Профиля:{selectedProfile.ProfileName},{selectedProfile.ProfileID}";
                labelOperatorsHeader.Content = $"Операторы задействованные на Профиле:{selectedProfile.ProfileName},{selectedProfile.ProfileID}";


                listViewPickets.Items.Refresh();
                listViewProfileCoordinates.Items.Refresh();
                listViewOperators.Items.Refresh();
            }
            if (treeView.SelectedItem is Picket selectedPicket)
            {
                ResetVisibility();
                listViewPickets.Items.Refresh();

                List<Operator> allOperators = new List<Operator> { selectedPicket.Operator };
                labelOperatorsHeader.Content = $"Оператор пикета:{selectedPicket.PicketID}";
                listViewOperators.ItemsSource = allOperators;
                listViewOperators.Items.Refresh();
            }
            Debug.WriteLine(1);
           
        }
        private void ResetVisibility()
        {
            listViewArea.Visibility = Visibility.Collapsed;
            listViewPickets.Visibility = Visibility.Collapsed;
            listViewProfile.Visibility = Visibility.Collapsed;
            listViewAnalysts.Visibility = Visibility.Collapsed;
            listViewProfileCoordinates.Visibility = Visibility.Collapsed;
            listViewAreaCoordinates.Visibility = Visibility.Collapsed;
            
            
            labelPicketsHeader.Visibility = Visibility.Collapsed;
            labelProfilesHeader.Visibility = Visibility.Collapsed;
            labelAreaHeader.Visibility = Visibility.Collapsed;
            labelProfileCoordinatesHeader.Visibility = Visibility.Collapsed;
            labelAreaCoordinatesHeader.Visibility = Visibility.Collapsed;
            labelAnalystHeader.Visibility = Visibility.Collapsed;
        }

    }
}
