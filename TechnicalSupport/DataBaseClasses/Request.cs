using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.DataBaseClasses
{
    public class Request
    {
        [Key]
        public int RequestID { get; set; }
       
        public Request() { }
       
        public string RequestDateStart { get; set; }
        public string RequestDateFinish{ get; set; }
        public string RequestDeadline { get; set; }
        public string Description {  get; set; }

        [ForeignKey("StatusRequest")]
        public int StatusID { get; set; }
        public virtual StatusRequest StatusRequest { get; set; }
        [ForeignKey("Client")]
        public int ClientID { get; set; }
        public virtual Client Client { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<RequestUser> RequestUsers { get; set; }
        public Request(int positionID, int departmentID, int statusID,  string requestDateStart, 
            string requestDateFinish,
            string description, string fio, string requestDeadline)
        {
            //   PositionID = positionID;
            // DepartmentID = departmentID;
            StatusID = statusID;
            this.RequestDateStart = requestDateStart;
            this.RequestDateFinish = requestDateFinish;
            Description = description;
            RequestDeadline = requestDeadline;
            // Fio = fio;
        }
    }
}
