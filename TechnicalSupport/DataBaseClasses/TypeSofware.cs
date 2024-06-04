using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport.DataBaseClasses
{
    public class TypeSofware
    {
        [Key]
        public int TypeSofwareID { get; set; }
        public string NameType { get; set; }
        public virtual ICollection<Software> Softwares { get; set; }
        public TypeSofware() { }
        public TypeSofware(string nameType)
        {
            this.NameType = nameType;

        }
    }
}
