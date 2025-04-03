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
using System.Collections.ObjectModel;
namespace Spectr
{
    /// <summary>
    /// Логика взаимодействия для AdministratorPage.xaml
    /// </summary>
    public partial class AdministratorPage : Page
    {
        Administrator Administrator;
        Db_Helper dbHelper;
        ObservableCollection<Area> areas = new ObservableCollection<Area>();
        ObservableCollection<Profile> profiles = new ObservableCollection<Profile>();
        ObservableCollection<Picket> pickets = new ObservableCollection<Picket>();
        ObservableCollection<Operator> operators = new ObservableCollection<Operator>();
        ObservableCollection<Analyst> analysts = new ObservableCollection<Analyst>();
        ObservableCollection<AreaCoordinates> areaCcoordinates = new ObservableCollection<AreaCoordinates>();
        ObservableCollection<ProfileCoordinates> profileCoordinates = new ObservableCollection<ProfileCoordinates>();

        public AdministratorPage(Administrator administrator)
        {
            Administrator = administrator;
            InitializeComponent();
            LabelLogin.Content = administrator.AdministratorLogin;
            dbHelper = new();
            dbHelper.LoadContract();
            treeView.ItemsSource = dbHelper.contracts;
            ResetVisibility();
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
       
            if (sender is Button button)
            {
                string tagValue = button.Tag as string;

               
                switch (tagValue) 
                {
                    case "Area":
                        Area newArea = new Area { AreaName = "Введите Имя" };
                        areas.Add(newArea); 
                        break;

                    case "Profile":
                        Profile newProfile = new Profile { ProfileType= "Ведите тип профиля" , ProfileName= "Введите имя профиля"};
                        profiles.Add(newProfile); // Используем свойство Profiles
                        break;

                    case "Picket":
                        Picket newPicket = new Picket { /* Заполните свойства Picket */ };
                        pickets.Add(newPicket);  // Используем свойство Pickets
                        break;

                    case "Operator":
                        Operator newOperator = new Operator { /* Заполните свойства Operator */ };
                        operators.Add(newOperator); // Используем свойство Operators
                        break;

                    case "Analyst":
                        Analyst newAnalyst = new Analyst { /* Заполните свойства Analyst */ };
                        analysts.Add(newAnalyst); // Используем свойство Analysts
                        break;

                    case "AreaCoordinates":
                        AreaCoordinates newAreaCoordinate = new AreaCoordinates { /* Заполните свойства AreaCoordinates */ };
                        areaCcoordinates.Add(newAreaCoordinate); // Используем свойство AreaCoordinatesCollection
                        break;

                    case "ProfileCoordinates":
                        ProfileCoordinates newProfileCoordinate = new ProfileCoordinates { /* Заполните свойства ProfileCoordinates */ };
                        profileCoordinates.Add(newProfileCoordinate); // Используем свойство ProfileCoordinatesCollection
                        break;

                    default:
                        MessageBox.Show("Неизвестный тип данных для добавления.");
                        break;
                }
            }
        }



        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (treeView.SelectedItem == null) return; 

            MessageBoxResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить этот элемент?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var itemToDelete = treeView.SelectedItem;
                dbHelper.DeleteProject(itemToDelete);
               
            }
        }

        private void BtnDeleteListview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
 
                var itemToDelete = button.DataContext;

                if (itemToDelete == null) return;

                MessageBoxResult result = MessageBox.Show(
                    "Вы уверены, что хотите удалить этот элемент?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                   
                    switch (itemToDelete)
                    {
                        case Area area:
                            dbHelper.DeleteProject(area); 
                            areas.Remove(area);
                            break;

                        case Profile profile:
                            dbHelper.DeleteProject(profile);
                            profiles.Remove(profile);
                            break;

                        case Picket picket:
                            dbHelper.DeleteProject(picket);
                            pickets.Remove(picket);
                            break;

                        case Operator @operator: // Используем @ перед operator, чтобы избежать конфликта с ключевым словом
                            dbHelper.DeleteProject(@operator);
                            operators.Remove(@operator);
                            break;

                        case Analyst analyst:
                            dbHelper.DeleteProject(analyst);
                            analysts.Remove(analyst);
                            break;

                        case AreaCoordinates areaCoordinate:
                            dbHelper.DeleteProject(areaCoordinate);
                            areaCcoordinates.Remove(areaCoordinate);
                            break;

                        case ProfileCoordinates profileCoordinate:
                            dbHelper.DeleteProject(profileCoordinate);
                            profileCoordinates.Remove(profileCoordinate);
                            break;

                        default:
                            MessageBox.Show("Неизвестный тип данных для удаления.");
                            break;
                    }
                }
            }
        }



        private void btnDeleteInTreeview_Click(object sender, RoutedEventArgs e)
        {



         

            MessageBoxResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить этот элемент?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
               
             
                if (sender is Button button)
                {

                    var itemToRemove = button.DataContext;

                    if (itemToRemove != null)
                    {
                        dbHelper.DeleteProject(itemToRemove);
                  
                    }
                }

             
            }
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
                this.DataContext = selectedContract;
                infoLabel.Content = $"Информация о контракте {selectedContract.ContractID}";
                areas = selectedContract.Areas;
                listViewArea.ItemsSource = areas;

                var analystsList = selectedContract.ContractAnalysts
      .Select(ca => ca.Analyst)
      .Distinct()
      .ToList();


                analysts.Clear(); 
                foreach (var analyst in selectedContract.ContractAnalysts.Select(ca => ca.Analyst).Distinct())
                {
                    analysts.Add(analyst);
                }
                listViewAnalysts.ItemsSource = analysts;

                List<Operator>list = selectedContract.Areas
                   .SelectMany(a => a.Profiles)
                   .SelectMany(p => p.Pickets)
                   .Select(pk => pk.Operator)
                   .Distinct()
                   .ToList();
                operators = CollectionExtensions.ToObservableCollection(list);
                listViewOperators.ItemsSource = operators;

             
                addAreaBtn.Visibility = Visibility.Visible;

                analystAddBtn.Visibility = Visibility.Visible;
                operatorAddBtn.Visibility = Visibility.Visible;
                

                infoContractId.Visibility = Visibility.Visible;
                infoContractIdLabel.Visibility = Visibility.Visible;
               // infoContractId.Text =  selectedContract.ContractID.ToString();

                infoContractDate.Visibility = Visibility.Visible;
                infoContractDate1.Visibility = Visibility.Visible;
                infoContractDateLabel.Visibility = Visibility.Visible;
                infoContractDateLabel1.Visibility = Visibility.Visible; 
            

                infoContractServiceDescription.Visibility = Visibility.Visible;
                infoContractServiceDescriptionLabel.Visibility = Visibility.Visible;


                infoContractCustomerinfo.Visibility = Visibility.Visible;
                infoContractCustomerinfo1.Visibility = Visibility.Visible;
                infoContractCustomerinfo2.Visibility = Visibility.Visible;
                infoContractCustomerinfo3.Visibility = Visibility.Visible;
                infoContractCustomerinfo4.Visibility = Visibility.Visible;
                infoContractCustomerinfoLabel.Visibility = Visibility.Visible;
                infoContractCustomerinfoLabel1.Visibility = Visibility.Visible;
                infoContractCustomerinfoLabel2.Visibility = Visibility.Visible;
                infoContractCustomerinfoLabel3.Visibility = Visibility.Visible;
                infoContractCustomerinfoLabel4.Visibility = Visibility.Visible;

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
                this.DataContext = selectedArea;
                infoLabel.Content = $"Информация о Площади {selectedArea.AreaName}, {selectedArea.AreaID}";
                profiles = selectedArea.Profiles;
                listViewProfile.ItemsSource = profiles;
                areaCcoordinates = selectedArea.AreaCoordinates;
                listViewAreaCoordinates.ItemsSource = areaCcoordinates;


                List<Operator> allOperators = selectedArea.Profiles
                   .SelectMany(p => p.Pickets)
                   .Select(pk => pk.Operator)
                   .Distinct()
                   .ToList();

                listViewOperators.ItemsSource = allOperators;

                
                addProfileBtn.Visibility = Visibility.Visible;

                areaCoordinateAddBtn.Visibility = Visibility.Visible;
            
                operatorAddBtn.Visibility = Visibility.Visible;
               

                infoAreaID.Visibility = Visibility.Visible;
                infoAreaIDLabel.Visibility = Visibility.Visible;

                infoAreaName.Visibility = Visibility.Visible;
                infoAreaNamedLabel.Visibility = Visibility.Visible;

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
                this.DataContext = selectedProfile;
                infoLabel.Content = $"Информация о Профиле {selectedProfile.ProfileName}, {selectedProfile.ProfileID}";
                pickets = selectedProfile.Pickets;
                listViewPickets.ItemsSource =pickets;
                selectedProfile.ProfileCoordinates = profileCoordinates;
                listViewProfileCoordinates.ItemsSource = profileCoordinates;

                List<Operator> allOperators = selectedProfile.Pickets
                   .Select(pk => pk.Operator)
                   .Distinct()
                   .ToList();
                operators = CollectionExtensions.ToObservableCollection(allOperators);
                listViewOperators.ItemsSource = operators;

                addPiketBtn.Visibility = Visibility.Visible;
               
          
                profileCoordinateAddBtn.Visibility = Visibility.Visible;
           
                operatorAddBtn.Visibility = Visibility.Visible;

                infoProfileID.Visibility = Visibility.Visible;
                infoProfileIDLabel.Visibility = Visibility.Visible;

                infoProfileName.Visibility = Visibility.Visible;
                infoProfileNameLabel.Visibility = Visibility.Visible;


                infoProfileType.Visibility = Visibility.Visible;
                infoProfileTypeLabel.Visibility = Visibility.Visible;

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
                this.DataContext = selectedPicket;
                Debug.WriteLine(selectedPicket.GammaSpectrometer.CommissioningDate.ToString());
                infoLabel.Content = $"Информация о Пикете {selectedPicket.PicketID}";

            
                
                operatorAddBtn.Visibility = Visibility.Visible;
            

                infoPicketID.Visibility = Visibility.Visible;
                infoPicketIDLabel.Visibility = Visibility.Visible;

                infoCoordinate1.Visibility = Visibility.Visible;
                infoCoordinate2.Visibility = Visibility.Visible;
                infoCoordinateLabel1.Visibility = Visibility.Visible;
                infoCoordinateLabel2.Visibility = Visibility.Visible;
                infoChannel1.Visibility = Visibility.Visible;
                infoChannel2.Visibility = Visibility.Visible;
                infoChannel3.Visibility = Visibility.Visible;
                infoChannelLabel1.Visibility = Visibility.Visible;
                infoChannelLabel2.Visibility = Visibility.Visible;
                infoChannelLabel3.Visibility = Visibility.Visible;


                infoGammaSpectrometerID.Visibility = Visibility.Visible;
                infoGammaSpectrometerIDLabel.Visibility = Visibility.Visible;
                infoGammaSpectrometerCommissioningDateLabel.Visibility = Visibility.Visible;
                infoGammaSpectrometerCommissioningDate.Visibility = Visibility.Visible;
                infoGammaSpectrometerDecommissioningDateLabel.Visibility = Visibility.Visible;
                infoGammaSpectrometerDecommissioningDate.Visibility = Visibility.Visible;
                infoGammaSpectrometerMeasurementAccuracyLabel.Visibility = Visibility.Visible;
                infoGammaSpectrometerMeasurementAccuracyDate.Visibility = Visibility.Visible;
                infoGammaSpectrometerMeasurementTimeLabel.Visibility = Visibility.Visible;
                infoGammaSpectrometerMeasurementTime.Visibility = Visibility.Visible;
               
              
               





                List<Operator> allOperators = new List<Operator> { selectedPicket.Operator };
                operators = CollectionExtensions.ToObservableCollection(allOperators);
                labelOperatorsHeader.Visibility = Visibility.Visible;
                labelOperatorsHeader.Content = $"Оператор пикета: {selectedPicket.PicketID}";

                listViewOperators.ItemsSource = operators;
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
            infoContractDate1.Visibility = Visibility.Collapsed;
            infoContractServiceDescription.Visibility = Visibility.Collapsed;
            infoContractCustomerinfo.Visibility = Visibility.Collapsed;

            infoAreaID.Visibility = Visibility.Collapsed;
            infoAreaName.Visibility = Visibility.Collapsed;


            infoProfileID.Visibility = Visibility.Collapsed;
            infoProfileName.Visibility = Visibility.Collapsed;
            infoProfileType.Visibility = Visibility.Collapsed;

            infoPicketID.Visibility = Visibility.Collapsed;
            infoCoordinate1.Visibility = Visibility.Collapsed;
            infoCoordinate2.Visibility = Visibility.Collapsed;
            infoCoordinateLabel1.Visibility = Visibility.Collapsed;
            infoCoordinateLabel2.Visibility = Visibility.Collapsed;



            infoChannel1.Visibility = Visibility.Collapsed;
            infoChannel2.Visibility = Visibility.Collapsed;
            infoChannel3.Visibility = Visibility.Collapsed;

            infoChannelLabel1.Visibility = Visibility.Collapsed;
            infoChannelLabel2.Visibility = Visibility.Collapsed;
            infoChannelLabel3.Visibility = Visibility.Collapsed;


           

            infoContractIdLabel.Visibility = Visibility.Collapsed;
            infoContractDateLabel.Visibility = Visibility.Collapsed;
            infoContractDateLabel1.Visibility = Visibility.Collapsed;
            infoContractCustomerinfoLabel.Visibility = Visibility.Collapsed;
            infoAreaIDLabel.Visibility = Visibility.Collapsed;
            infoAreaNamedLabel.Visibility = Visibility.Collapsed;
         
            infoProfileIDLabel.Visibility = Visibility.Collapsed;
            infoProfileNameLabel.Visibility = Visibility.Collapsed;
            infoProfileTypeLabel.Visibility = Visibility.Collapsed;
          
            infoPicketIDLabel.Visibility = Visibility.Collapsed;
            infoCoordinateLabel1.Visibility = Visibility.Collapsed;
            infoCoordinateLabel2.Visibility = Visibility.Collapsed;
            infoCoordinate1.Visibility = Visibility.Collapsed;
            infoCoordinate2.Visibility = Visibility.Collapsed;

          
        
            infoContractServiceDescriptionLabel.Visibility = Visibility.Collapsed;

            infoContractCustomerinfo.Visibility = Visibility.Collapsed;
            infoContractCustomerinfo1.Visibility = Visibility.Collapsed;
            infoContractCustomerinfo2.Visibility = Visibility.Collapsed;
            infoContractCustomerinfo3.Visibility = Visibility.Collapsed;
            infoContractCustomerinfo4.Visibility = Visibility.Collapsed;

            infoContractCustomerinfoLabel1.Visibility = Visibility.Collapsed;
            infoContractCustomerinfoLabel2.Visibility = Visibility.Collapsed;
            infoContractCustomerinfoLabel3.Visibility = Visibility.Collapsed;
            infoContractCustomerinfoLabel4.Visibility = Visibility.Collapsed;

            infoGammaSpectrometerID.Visibility = Visibility.Collapsed;
            infoGammaSpectrometerIDLabel.Visibility = Visibility.Collapsed;
            infoGammaSpectrometerCommissioningDateLabel.Visibility = Visibility.Collapsed;
            infoGammaSpectrometerCommissioningDate.Visibility = Visibility.Collapsed;
            infoGammaSpectrometerDecommissioningDateLabel.Visibility= Visibility.Collapsed;
            infoGammaSpectrometerDecommissioningDate.Visibility= Visibility.Collapsed;
            infoGammaSpectrometerMeasurementAccuracyLabel.Visibility = Visibility.Collapsed;
            infoGammaSpectrometerMeasurementAccuracyDate.Visibility= Visibility.Collapsed;
            infoGammaSpectrometerMeasurementTimeLabel.Visibility = Visibility.Collapsed;
            infoGammaSpectrometerMeasurementTime.Visibility= Visibility.Collapsed;

            addPiketBtn.Visibility = Visibility.Collapsed;
            addProfileBtn.Visibility = Visibility.Collapsed;
            addAreaBtn.Visibility = Visibility.Collapsed;
            profileCoordinateAddBtn.Visibility = Visibility.Collapsed;
            analystAddBtn.Visibility = Visibility.Collapsed;
            operatorAddBtn.Visibility = Visibility.Collapsed;
            areaCoordinateAddBtn.Visibility = Visibility.Collapsed;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("here2");
            if (sender is TextBox textBox && textBox.DataContext is object project)
            {
                Debug.WriteLine("here");
                dbHelper.SaveProject(project);
                
            }
        }




    }
}
