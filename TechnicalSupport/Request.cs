using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport
{
    public class Request
    {
        [Key]
        public int RequestID { get; set; }
       
        public Request() { }
        [ForeignKey("Position")]
        public int PositionID { get; set; }
        public virtual Position Position { get; set; }

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }
        public string RequestDateStart { get; set; }
        public string RequestDateFinish{ get; set; }
        
        [ForeignKey("Status")]
        public int StatusID { get; set; }
        public virtual Status Status { get; set; }
        public string Description { get; set; }
        public string Fio { get; set; }
        public virtual ICollection<RequestUser> RequestUsers { get; set; }
        public Request(int positionID, int departmentID, int statusID, string requestDateStart, string  RequestDateStart, string description, string fio)
        {
            PositionID = positionID;
            DepartmentID = departmentID;
            StatusID = statusID;
            this.RequestDateStart = requestDateStart;
            this.RequestDateFinish = RequestDateFinish;
            Description = description;
            Fio = fio;
        }
    }
}
