using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Spectr.Data
{
    public enum UserType
    {
        Администратор,Оператор,Заказчик,Аналитик
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

        public List<Contract> Contracts { get; set; }
    }
    [Index(nameof(CustomerLogin), IsUnique = true)]
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        [MaxLength(255)]
        public string CompanyName { get; set; }

        [Required]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public string ContactPerson { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string CustomerLogin { get; set; }

        [Required]
        [MaxLength(36)]
        public string CustomerPassword { get; set; }

        public List<Contract> Contracts { get; set; }
    }

    public class Contract : INotifyPropertyChanged
    {
        private int _contractID;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _serviceDescription;

        [Key]
        public int ContractID
        {
            get { return _contractID; }
            set
            {
                if (_contractID != value)
                {
                    _contractID = value;
                    OnPropertyChanged(nameof(ContractID));
                }
            }
        }

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

        public List<Area> Areas { get; set; }

        public List<ContractAnalyst> ContractAnalysts { get; set; }

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

        public List<AreaCoordinates> AreaCoordinates { get; set; }
        public List<Profile> Profiles { get; set; }
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

        public List<ProfileCoordinates> ProfileCoordinates { get; set; }
        public List<Picket> Pickets { get; set; }
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
    public class Operator
    {
        [Key]
        public int OperatorID { get; set; }

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
        [MaxLength(50)]
        public string OperatorLogin { get; set; }

        [Required]
        [MaxLength(36)]
        public string OperatorPassword { get; set; }

        public List<Picket> Pickets { get; set; }
        public List<ProfileOperator> ProfileOperators { get; set; }
    }

    public class GammaSpectrometer
    {
        [Key]
        public int GammaSpectrometerID { get; set; }

        [Required]
        public DateTime CommissioningDate { get; set; }

        public DateTime? DecommissioningDate { get; set; }

        [Required]
        public float MeasurementAccuracy { get; set; }

        [Required]
        public int MeasurementTime { get; set; }

        public List<Picket> Pickets { get; set; }
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
    public class Analyst
    {
        [Key]
        public int AnalystID { get; set; }

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
        [MaxLength(50)]
        public string AnalystLogin { get; set; }

        [Required]
        [MaxLength(36)]
        public string AnalystPassword { get; set; }

        // Связь многие ко многим
        public List<ContractAnalyst> ContractAnalysts { get; set; }
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
