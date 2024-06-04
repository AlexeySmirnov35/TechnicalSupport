using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport.DataBaseClasses
{
    public class PositionOfficeEquip
    {
        [Key]
        public int PositOffiiceID { get; set; }
       

        public PositionOfficeEquip() { }
        [ForeignKey("Position")]
        public int PositionsID { get; set; }


        public virtual Position Position { get; set; }

        [ForeignKey("OfficeEquipment")]
        public int OfficeEquipID { get; set; }


        public virtual OfficeEquipment OfficeEquipment { get; set; }
        public PositionOfficeEquip(string licenseType)
        {
           // this.LicenseType = licenseType;

        }
    }
}
