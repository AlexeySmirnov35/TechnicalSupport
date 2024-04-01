using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport
{
    public class Position
    {
        [Key]
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        [ForeignKey("OperatingSystem")]
        public int OperatingSystemsID { get; set; }
        public virtual OperatingSystem OperatingSystem { get; set; }
        public virtual ICollection<SoftwarePosition> SoftwarePositions { get; set; }
        public virtual ICollection<PositionOfficeEquip> PositionOfficeEquips{ get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<User> Users { get; set; }
        

        public Position() { }
        public Position(string positionName)
        {
            this.PositionName = positionName;

        }
        
   
    //OperatingID INTEGER NULL
    }
}
