using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.DataBaseClasses
{
    public class RequestUser
    {
        [Key]
        public int RequestUserID { get; set; }
        

        public RequestUser() { }
        [ForeignKey("Request")]
        public int RequestID { get; set; }
        public virtual Request Request { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }
       
        
        public RequestUser(string licenseType)
        {
            //this.LicenseType = licenseType;

        }
    }
}
