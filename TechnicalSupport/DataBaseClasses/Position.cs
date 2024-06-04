using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalSupport.DataBaseClasses;
//using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.DataBaseClasses
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
        
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<User> Users { get; set; }
        

        public Position() { }
        public Position(int oper,string positionName)
        {
            this.PositionName = positionName;
            OperatingSystemsID=oper;
        }
        
   
    //OperatingID INTEGER NULL
    }
}
