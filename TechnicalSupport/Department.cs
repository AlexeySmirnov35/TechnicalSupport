using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Request> Requests { get; set; }

        public Department()
        {
            Users = new HashSet<User>();
            Requests = new HashSet<Request>();
        }

        public Department(string DepartmentName)
        {
            this.DepartmentName = DepartmentName;
            Users = new HashSet<User>();
            Requests = new HashSet<Request>();
        }
    }
}
