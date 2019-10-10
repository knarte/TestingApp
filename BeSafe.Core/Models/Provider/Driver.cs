using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Models.Provider
{
    public class Driver
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
        public virtual IList<Group> DriverGroups { get; set; }
        public List<Key> Keys { get; set; }
        public bool? ViewDriversOwnDataOnly { get; set; }
        public string LicenseProvince { get; set; }
        public string LicenseNumber { get; set; }
    }
}
