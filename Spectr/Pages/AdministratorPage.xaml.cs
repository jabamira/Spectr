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
using Spectr.Db;
namespace Spectr
{
    /// <summary>
    /// Логика взаимодействия для AdministratorPage.xaml
    /// </summary>
    public partial class AdministratorPage : Page
    {
        Administrator Administrator;
        Db_Helper dbHelper;
        
        public AdministratorPage(Administrator administrator)
        {
            Administrator = administrator;
            InitializeComponent();
            LabelLogin.Content = administrator.AdministratorLogin;
            dbHelper = new();
            dbHelper.LoadContract();
            treeView.ItemsSource = dbHelper.contracts;
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
            dbHelper.DeleteProject((object)treeView.SelectedItem);
            treeView.Items.Refresh();
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
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

       

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void treeView_SelectedItemChanged_1(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ResetVisibility();

            if (treeView.SelectedItem is Contract selectedContract)
            {
                infoLabel.Content = $"Информация о контракте {selectedContract.ContractID}";
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

                infoContractId.Visibility = Visibility.Visible;
                infoContractId.Content = $"ID: {selectedContract.ContractID}";

                infoContractDate.Visibility = Visibility.Visible;
                infoContractDate.Content = $"Дата заключения контракта: {selectedContract.StartDate.ToShortDateString()}   Дата завершения контракта: {selectedContract.EndDate.ToShortDateString()}";

                infoContractServiceDescription.Visibility = Visibility.Visible;
                infoContractServiceDescription.Content = $"Описание услуги: {selectedContract.ServiceDescription}";

                infoContractCustomerinfo.Visibility = Visibility.Visible;
                infoContractCustomerinfo.Text = $"Заказчик: Компания {selectedContract.Customer.CompanyName}\nКонтактное лицо:{selectedContract.Customer.ContactPerson}\nНомер телефона:{selectedContract.Customer.PhoneNumber}\nЭлектронная почта:{selectedContract.Customer.Email}";

                labelAnalystHeader.Visibility = Visibility.Visible;
                labelAnalystHeader.Content = $"Аналитики Контракта: {selectedContract.ContractID}";

                labelAreaHeader.Visibility = Visibility.Visible;
                labelAreaHeader.Content = $"Площади Контракта: {selectedContract.ContractID}";

                labelOperatorsHeader.Visibility = Visibility.Visible;
                labelOperatorsHeader.Content = $"Операторы задействованные в контракте: {selectedContract.ContractID}";

                listViewArea.Visibility = Visibility.Visible;
                listViewAnalysts.Visibility = Visibility.Visible;
                listViewOperators.Visibility = Visibility.Visible;

                listViewAnalysts.Items.Refresh();
                listViewArea.Items.Refresh();
                listViewOperators.Items.Refresh();
            }
            else if (treeView.SelectedItem is Area selectedArea)
            {
                infoLabel.Content = $"Информация о Площади {selectedArea.AreaName}, {selectedArea.AreaID}";
                listViewProfile.ItemsSource = selectedArea.Profiles;
                listViewAreaCoordinates.ItemsSource = selectedArea.AreaCoordinates;

                List<Operator> allOperators = selectedArea.Profiles
                   .SelectMany(p => p.Pickets)
                   .Select(pk => pk.Operator)
                   .Distinct()
                   .ToList();

                listViewOperators.ItemsSource = allOperators;

                infoAreaID.Visibility = Visibility.Visible;
                infoAreaID.Content = $"ID: {selectedArea.AreaID}";

                infoAreaName.Visibility = Visibility.Visible;
                infoAreaName.Content = $"Название: {selectedArea.AreaName}";

                labelProfilesHeader.Visibility = Visibility.Visible;
                labelProfilesHeader.Content = $"Профили Площади: {selectedArea.AreaName}, {selectedArea.AreaID}";

                labelAreaCoordinatesHeader.Visibility = Visibility.Visible;
                labelAreaCoordinatesHeader.Content = $"Координаты Площади: {selectedArea.AreaName}, {selectedArea.AreaID}";

                labelOperatorsHeader.Visibility = Visibility.Visible;
                labelOperatorsHeader.Content = $"Операторы на Площади: {selectedArea.AreaName}, {selectedArea.AreaID}";

                listViewProfile.Visibility = Visibility.Visible;
                listViewAreaCoordinates.Visibility = Visibility.Visible;
                listViewOperators.Visibility = Visibility.Visible;

                listViewProfile.Items.Refresh();
                listViewAreaCoordinates.Items.Refresh();
                listViewOperators.Items.Refresh();
            }
            else if (treeView.SelectedItem is Profile selectedProfile)
            {
                infoLabel.Content = $"Информация о Профиле {selectedProfile.ProfileName}, {selectedProfile.ProfileID}";
                listViewPickets.ItemsSource = selectedProfile.Pickets;
                listViewProfileCoordinates.ItemsSource = selectedProfile.ProfileCoordinates;

                List<Operator> allOperators = selectedProfile.Pickets
                   .Select(pk => pk.Operator)
                   .Distinct()
                   .ToList();

                listViewOperators.ItemsSource = allOperators;

                infoProfileID.Visibility = Visibility.Visible;
                infoProfileID.Content = $"ID: {selectedProfile.ProfileID}";

                infoProfileName.Visibility = Visibility.Visible;
                infoProfileName.Content = $"Название: {selectedProfile.ProfileName}";

                infoProfileType.Visibility = Visibility.Visible;
                infoProfileType.Content = $"Тип: {selectedProfile.ProfileType}";

                labelPicketsHeader.Visibility = Visibility.Visible;
                labelPicketsHeader.Content = $"Пикеты Профиля: {selectedProfile.ProfileName}, {selectedProfile.ProfileID}";

                labelProfilesHeader.Visibility = Visibility.Visible;
                labelProfilesHeader.Content = $"Координаты Профиля: {selectedProfile.ProfileName}, {selectedProfile.ProfileID}";

                labelOperatorsHeader.Visibility = Visibility.Visible;
                labelOperatorsHeader.Content = $"Операторы на Профиле: {selectedProfile.ProfileName}, {selectedProfile.ProfileID}";

                listViewPickets.Visibility = Visibility.Visible;
                listViewProfileCoordinates.Visibility = Visibility.Visible;
                listViewOperators.Visibility = Visibility.Visible;

                listViewPickets.Items.Refresh();
                listViewProfileCoordinates.Items.Refresh();
                listViewOperators.Items.Refresh();
            }
            else if (treeView.SelectedItem is Picket selectedPicket)
            {
                infoLabel.Content = $"Информация о Пикете {selectedPicket.PicketID}";

                infoPicketID.Visibility = Visibility.Visible;
                infoPicketID.Content = $"ID: {selectedPicket.PicketID}";

                infoCoordinate.Visibility = Visibility.Visible;
                infoCoordinate.Content = $"Координата Х: {selectedPicket.CoordinateX} Координата Y: {selectedPicket.CoordinateY}";

                infoChannel.Visibility = Visibility.Visible;
                infoChannel.Content = $"Канал 1: {selectedPicket.Channel1} Канал 2: {selectedPicket.Channel2} Канал 3: {selectedPicket.Channel3}";

                infoProfile.Visibility = Visibility.Visible;
                infoProfile.Content = $"Профиль: {selectedPicket.Profile.ProfileName}";

                infoGammaSpectrometer.Visibility = Visibility.Visible;
                infoGammaSpectrometer.Content = $"Гамма-спектрометр: {selectedPicket.GammaSpectrometer}";

                List<Operator> allOperators = new List<Operator> { selectedPicket.Operator };

                labelOperatorsHeader.Visibility = Visibility.Visible;
                labelOperatorsHeader.Content = $"Оператор пикета: {selectedPicket.PicketID}";

                listViewOperators.ItemsSource = allOperators;
                listViewOperators.Visibility = Visibility.Visible;

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

            infoContractId.Visibility = Visibility.Collapsed;
            infoContractDate.Visibility = Visibility.Collapsed;
            infoContractServiceDescription.Visibility = Visibility.Collapsed;
            infoContractCustomerinfo.Visibility = Visibility.Collapsed;

            infoAreaID.Visibility = Visibility.Collapsed;
            infoAreaName.Visibility = Visibility.Collapsed;
            infoContractID.Visibility = Visibility.Collapsed;

            infoProfileID.Visibility = Visibility.Collapsed;
            infoProfileName.Visibility = Visibility.Collapsed;
            infoProfileType.Visibility = Visibility.Collapsed;
            infoArea.Visibility = Visibility.Collapsed;

            infoPicketID.Visibility = Visibility.Collapsed;
            infoCoordinate.Visibility = Visibility.Collapsed;
            infoChannel.Visibility = Visibility.Collapsed;
            infoProfile.Visibility = Visibility.Collapsed;
            infoGammaSpectrometer.Visibility = Visibility.Collapsed;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext is object picket)
            {
                dbHelper.SaveProject(picket);
                
            }
        }



    }
}
