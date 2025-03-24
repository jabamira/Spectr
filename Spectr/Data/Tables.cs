using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

    public class Contract
    {
        [Key]
        public int ContractID { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string ServiceDescription { get; set; }

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Administrator")]
        public int AdministratorID { get; set; }
        public Administrator Administrator { get; set; }

        public List<Area> Areas { get; set; }
        public List<Analyst> Analysts { get; set; }
    }

    public class Area
    {
        [Key]
        public int AreaID { get; set; }

        [Required]
        [MaxLength(255)]
        public string AreaName { get; set; }

        [ForeignKey("Contract")]
        public int ContractID { get; set; }
        public Contract Contract { get; set; }

        public List<AreaCoordinates> AreaCoordinates { get; set; }
        public List<Profile> Profiles { get; set; }
    }

    public class AreaCoordinates
    {
        [Key]
        public int AreaCoordinatesID { get; set; }

        [Required]
        public float X { get; set; }

        [Required]
        public float Y { get; set; }

        [ForeignKey("Area")]
        public int AreaID { get; set; }
        public Area Area { get; set; }
    }

    public class Profile
    {
        [Key]
        public int ProfileID { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProfileName { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProfileType { get; set; }

        [ForeignKey("Area")]
        public int AreaID { get; set; }
        public Area Area { get; set; }

        public List<ProfileCoordinates> ProfileCoordinates { get; set; }
        public List<Picket> Pickets { get; set; }
    }

    public class ProfileCoordinates
    {
        [Key]
        public int ProfileCoordinatesID { get; set; }

        [Required]
        public float X { get; set; }

        [Required]
        public float Y { get; set; }

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

    public class Picket
    {
        [Key]
        public int PicketID { get; set; }

        [Required]
        public float CoordinateX { get; set; }

        [Required]
        public float CoordinateY { get; set; }

        public float? Channel1 { get; set; }
        public float? Channel2 { get; set; }
        public float? Channel3 { get; set; }

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

        [ForeignKey("Contract")]
        public int ContractID { get; set; }
        public Contract Contract { get; set; }
    }
}
