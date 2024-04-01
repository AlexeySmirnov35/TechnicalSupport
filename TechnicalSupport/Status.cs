using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport
{
    public class Status
    {
        [Key]
        public int StatusID { get; set; }
        public string NameStatus { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public Status() { }
        public Status(string nameStatus)
        {
            this.NameStatus = nameStatus;

        }
    }
}
