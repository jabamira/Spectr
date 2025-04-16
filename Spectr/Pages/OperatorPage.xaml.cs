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
        Operator Operator;
        Db_Helper dbHelper;
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
            Operator = _operator;
            InitializeComponent();

            LabelLogin.Content = Operator.OperatorLogin;
            dbHelper = new();
            dbHelper.LoadProfilesForOperator(Operator.OperatorID);



            treeView.ItemsSource = dbHelper.profiles;
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
                dbHelper.SaveProject(project);

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

            if (treeView.SelectedItem is Contract selectedContract)
            {
                dbHelper.LoadAnalyst();

                this.DataContext = selectedContract;
                infoLabel.Content = $"Информация о контракте {selectedContract.ContractID}";
                areas = selectedContract.Areas;
                listViewArea.ItemsSource = areas;

                var analystsList = selectedContract.ContractAnalysts
                  .Select(ca => ca.Analyst)
                  .Distinct()
                  .ToList();

     
                analystsContract.Clear();
                foreach (var analyst in selectedContract.ContractAnalysts.Select(ca => ca.Analyst).Distinct())
                {
                    analystsContract.Add(analyst);
                }
                listViewAnalystsContract.ItemsSource = analystsContract;

                labelAnalystContract.Visibility = Visibility.Visible;
                listViewAnalystsContract.Visibility = Visibility.Visible;

        





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



                labelAreaHeader.Visibility = Visibility.Visible;
                labelAreaHeader.Content = $"Площади Контракта: {selectedContract.ContractID}";

                listViewArea.Visibility = Visibility.Visible;

                listViewAnalystsContract.Items.Refresh();



                listViewArea.Items.Refresh();

            }
            else if (treeView.SelectedItem is Area selectedArea)
            {
                this.DataContext = selectedArea;
                infoLabel.Content = $"Информация о Площади {selectedArea.AreaName}, {selectedArea.AreaID}";
                profiles = selectedArea.Profiles;
                listViewProfile.ItemsSource = profiles;
                areaCcoordinates = selectedArea.AreaCoordinates;
                listViewAreaCoordinates.ItemsSource = areaCcoordinates;









                infoAreaID.Visibility = Visibility.Visible;
                infoAreaIDLabel.Visibility = Visibility.Visible;

                infoAreaName.Visibility = Visibility.Visible;
                infoAreaNamedLabel.Visibility = Visibility.Visible;

                labelProfilesHeader.Visibility = Visibility.Visible;
                labelProfilesHeader.Content = $"Профили Площади: {selectedArea.AreaName}, {selectedArea.AreaID}";

                labelAreaCoordinatesHeader.Visibility = Visibility.Visible;
                labelAreaCoordinatesHeader.Content = $"Координаты Площади: {selectedArea.AreaName}, {selectedArea.AreaID}";



                listViewProfile.Visibility = Visibility.Visible;
                listViewAreaCoordinates.Visibility = Visibility.Visible;


                listViewProfile.Items.Refresh();
                listViewAreaCoordinates.Items.Refresh();

            }
            else if (treeView.SelectedItem is Profile selectedProfile)
            {
                this.DataContext = selectedProfile;
                infoLabel.Content = $"Информация о Профиле {selectedProfile.ProfileName}, {selectedProfile.ProfileID}";
                pickets = selectedProfile.Pickets;
                listViewPickets.ItemsSource = pickets;
                profileCoordinates = selectedProfile.ProfileCoordinates;
                listViewProfileCoordinates.ItemsSource = profileCoordinates;

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

                listViewAnalystsContract.ItemsSource = dbHelper.analysts;

                listViewOperatorsProfile.ItemsSource = operatorsProfile;
                dbHelper.LoadOperators();
  

                labelPicketsHeader.Content = $"Пикеты Профиля: {selectedProfile.ProfileName}, {selectedProfile.ProfileID}";

                labelProfilesHeader.Visibility = Visibility.Visible;
                labelProfilesHeader.Content = $"Координаты Профиля: {selectedProfile.ProfileName}, {selectedProfile.ProfileID}";




                listViewPickets.Visibility = Visibility.Visible;
                listViewProfileCoordinates.Visibility = Visibility.Visible;
      
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
                listViewProfileCoordinates.Items.Refresh();
       
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

    }
}
