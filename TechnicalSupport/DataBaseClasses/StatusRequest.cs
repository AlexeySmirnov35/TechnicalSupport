using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport.DataBaseClasses
{
    public class StatusRequest
    {
        [Key]
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public StatusRequest() { }
        public StatusRequest(int statId, string nameStatus)
        {
            StatusID = statId;
            StatusName = nameStatus;

        }
    }
}
