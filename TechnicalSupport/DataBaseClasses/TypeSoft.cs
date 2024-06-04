using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport
{
    public class TypeSoft
    {
        [Key]
        public int TypeSoftwareID { get; set; }
        public string NameType { get; set; }
        public virtual ICollection<Software> Softwares { get; set; }
        public TypeSoft() { }
        public TypeSoft(string nameType)
        {
            this.NameType = nameType;

        }
    }
}
