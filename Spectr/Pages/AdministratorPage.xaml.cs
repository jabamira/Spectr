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
                                .ThenInclude(o=>o.Operator)
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
                ResetVisibility();
                listViewArea.Visibility = Visibility.Visible;
                listViewArea.Items.Refresh();

                 List<Operator> allOperators = selectedContract.Areas
                .SelectMany(a => a.Profiles)
                .SelectMany(p => p.Pickets)
                .Select(pk => pk.Operator)
                .Distinct()
                .ToList();

                listViewOperators.ItemsSource = allOperators;
                listViewOperators.Items.Refresh();
            }
            if (treeView.SelectedItem is Area selecteArea)
            {
                listViewProfile.ItemsSource = selecteArea.Profiles;
                listViewAreaCoordinates.ItemsSource = selecteArea.AreaCoordinates;

                ResetVisibility();
                listViewProfile.Visibility = Visibility.Visible;
                listViewAreaCoordinates.Visibility = Visibility.Visible;
                listViewProfile.Items.Refresh();
                listViewAreaCoordinates.Items.Refresh();

                List<Operator> allOperators = selecteArea.Profiles
                .SelectMany(p => p.Pickets)
                .Select(pk => pk.Operator)
                .Distinct()
                .ToList();

                listViewOperators.ItemsSource = allOperators;
                listViewOperators.Items.Refresh();
            }
            if (treeView.SelectedItem is Profile selectedProfile)
            {
                
                listViewPickets.ItemsSource = selectedProfile.Pickets;
                listViewProfileCoordinates.ItemsSource = selectedProfile.ProfileCoordinates;
                
                ResetVisibility();
                listViewPickets.Visibility = Visibility.Visible;
                listViewProfileCoordinates.Visibility = Visibility.Visible;
               
                listViewPickets.Items.Refresh();
                listViewProfileCoordinates.Items.Refresh();

                List<Operator> allOperators = selectedProfile.Pickets
              .Select(pk => pk.Operator)
              .Distinct()
              .ToList();

                listViewOperators.ItemsSource = allOperators;
                listViewOperators.Items.Refresh();
            }
            if (treeView.SelectedItem is Picket selectedPicket)
            {
                ResetVisibility();
                listViewPickets.Items.Refresh();

                List<Operator> allOperators = new List<Operator> { selectedPicket.Operator };

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
            listViewProfileCoordinates.Visibility = Visibility.Collapsed;
            listViewAreaCoordinates.Visibility = Visibility.Collapsed;
        }

    }
}
