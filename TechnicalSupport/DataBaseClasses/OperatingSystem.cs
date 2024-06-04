using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport.DataBaseClasses
{
    public class OperatingSystem
    {
        [Key]
        public int OperatingSystemsID { get; set; }
        public string NameOperatingSystem { get; set; }
        public string WebUrl { get; set; }


        [ForeignKey("FilesSoftwares")]
        public int FileID { get; set; }
        public virtual FilesSoftware FilesSoftwares { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public OperatingSystem() { }
        public OperatingSystem(int oper,string nameOperatingSystem)
        {
            this.NameOperatingSystem = NameOperatingSystem;
            OperatingSystemsID=oper;
        }
    }
}
