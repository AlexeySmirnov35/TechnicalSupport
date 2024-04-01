using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport
{
    public class Software
    {
        [Key]
        public int SoftwareID { get; set; }

        public string SoftwareName { get; set; }
        public string LicenseExpirationDate { get; set; }
        public string WebUrl { get; set; }


        [ForeignKey("File")]
        public int FileID { get; set; }

        public virtual FilesSoftware File { get; set; }

        [ForeignKey("LicensiaInfo")]
        public int LicensiaID { get; set; }
        public virtual LicensiaInfo LicensiaInfo{ get; set; }
        [ForeignKey("TypeSoft")]
        public int SofwareTypeID { get; set; }
        public virtual TypeSoft TypeSoft { get; set; }
        public virtual ICollection<SoftwarePosition> SoftwarePositions { get; set; }
        public Software() { }
        public Software(string softwareName)
        {
           this.SoftwareName = softwareName;

        }
    }
}
