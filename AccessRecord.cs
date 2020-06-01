namespace ScannerDataPull
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AccessRecord
    {
        public string id { get; set; }

        public DateTime recordTime { get; set; }

        public string passType { get; set; }

        public string sn { get; set; }

        public string equipName { get; set; }

        public string photoUrl { get; set; }

        public string photoSize { get; set; }

        public string name { get; set; }

        public string phone { get; set; }

        public string arType { get; set; }

        public string temperature { get; set; }

        public string deptId { get; set; }

        public string deptName { get; set; }

        public string cardNum { get; set; }
    }
}
