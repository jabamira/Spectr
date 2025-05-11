using Microsoft.EntityFrameworkCore;
using OxyPlot;
using OxyPlot.Wpf;
using Spectr.Data;
using Spectr.Db;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Contract = Spectr.Data.Contract;
namespace Spectr
{
    /// <summary>
    /// Логика взаимодействия для AdministratorPage.xaml
    /// </summary>
    public partial class AdministratorPage : Page
    {
        Administrator Administrator;

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

        public AdministratorPage(Administrator administrator)
        {
            Administrator = administrator;
            InitializeComponent();
            LabelLogin.Content = administrator.AdministratorLogin;

            Db_Helper.LoadContract();
            Db_Helper.LoadSpectrometrs();


            treeView.ItemsSource = Db_Helper.contracts;
            ResetVisibility();
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
                        Profile newProfile = new Profile { ProfileType = "Ведите тип профиля", ProfileName = "Введите имя профиля" , Area = (Area)infoAreaID.DataContext };
                        if (profiles == null)
                        {
                            profiles = new ObservableCollection<Profile>();
                            listViewProfile.ItemsSource = profiles;
                            Debug.WriteLine(000);
                        }
              
     
                        profiles.Add(newProfile);
          
                     
                        Debug.WriteLine(profiles[0].ProfileName);
                        listViewProfile.Items.Refresh();
                        break;

                    case "Picket":
                       
                        if (infoProfileID.DataContext is Profile profile)
                        {
                            if ((profile.ProfileOperators?.FirstOrDefault()?.OperatorID ?? 1) == 1)
                            {
                                MessageBox.Show("Доабавьте сначала оператора ");
                            }
                            else 
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
                                    ProfileID = profile.ProfileID,
                                    Profile = profile,

                                    OperatorID = profile.ProfileOperators?.FirstOrDefault()?.OperatorID ?? 1,


                                    GammaSpectrometer = Db_Helper.gammaSpectrometers[0],

                                };


                                pickets.Add(newPicket);

                            }


                        }
                        else
                        {
                            MessageBox.Show("Выберите профиль перед добавлением пикета.");
                        }
                        break;


                    case "OperatorAdd":
                        Operator newOperator = new Operator
                        {
                            FullName = "Введите ФИО",
                            PhoneNumber = "Телефонный номер",
                            Email = "Введиет эл почту",
                            JobTitle = "ВЫполняемая работа",
                            OperatorPassword = "Введите пароль",
                            OperatorLogin = "Введите логин"
                        };
                        Db_Helper.operators.Add(newOperator);
                        break;

                    case "Analyst":
                        Analyst newAnalyst = new Analyst
                        {
                            FullName = "Введите Фио",
                            PhoneNumber = "Телефонный номер",
                            Email = "Введиет эл почту",
                            JobTitle = "ВЫполняемая работа",
                            AnalystLogin = "Login",
                            AnalystPassword = Guid.NewGuid().ToString(),
                            ContractAnalysts = new ObservableCollection<ContractAnalyst>()
                        };
                        Db_Helper.analysts.Add(newAnalyst);
                        break;

                    case "AreaCoordinates":
                        if (areaCcoordinates == null)
                            areaCcoordinates = new ObservableCollection<AreaCoordinates>();
                        listViewAreaCoordinates.ItemsSource = areaCcoordinates;

                        AreaCoordinates newAreaCoordinate = new AreaCoordinates { Area = (Area)infoAreaID.DataContext };
                        areaCcoordinates.Add(newAreaCoordinate);
                        break;

                    case "ProfileCoordinates":
                        ProfileCoordinates newProfileCoordinate = new ProfileCoordinates { Profile = (Profile)infoProfileID.DataContext};
                        if (profileCoordinates == null)
                            profileCoordinates = new ObservableCollection<ProfileCoordinates>();
                        listViewProfileCoordinates.ItemsSource = profileCoordinates;

                        profileCoordinates.Add(newProfileCoordinate); 
                        break;

                    default:
                        MessageBox.Show("Неизвестный тип данных для добавления.");
                        break;
                }
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

        private void BtnManagmentWorkers_Click(object sender, RoutedEventArgs e)
        {
            Db_Helper.LoadOperators();
            Db_Helper.LoadAnalyst();
            ResetVisibility();
            labelOperatorsHeader.Visibility = Visibility.Visible;
            operatorSaveBtn.Visibility = Visibility.Visible;
            infoLabel.Visibility = Visibility.Collapsed;
            listViewOperators.Visibility = Visibility.Visible;
            operatorAddBtn.Visibility = Visibility.Visible;
            labelAnalystHeader.Visibility = Visibility.Visible;
            listViewAnalysts.Visibility = Visibility.Visible;
            analystAddBtn.Visibility = Visibility.Visible;

            listViewOperators.ItemsSource = Db_Helper.operators;
            listViewAnalysts.ItemsSource = Db_Helper.analysts;
        }

        private void SaveChangeManagment(object sender, RoutedEventArgs e)
        {
            foreach (var op in operatorsNew)
            {
                Db_Helper.SaveProject(op);
            }
            foreach (var op in operatorsDelete)
            {
                Db_Helper.DeleteProject(op);
            }
            foreach (var an in analystsNew)
            {
                Db_Helper.SaveProject(an);
            }
            foreach (var an in analystsDelete)
            {
                Db_Helper.DeleteProject(an);
            }
            MessageBox.Show("Все Изменения успешно сохранены.");
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
                        Db_Helper.DeleteProject(itemToRemove);

                    }
                }


            }
        }
        private void BtnDeleteOperator_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                Operator @operator = (Operator)button.DataContext;
                operatorsDelete.Add(@operator);
                Db_Helper.operators.Remove(@operator);
            }

        }
        private void BtnDeleteContract_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                Analyst contraa = (Analyst)button.DataContext;
                analystsDelete.Add(contraa);
                Db_Helper.analysts.Remove(contraa);
            }

        }


        private void BtnGammaSpectr_Click(object sender, RoutedEventArgs e)
        {
         
           ResetVisibility();
            infoLabel.Visibility = Visibility.Collapsed;
            Db_Helper.LoadSpectrometrs();
            listViewSpectrometrs.ItemsSource = Db_Helper.gammaSpectrometers;
            listViewSpectrometrs.Visibility = Visibility.Visible;
            labelSpectrometrsHeader.Visibility = Visibility.Visible;



        }

        private void BtnGraphik_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSchem_Click(object sender, RoutedEventArgs e)
        {
          
            Contract contract = treeView.SelectedItem as Contract;
            if (contract != null)
            {
                ResetVisibility();
                MyPlotView.Visibility = Visibility.Visible;
                this.MyPlotView.Model = Graphic.GenerateSchem(contract);
                this.MyPlotView.InvalidatePlot(true);
            }
            else
            {
                MessageBox.Show("Выберите Контракт");
            }
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {

        }



        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {

        }



        private void treeView_SelectedItemChanged_1(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ResetVisibility();

            if (treeView.SelectedItem is Contract selectedContract)
            {
                Db_Helper.LoadAnalyst();

                this.DataContext = selectedContract;
                infoLabel.Content = $"Информация о контракте {selectedContract.ContractID}";
                areas = selectedContract.Areas;
                listViewArea.ItemsSource = areas;

                var analystsList = selectedContract.ContractAnalysts
                  .Select(ca => ca.Analyst)
                  .Distinct()
                  .ToList();

                listViewAnalystsAll.ItemsSource = Db_Helper.analysts;
                analystsContract.Clear();
                foreach (var analyst in selectedContract.ContractAnalysts.Select(ca => ca.Analyst).Distinct())
                {
                    analystsContract.Add(analyst);
                }
                listViewAnalystsContract.ItemsSource = analystsContract;

                labelAnalystContract.Visibility = Visibility.Visible;
                listViewAnalystsContract.Visibility = Visibility.Visible;

                labelAnalystAll.Visibility = Visibility.Visible;
                listViewAnalystsAll.Visibility = Visibility.Visible;
                addAreaBtn.Visibility = Visibility.Visible;





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





                addProfileBtn.Visibility = Visibility.Visible;

                areaCoordinateAddBtn.Visibility = Visibility.Visible;




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

                listViewAnalystsContract.ItemsSource = Db_Helper.analysts;

                listViewOperatorsProfile.ItemsSource = operatorsProfile;
                Db_Helper.LoadOperators();
                listViewOperatorsAdd.ItemsSource = Db_Helper.operators;

                labelPicketsHeader.Content = $"Пикеты Профиля: {selectedProfile.ProfileName}, {selectedProfile.ProfileID}";

                labelProfilesHeader.Visibility = Visibility.Visible;
                labelProfilesHeader.Content = $"Координаты Профиля: {selectedProfile.ProfileName}, {selectedProfile.ProfileID}";




                listViewPickets.Visibility = Visibility.Visible;
                listViewProfileCoordinates.Visibility = Visibility.Visible;
                listViewOperatorsAdd.Visibility = Visibility.Visible;
                listViewOperatorsProfile.Visibility = Visibility.Visible;
                labelOperatorsProfile.Visibility = Visibility.Visible;
                labelOperatorsAdd.Visibility = Visibility.Visible;
                addPiketBtn.Visibility = Visibility.Visible;


                profileCoordinateAddBtn.Visibility = Visibility.Visible;



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
                listViewOperatorsAdd.Items.Refresh();
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
        private void ResetVisibility()

        {
            MyPlotView.Visibility = Visibility.Collapsed;
            labelSpectrometrsHeader.Visibility = Visibility.Collapsed;
            listViewSpectrometrs.Visibility = Visibility.Collapsed;
            labelAnalystContract.Visibility = Visibility.Collapsed;
            listViewAnalystsContract.Visibility = Visibility.Collapsed;
            labelAnalystAll.Visibility = Visibility.Collapsed;
            listViewAnalystsAll.Visibility = Visibility.Collapsed;
            listViewOperatorsAdd.Visibility = Visibility.Collapsed;
            labelOperatorsAdd.Visibility = Visibility.Collapsed;
            listViewOperatorsProfile.Visibility = Visibility.Collapsed;
            labelOperatorsProfile.Visibility = Visibility.Collapsed;
            infoLabel.Visibility = Visibility.Visible;
            labelOperatorsHeader.Visibility = Visibility.Collapsed;
            operatorSaveBtn.Visibility = Visibility.Collapsed;
            listViewOperators.Visibility = Visibility.Collapsed;
            operatorAddBtn.Visibility = Visibility.Collapsed;
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
                Db_Helper.SaveProject(project);

            }
        }
        private void OperatorWasEdited(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext is Operator _operator)
            {
                operatorsNew.Add(_operator);

            }
        }
        private void AnalystWasEdited(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext is Analyst analyst)
            {
                analystsNew.Add(analyst);

            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.NavigationService.Navigate(new AuthPage());
            }
        }

        private void BtnAddOperatorListview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Operator _operator)
            {

                Profile profile = (Profile)infoProfileID.DataContext;


                Db_Helper.AddProfileOperator(profile.ProfileID, _operator.OperatorID);


                bool alreadyExists = operatorsProfile.Any(op => op.OperatorID == _operator.OperatorID);

                if (!alreadyExists)
                {
                    operatorsProfile.Add(_operator);
                }
                else
                {
                    MessageBox.Show("Оператор уже добавлен!");
                }

            }

        }
        private void BtnAddAnalystListview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Analyst analyst)
            {

                Contract contract = (Contract)infoContractIdLabel.DataContext;


                Db_Helper.AddContractAnalyst(contract.ContractID, analyst.AnalystID);


                bool alreadyExists = analystsContract.Any(op => op.AnalystID == analyst.AnalystID);

                if (!alreadyExists)
                {
                    analystsContract.Add(analyst);
                }
                else
                {
                    MessageBox.Show("Аналитик уже добавлен!");
                }

            }

        }

        private void BtnOperatorOnPicketListview_Click(object sender, RoutedEventArgs e)
        {
            Picket picket = (Picket)listViewPickets.SelectedItem;
            if (picket != null)
            {
                if (sender is Button btn && btn.DataContext is Operator _operator)
                {
                    picket.Operator = _operator;
                }
                Db_Helper.SaveProject(picket);
                listViewPickets.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Выберите Пикет");
            }


        }

        private async void ResetDatabaset_Click(object sender, RoutedEventArgs e)
        {
            await Db_Helper.SeedDatabaseAsync(Db_Helper.context);
            Db_Helper.LoadContract(); // Загружаем только после завершения вставки
            treeView.ItemsSource = Db_Helper.contracts;
            Debug.WriteLine("DB reset");
        }


    }
}