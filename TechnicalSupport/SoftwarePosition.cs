using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport
{
    public class SoftwarePosition
    {
        [Key]
        public int SoftwareProgPositionID { get; set; }

        [ForeignKey("Position")]
        public int PositionID { get; set; }
        public virtual Position Position { get; set; }
        [ForeignKey("Software")]
        public int SoftwareID { get; set; }
        public virtual Software Software{ get; set; }
        public int LicenseTreb { get; set; }
        public SoftwarePosition() { }
        public SoftwarePosition(int sofrwareID, int positionID, int licenseTreb)
        {
            this.SoftwareID = sofrwareID;
            this.PositionID = positionID;
            this.LicenseTreb = licenseTreb;

        }
    }
}
