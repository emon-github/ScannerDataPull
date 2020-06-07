namespace ScannerDataPull
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


  

    public partial class Staff
    {
        [Key]
        [Column("perId")]
        public string id { get; set; }

        public string photoUrl { get; set; }

        public string deptName { get; set; }

        public string fiUrl { get; set; }

        public string phone { get; set; }

        public string imToken { get; set; }

        public string appId { get; set; }

        public string name { get; set; }

        public string imUserId { get; set; }

        public string deptId { get; set; }

        public string personType { get; set; }
        public string job { get; set; }
        public DateTime ORDER_BY_DERIVED_0 { get; set; }
    }
}
