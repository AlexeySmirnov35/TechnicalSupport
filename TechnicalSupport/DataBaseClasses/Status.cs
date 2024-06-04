using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalSupport;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.DataBaseClasses
{
    public class Status
    {
        [Key]
        public int StatusID { get; set; }
        public string NameStatus { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public Status() { }
        public Status(int statId, string nameStatus)
        {
            StatusID=statId;
            NameStatus = nameStatus;

        }
    }
}




   
