using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalSupport.DataBaseClasses;


namespace TechnicalSupport.DataBaseClasses
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
       

        public Department()
        {
            Users = new HashSet<User>();
            Clients = new HashSet<Client>();
           
        }

        public Department(string departmentName)
        {
            this.DepartmentName = departmentName;
            Users = new HashSet<User>();
            Clients = new HashSet<Client>();
          
        }
    }
}
