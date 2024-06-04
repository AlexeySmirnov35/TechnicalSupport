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
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Patranomic { get; set; }
        public string Login{ get; set; }
        public string Password { get; set; }
        public string Cabinet { get; set; }
        public string NumberPhone { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public virtual Role Role { get; set; }

        [ForeignKey("Position")]
        public int PositionsID { get; set; }
        public virtual Position Position { get; set; }

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<RequestUser> RequestUsers { get; set; }
        public virtual ICollection<CommitMessage> CommitMessages { get; set; }
        public virtual ICollection<Request> Request { get; set; }
        public User() { }
        public User(string Surname, string Patranomic, string Firsname , string Login, string Password,int RoleID, int dep,int pos)
        {
            this.Firstname = Firstname;
            this.Surname = Surname;
            this.Patranomic=Patranomic;
            this.Login = Login;
            this.Password = Password;
            this.RoleID=RoleID;
            DepartmentID = dep;
            PositionsID = pos;
        }
    }
}
