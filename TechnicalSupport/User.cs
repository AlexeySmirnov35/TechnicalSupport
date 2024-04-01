   using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Patranomic { get; set; }
        public string Login{ get; set; }
        public string Password { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public virtual Role Role { get; set; }

        [ForeignKey("Position")]
        public int PositionsID { get; set; }
        public virtual Position Position { get; set; }

        [ForeignKey("Department")]
        public int DepartamentsID { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<RequestUser> RequestUsers { get; set; }
        public User() { }
        public User(string Surname, string Patranomic, string Firsname , string Login, string Password,int RoleID)
        {
            this.Firstname = Firstname;
            this.Surname = Surname;
            this.Patranomic=Patranomic;
            this.Login = Login;
            this.Password = Password;
            this.RoleID=RoleID;
        }
    }
}
