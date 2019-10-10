using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Models.Provider
{
    public partial class User
    {
        public DateTime? ActiveTo { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public string AuthorityAddress { get; set; }
        public string AuthorityName { get; set; }
        public string[] AvailableDashboardReports { get; set; }
        public string[][] CannedResponseOptions { get; set; }
        public bool? ChangePassword { get; set; }
        public string Comment { get; set; }
        public virtual IList<Group> CompanyGroups { get; set; }
        public string DateFormat { get; set; }
        public string DefaultPage { get; set; }
        public string Designation { get; set; }
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string Id { get; set; }
        public bool? IsDriver { get; set; }
        public bool? IsEULAAccepted { get; }
        public bool? IsLabsEnabled { get; set; }
        public bool? IsMetric { get; set; }
        public bool? IsNewsEnabled { get; set; }
        public bool? IsPersonalConveyanceEnabled { get; set; }
        public bool? IsYardMoveEnabled { get; set; }
        public string Language { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public virtual IList<Group> PrivateUserGroups { get; set; }
        public virtual IList<Group> ReportGroups { get; set; }
        public virtual IList<Group> SecurityGroups { get; set; }
        public bool? ShowClickOnceWarning { get; set; }
        public string TimeZoneId { get; set; }


        #region Zuluaga
        //[JsonProperty("firstName")]
        //public string FirstName { get; set; }

        //[JsonProperty("lastName")]
        //public string LastName { get; set; }

        //[JsonProperty("id")]
        //public Guid Id { get; set; }

        //[JsonProperty("userName")]
        //public string UserName { get; set; }

        //[JsonProperty("normalizedUserName")]
        //public string NormalizedUserName { get; set; }

        //[JsonProperty("email")]
        //public string Email { get; set; }

        //[JsonProperty("normalizedEmail")]
        //public string NormalizedEmail { get; set; }

        //[JsonProperty("emailConfirmed")]
        //public bool EmailConfirmed { get; set; }

        //[JsonProperty("passwordHash")]
        //public string PasswordHash { get; set; }

        //[JsonProperty("securityStamp")]
        //public string SecurityStamp { get; set; }

        //[JsonProperty("concurrencyStamp")]
        //public Guid ConcurrencyStamp { get; set; }

        //[JsonProperty("phoneNumber")]
        //public string PhoneNumber { get; set; }

        //[JsonProperty("phoneNumberConfirmed")]
        //public bool PhoneNumberConfirmed { get; set; }

        //[JsonProperty("twoFactorEnabled")]
        //public bool TwoFactorEnabled { get; set; }

        //[JsonProperty("lockoutEnd")]
        //public object LockoutEnd { get; set; }

        //[JsonProperty("lockoutEnabled")]
        //public bool LockoutEnabled { get; set; }

        //[JsonProperty("accessFailedCount")]
        //public long AccessFailedCount { get; set; }

        //[JsonProperty("cityId")]
        //public int CityId { get; set; }

        //[JsonProperty("address")]
        //public string Address { get; set; }

        //public string FullName { get { return $"{this.FirstName} {this.LastName}"; } } 
        #endregion
    }
}
