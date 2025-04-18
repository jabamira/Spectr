using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spectr.Data
{
    public enum UserType
    {
        Администратор, Оператор, Заказчик, Аналитик
    }
    [Index(nameof(AdministratorLogin), IsUnique = true)]
    public class Administrator
    {
        [Key]
        public int AdministratorID { get; set; }

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string JobTitle { get; set; }

        [Required]
        [MaxLength(20)]
        public string AdministratorLogin { get; set; }

        [Required]
        [MaxLength(36)]
        public string AdministratorPassword { get; set; }

        public ObservableCollection<Contract> Contracts { get; set; }
    }
    [Index(nameof(CustomerLogin), IsUnique = true)]

    public class Customer : INotifyPropertyChanged
    {
        private int _customerID;
        private string _companyName;
        private string _phoneNumber;
        private string _contactPerson;
        private string _email;
        private string _address;
        private string _customerLogin;
        private string _customerPassword;


        [Key]
        public int CustomerID
        {
            get { return _customerID; }
            set
            {
                if (_customerID != value)
                {
                    _customerID = value;
                    OnPropertyChanged(nameof(CustomerID));
                }
            }
        }

        [Required]
        [MaxLength(255)]
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                if (_companyName != value)
                {
                    _companyName = value;
                    OnPropertyChanged(nameof(CompanyName));
                }
            }
        }

        [Required]
        [MaxLength(50)]
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        [Required]
        [MaxLength(255)]
        public string ContactPerson
        {
            get { return _contactPerson; }
            set
            {
                if (_contactPerson != value)
                {
                    _contactPerson = value;
                    OnPropertyChanged(nameof(ContactPerson));
                }
            }
        }

        [MaxLength(255)]
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        [Required]
        [MaxLength(255)]
        public string Address
        {
            get { return _address; }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        [Required]
        [MaxLength(50)]
        public string CustomerLogin
        {
            get { return _customerLogin; }
            set
            {
                if (_customerLogin != value)
                {
                    _customerLogin = value;
                    OnPropertyChanged(nameof(CustomerLogin));
                }
            }
        }

        [Required]
        [MaxLength(36)]
        public string CustomerPassword
        {
            get { return _customerPassword; }
            set
            {
                if (_customerPassword != value)
                {
                    _customerPassword = value;
                    OnPropertyChanged(nameof(CustomerPassword));
                }
            }
        }

        public ObservableCollection<Contract> Contracts { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    public class Contract : INotifyPropertyChanged
    {

        private DateTime _startDate;
        private DateTime _endDate;
        private string _serviceDescription;

        [Key]
        public int ContractID { get; set; }


        [Required]
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        [Required]
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        [Required]
        public string ServiceDescription
        {
            get { return _serviceDescription; }
            set
            {
                if (_serviceDescription != value)
                {
                    _serviceDescription = value;
                    OnPropertyChanged(nameof(ServiceDescription));
                }
            }
        }

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Administrator")]
        public int AdministratorID { get; set; }
        public Administrator Administrator { get; set; }

        public ObservableCollection<Area> Areas { get; set; }

        public ObservableCollection<ContractAnalyst> ContractAnalysts { get; set; }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Area : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        [Key]
        public int AreaID { get; set; }

        private string _areaName;
        [Required]
        [MaxLength(255)]
        public string AreaName
        {
            get => _areaName;
            set
            {
                _areaName = value;
                OnPropertyChanged(nameof(AreaName));
            }
        }

        [ForeignKey("Contract")]
        public int ContractID { get; set; }
        public Contract Contract { get; set; }

        public ObservableCollection<AreaCoordinates> AreaCoordinates { get; set; }
        public ObservableCollection<Profile> Profiles { get; set; }
    }

    public class AreaCoordinates : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        [Key]
        public int AreaCoordinatesID { get; set; }

        private float _x;
        [Required]
        public float X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        private float _y;
        [Required]
        public float Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        [ForeignKey("Area")]
        public int AreaID { get; set; }
        public Area Area { get; set; }
    }

    public class Profile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        [Key]
        public int ProfileID { get; set; }

        private string _profileName;
        [Required]
        [MaxLength(255)]
        public string ProfileName
        {
            get => _profileName;
            set
            {
                _profileName = value;
                OnPropertyChanged(nameof(ProfileName));
            }
        }

        private string _profileType;
        [Required]
        [MaxLength(100)]
        public string ProfileType
        {
            get => _profileType;
            set
            {
                _profileType = value;
                OnPropertyChanged(nameof(ProfileType));
            }
        }

        [ForeignKey("Area")]
        public int AreaID { get; set; }
        public Area Area { get; set; }

        public ObservableCollection<ProfileCoordinates> ProfileCoordinates { get; set; }
        public ObservableCollection<ProfileOperator> ProfileOperators { get; set; }
        public ObservableCollection<Picket> Pickets { get; set; }
    }

    public class ProfileCoordinates : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        [Key]
        public int ProfileCoordinatesID { get; set; }

        private float _x;
        [Required]
        public float X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        private float _y;
        [Required]
        public float Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        [ForeignKey("Profile")]
        public int ProfileID { get; set; }
        public Profile Profile { get; set; }
    }
    [Index(nameof(OperatorLogin), IsUnique = true)]
    public class Operator : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        private string _fullName;
        private string _phoneNumber;
        private string _email;
        private string _jobTitle;
        private string _operatorLogin;
        private string _operatorPassword;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OperatorID { get; set; }


        [Required]
        [MaxLength(255)]
        public string FullName
        {
            get => _fullName;
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        [Required]
        [MaxLength(50)]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        [Required]
        [MaxLength(255)]
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        [Required]
        [MaxLength(100)]
        public string JobTitle
        {
            get => _jobTitle;
            set
            {
                if (_jobTitle != value)
                {
                    _jobTitle = value;
                    OnPropertyChanged(nameof(JobTitle));
                }
            }
        }

        [Required]
        [MaxLength(50)]
        public string OperatorLogin
        {
            get => _operatorLogin;
            set
            {
                if (_operatorLogin != value)
                {
                    _operatorLogin = value;
                    OnPropertyChanged(nameof(OperatorLogin));
                }
            }
        }

        [Required]
        [MaxLength(36)]
        public string OperatorPassword
        {
            get => _operatorPassword;
            set
            {
                if (_operatorPassword != value)
                {
                    _operatorPassword = value;
                    OnPropertyChanged(nameof(OperatorPassword));
                }
            }
        }

        public ObservableCollection<Picket> Pickets { get; set; }
        public ObservableCollection<ProfileOperator> ProfileOperators { get; set; }
    }



    public class GammaSpectrometer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private DateTime _commissioningDate;
        private DateTime? _decommissioningDate;
        private float _measurementAccuracy;
        private int _measurementTime;


        [Key]
        public int GammaSpectrometerID { get; set; }

        [Required]
        public DateTime CommissioningDate
        {
            get { return _commissioningDate; }
            set
            {
                if (_commissioningDate != value)
                {
                    _commissioningDate = value;
                    OnPropertyChanged(nameof(CommissioningDate));
                }
            }
        }

        public DateTime? DecommissioningDate
        {
            get { return _decommissioningDate; }
            set
            {
                if (_decommissioningDate != value)
                {
                    _decommissioningDate = value;
                    OnPropertyChanged(nameof(DecommissioningDate));
                }
            }
        }

        [Required]
        public float MeasurementAccuracy
        {
            get { return _measurementAccuracy; }
            set
            {
                if (_measurementAccuracy != value)
                {
                    _measurementAccuracy = value;
                    OnPropertyChanged(nameof(MeasurementAccuracy));
                }
            }
        }

        [Required]
        public int MeasurementTime
        {
            get { return _measurementTime; }
            set
            {
                if (_measurementTime != value)
                {
                    _measurementTime = value;
                    OnPropertyChanged(nameof(MeasurementTime));
                }
            }
        }

        public ObservableCollection<Picket> Pickets { get; set; }

    }

    
    public class Picket : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        [Key]
        public int PicketID { get; set; }

        private float _coordinateX;
        private float _coordinateY;
        private float? _channel1;
        private float? _channel2;
        private float? _channel3;

        [Required]
        public float CoordinateX
        {
            get => _coordinateX;
            set
            {
                _coordinateX = value;
                OnPropertyChanged(nameof(CoordinateX));
            }
        }

        [Required]
        public float CoordinateY
        {
            get => _coordinateY;
            set
            {
                _coordinateY = value;
                OnPropertyChanged(nameof(CoordinateY));
            }
        }

        public float? Channel1
        {
            get => _channel1;
            set
            {
                _channel1 = value;
                OnPropertyChanged(nameof(Channel1));
            }
        }

        public float? Channel2
        {
            get => _channel2;
            set
            {
                _channel2 = value;
                OnPropertyChanged(nameof(Channel2));
            }
        }

        public float? Channel3
        {
            get => _channel3;
            set
            {
                _channel3 = value;
                OnPropertyChanged(nameof(Channel3));
            }
        }

        [ForeignKey("Profile")]
        public int ProfileID { get; set; }
        public Profile Profile { get; set; }

        [ForeignKey("Operator")]
        public int OperatorID { get; set; }
        public Operator Operator { get; set; }

        [ForeignKey("GammaSpectrometer")]
        public int SpectrometerID { get; set; }
        public GammaSpectrometer GammaSpectrometer { get; set; }
    }

    [Index(nameof(AnalystLogin), IsUnique = true)]
    public class Analyst : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        private string _fullName;
        private string _phoneNumber;
        private string _email;
        private string _jobTitle;
        private string _analystLogin;
        private string _analystPassword;

        [Key]
        public int AnalystID { get; set; }

        [Required]
        [MaxLength(255)]
        public string FullName
        {
            get => _fullName;
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        [Required]
        [MaxLength(50)]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        [Required]
        [MaxLength(255)]
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        [Required]
        [MaxLength(100)]
        public string JobTitle
        {
            get => _jobTitle;
            set
            {
                if (_jobTitle != value)
                {
                    _jobTitle = value;
                    OnPropertyChanged(nameof(JobTitle));
                }
            }
        }

        [Required]
        [MaxLength(50)]
        public string AnalystLogin
        {
            get => _analystLogin;
            set
            {
                if (_analystLogin != value)
                {
                    _analystLogin = value;
                    OnPropertyChanged(nameof(AnalystLogin));
                }
            }
        }

        [Required]
        [MaxLength(36)]
        public string AnalystPassword
        {
            get => _analystPassword;
            set
            {
                if (_analystPassword != value)
                {
                    _analystPassword = value;
                    OnPropertyChanged(nameof(AnalystPassword));
                }
            }
        }

        public ObservableCollection<ContractAnalyst> ContractAnalysts { get; set; }
    }
    public class ContractAnalyst
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Contract")]
        public int ContractID { get; set; }
        public Contract Contract { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Analyst")]
        public int AnalystID { get; set; }
        public Analyst Analyst { get; set; }
    }
    public class ProfileOperator
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Profile")]
        public int ProfileID { get; set; }
        public Profile Profile { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Operator")]
        public int OperatorID { get; set; }
        public Operator Operator { get; set; }
    }
}
