using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Spectr.Data;
using Spectr.Db;

namespace Spectr.Pages
{
    /// <summary>
    /// Interaction logic for OperatorPage.xaml
    /// </summary>
    public partial class OperatorPage : Page
    {
        Operator _Operator;

        ObservableCollection<Area> areas = new ObservableCollection<Area>();
        ObservableCollection<Profile> profiles = new ObservableCollection<Profile>();
        ObservableCollection<Picket> pickets = new ObservableCollection<Picket>();


        ObservableCollection<AreaCoordinates> areaCcoordinates = new ObservableCollection<AreaCoordinates>();
        ObservableCollection<ProfileCoordinates> profileCoordinates = new ObservableCollection<ProfileCoordinates>();

        public ObservableCollection<Operator> operatorsProfile = new ObservableCollection<Operator>();
        ObservableCollection<Operator> operatorsNew = new ObservableCollection<Operator>();
        ObservableCollection<Operator> operatorsDelete = new ObservableCollection<Operator>();
        ObservableCollection<Analyst> analystsContract = new ObservableCollection<Analyst>();
        ObservableCollection<Analyst> analystsNew = new ObservableCollection<Analyst>();
        ObservableCollection<Analyst> analystsDelete = new ObservableCollection<Analyst>();
        public OperatorPage(Operator _operator )
        {
            _Operator = _operator;
            InitializeComponent();

            LabelLogin.Content = _Operator.OperatorLogin;

            Db_Helper.LoadProfilesForOperator(_Operator.OperatorID);

            Db_Helper.LoadSpectrometrs();

            treeView.ItemsSource = Db_Helper.profiles;
            ResetVisibility();
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.NavigationService.Navigate(new AuthPage());
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("here2");
            if (sender is TextBox textBox && textBox.DataContext is object project)
            {
                Debug.WriteLine("here");
                Db_Helper.SaveProject(project);

            }
        }
        private void ResetVisibility()

        {
            labelAnalystContract.Visibility = Visibility.Collapsed;
            listViewAnalystsContract.Visibility = Visibility.Collapsed;

            listViewOperatorsProfile.Visibility = Visibility.Collapsed;
            labelOperatorsProfile.Visibility = Visibility.Collapsed;
            infoLabel.Visibility = Visibility.Visible;
            labelOperatorsHeader.Visibility = Visibility.Collapsed;
  
            listViewOperators.Visibility = Visibility.Collapsed;

            listViewArea.Visibility = Visibility.Collapsed;
            listViewPickets.Visibility = Visibility.Collapsed;
            listViewProfile.Visibility = Visibility.Collapsed;
            listViewAnalysts.Visibility = Visibility.Collapsed;
    
            listViewAreaCoordinates.Visibility = Visibility.Collapsed;

            labelPicketsHeader.Visibility = Visibility.Collapsed;
            labelProfilesHeader.Visibility = Visibility.Collapsed;
            labelAreaHeader.Visibility = Visibility.Collapsed;
 
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
            infoGammaSpectrometerDecommissioningDateLabel.Visibility = Visibility.Collapsed;
            infoGammaSpectrometerDecommissioningDate.Visibility = Visibility.Collapsed;
            infoGammaSpectrometerMeasurementAccuracyLabel.Visibility = Visibility.Collapsed;
            infoGammaSpectrometerMeasurementAccuracyDate.Visibility = Visibility.Collapsed;
            infoGammaSpectrometerMeasurementTimeLabel.Visibility = Visibility.Collapsed;
            infoGammaSpectrometerMeasurementTime.Visibility = Visibility.Collapsed;


        }
        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ResetVisibility();

           
        
             if (treeView.SelectedItem is Profile selectedProfile)
            {
                this.DataContext = selectedProfile;
                infoLabel.Content = $"Информация о Профиле {selectedProfile.ProfileName}, {selectedProfile.ProfileID}";
                pickets = selectedProfile.Pickets.Where(op => op.OperatorID == _Operator.OperatorID).ToObservableCollection();
                listViewPickets.ItemsSource = pickets;
                profileCoordinates = selectedProfile.ProfileCoordinates;
              

                operatorsProfile.Clear();

                if (selectedProfile.ProfileOperators != null)
                {
                    foreach (var @operator in selectedProfile.ProfileOperators
                                                            .Where(ca => ca.Operator != null)
                                                            .Select(ca => ca.Operator)
                                                            .Distinct())
                    {
                        operatorsProfile.Add(@operator);
                    }
                }

                listViewAnalystsContract.ItemsSource = Db_Helper.analysts;

                listViewOperatorsProfile.ItemsSource = operatorsProfile;
                Db_Helper.LoadOperators();
  

                labelPicketsHeader.Content = $"Пикеты Профиля: {selectedProfile.ProfileName}, {selectedProfile.ProfileID}";

             



                listViewPickets.Visibility = Visibility.Visible;
            
      
                listViewOperatorsProfile.Visibility = Visibility.Visible;
                labelOperatorsProfile.Visibility = Visibility.Visible;







                infoProfileID.Visibility = Visibility.Visible;
                infoProfileIDLabel.Visibility = Visibility.Visible;

                infoProfileName.Visibility = Visibility.Visible;
                infoProfileNameLabel.Visibility = Visibility.Visible;


                infoProfileType.Visibility = Visibility.Visible;
                infoProfileTypeLabel.Visibility = Visibility.Visible;

                labelPicketsHeader.Visibility = Visibility.Visible;
                listViewPickets.Items.Refresh();
                listViewOperatorsProfile.Items.Refresh();
           
       
            }
            else if (treeView.SelectedItem is Picket selectedPicket)
            {
                this.DataContext = selectedPicket;

                infoLabel.Content = $"Информация о Пикете {selectedPicket.PicketID}";



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










                listViewOperators.Items.Refresh();
            }

            Debug.WriteLine(1);
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
                            Db_Helper.DeleteProject(area);
                            areas.Remove(area);
                            break;

                        case Profile profile:
                            Db_Helper.DeleteProject(profile);
                            profiles.Remove(profile);
                            break;

                        case Picket picket:
                            Db_Helper.DeleteProject(picket);
                            pickets.Remove(picket);
                            break;

                        case Operator @operator: // Используем @ перед operator, чтобы избежать конфликта с ключевым словом
                            Profile _profile = (Profile)infoProfileID.DataContext;
                            Db_Helper.DeleteProfileOperator(_profile.ProfileID, @operator.OperatorID);
                            operatorsProfile.Remove(@operator);
                            break;

                        case Analyst analyst:
                            Contract contract = (Contract)infoContractIdLabel.DataContext;
                            Db_Helper.DeleteContractAnalyst(contract.ContractID, analyst.AnalystID);
                            analystsContract.Remove(analyst);
                            break;

                        case AreaCoordinates areaCoordinate:
                            Db_Helper.DeleteProject(areaCoordinate);
                            areaCcoordinates.Remove(areaCoordinate);
                            break;

                        case ProfileCoordinates profileCoordinate:
                            Db_Helper.DeleteProject(profileCoordinate);
                            profileCoordinates.Remove(profileCoordinate);
                            break;
                        case GammaSpectrometer gammaSpectrometer:
                            Db_Helper.DeleteProject(gammaSpectrometer);
                            Db_Helper.gammaSpectrometers.Remove(gammaSpectrometer);
                            break;

                        default:
                            MessageBox.Show("Неизвестный тип данных для удаления.");
                            break;
                    }
                }
            }
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

            if (sender is Button button)
            {
                string tagValue = button.Tag as string;


                switch (tagValue)
                {
                  

                    case "Picket":
                        if (infoProfileID.DataContext is Profile profile)
                        {
                            if (pickets == null)
                            {
                                pickets = new ObservableCollection<Picket>();
                                listViewPickets.ItemsSource = pickets;

                            }

                            Picket newPicket = new Picket
                            {
                                CoordinateX = 0f,
                                CoordinateY = 0f,
                                Channel1 = 0,
                                Channel2 = 0,
                                Channel3 = 0,
                          
                                Profile = profile,
                                

                              
                                OperatorID = _Operator.OperatorID,

                                GammaSpectrometer = Db_Helper.gammaSpectrometers[0],

                            };


                            pickets.Add(newPicket);

                            listViewPickets.Items.Refresh();
                        }
                        else
                        {
                            MessageBox.Show("Выберите профиль перед добавлением пикета.");
                        }
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
    }
}
