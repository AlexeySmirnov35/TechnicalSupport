using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport.DataBaseClasses
{
    public class OfficeEquipment
    {
        [Key]
        public int OfficeEquipmentID { get; set; }
        public string NameOfficeEquipment { get; set; }
        public string WebUrl { get; set; }
        [ForeignKey("FilesSoftwares")]
        public int FileID { get; set; }
        public virtual FilesSoftware FilesSoftwares { get; set; }
        public virtual ICollection<PositionOfficeEquip> PositionOfficeEquips{get; set;}
        public OfficeEquipment() { }
        public OfficeEquipment(string nameOfficeEquipment)
        {
            this.NameOfficeEquipment = nameOfficeEquipment;

        }
    }
}
