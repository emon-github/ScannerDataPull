namespace ScannerDataPull
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelContext : DbContext
    {
        public ModelContext()
            : base("name=DbModel")
        {
        }

        public virtual DbSet<AccessRecord> AccessRecords { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Device1> Devices1 { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
