using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSupport
{
    public class FilesSoftware
    {
        [Key]
        public int FileID { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public virtual ICollection<Software> Softwares { get; set; }
        public FilesSoftware() { }
        public FilesSoftware(string FileName, byte[] fileContent)
        {
            this.FileName = FileName;
            this.FileContent = fileContent;

        }
    }
}
