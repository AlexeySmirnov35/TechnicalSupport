using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport.DataBaseClasses
{
    public class CommitMessage
    {
        [Key]
        public int ComitID { get; set; }
        public string CommitTextMessage { get; set; }
        

        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Request")]
        public int RequestID { get; set; }
        public virtual Request Request{ get; set; }

       
        public CommitMessage() { }
        public CommitMessage(string Surname, string Patranomic, string Firsname, string Login, string Password, int RoleID)
        {
            
        }
    }
}
