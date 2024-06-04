using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport.DataBaseClasses
{
    public class Software
    {
        [Key]
        public int SoftwareID { get; set; }

        public string SoftwareName { get; set; }
        public string LicenseExpirationDate { get; set; }
        public string WebUrl { get; set; }


        [ForeignKey("FilesSoftware")]
        public int FileID { get; set; }

        public virtual FilesSoftware FilesSoftware { get; set; }

        [ForeignKey("LicensiaInfo")]
        public int LicenseID { get; set; }
        public virtual LicensiaInfo LicensiaInfo{ get; set; }
        [ForeignKey("TypeSofware")]
        public int TypeSofwareID { get; set; }
        public virtual TypeSofware TypeSofware { get; set; }
        public virtual ICollection<SoftwarePosition> SoftwarePositions { get; set; }
        public Software() { }
        public Software(string softwareName, int f,int l, int t)
        {
           this.SoftwareName = softwareName;
            FileID = f;
            LicenseID = l;
            TypeSofwareID = t;

        }
    }
}
