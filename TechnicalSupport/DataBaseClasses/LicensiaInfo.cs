using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport.DataBaseClasses
{
    public class LicensiaInfo
    {
        [Key]
        public int LicenseID { get; set; }
        public string LicenseType { get; set; }
        public virtual ICollection<Software> Softwares { get; set; }
        public LicensiaInfo() { }
        public LicensiaInfo(string licenseType)
        {
            this.LicenseType = licenseType;

        }
    }
}
