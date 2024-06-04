using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport.DataBaseClasses
{
    public class Client
    {
        [Key]
        public int ClientID { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Patranomic { get; set; }
        public string Cabinet { get; set; }
        public string NumberPhone { get; set; }

       
        [ForeignKey("Position")]
        public int PositionID { get; set; }
        public virtual Position Position { get; set; }

        [ForeignKey("Department")]
        public int DepartamentID { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
       
        public Client() { }
        public Client(string Surname, string Patranomic, string Firsname, string Login, string Password, int RoleID)
        {
            this.Firstname = Firstname;
            this.Surname = Surname;
            this.Patranomic = Patranomic;
            this.Cabinet = Login;
            this.NumberPhone = Password;
            
        }
    }
}
