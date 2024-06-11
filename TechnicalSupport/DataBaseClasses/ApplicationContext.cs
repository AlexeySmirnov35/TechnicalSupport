using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalSupport.DataBaseClasses;


namespace TechnicalSupport.DataBaseClasses
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        
        public DbSet<LicensiaInfo> LicensiaInfoes { get; set; }
        public DbSet<FilesSoftware> FilesSoftwares { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OfficeEquipment> OfficeEquipments{ get; set; }
        public DbSet<StatusRequest> StatusRequests { get; set; }
        public DbSet<DataBaseClasses.OperatingSystem> OperatingSystems{ get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<PositionOfficeEquip> PositionOfficeEquips { get; set; }
        public DbSet<Request> Requests{ get; set; }
      
        public DbSet<RequestUser> RequestUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<SoftwarePosition> SoftwarePositions { get; set; }
       
        public DbSet<TypeSofware> TypeSofwares { get; set; }
        
        public DbSet<Client> Clients { get; set; }
       public DbSet<CommitMessage> CommitMessages { get; set; }
        public ApplicationContext() : base("DefaultConnection")
        {
        }

    }
}
