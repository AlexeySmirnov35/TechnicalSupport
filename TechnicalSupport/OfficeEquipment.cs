using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport
{
    public class OfficeEquipment
    {
        [Key]
        public int OfficeEquipmentID { get; set; }
        public string NameOfficeEquipment { get; set; }
        public virtual ICollection<PositionOfficeEquip> PositionOfficeEquips{get; set;}
        public OfficeEquipment() { }
        public OfficeEquipment(string nameOfficeEquipment)
        {
            this.NameOfficeEquipment = nameOfficeEquipment;

        }
    }
}
