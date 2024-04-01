using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport
{
    public class OperatingSystem
    {
        [Key]
        public int OperatingSystemsID { get; set; }
        public string NameOperatingSystem { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public OperatingSystem() { }
        public OperatingSystem(string nameOperatingSystem)
        {
            this.NameOperatingSystem = NameOperatingSystem;

        }
    }
}
