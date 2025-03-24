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
            Debug.WriteLine(administrator.AdministratorLogin);
            InitializeComponent();
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
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.Pickets)
                    .ToList();
                treeView.ItemsSource = contracts;
                Debug.WriteLine(contracts[0].Areas.ToString());
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

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeView.SelectedItem is TreeViewItem selectedItem)
            {
                infoLabel.Content = selectedItem.Header.ToString();
            }

        }
    }
}
