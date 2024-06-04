using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalSupport.Pages;

namespace TechnicalSupport.DataBaseClasses
{
    public class FilesSoftware
    {
        [Key]
        public int FileID { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public virtual ICollection<Software> Softwares { get; set; }
        public virtual ICollection<DataBaseClasses.OperatingSystem> OperatingSystems { get; set; }
        public virtual ICollection<OfficeEquipment> OfficeEquipments { get; set; }
        public FilesSoftware() { }
        public FilesSoftware(string fileName, byte[] fileContent)
        {
            this.FileName = fileName;
            this.FileContent = fileContent;

        }
    }
}
