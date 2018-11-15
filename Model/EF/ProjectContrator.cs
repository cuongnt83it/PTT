namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectContrator")]
    public partial class ProjectContrator
    {
        public long ID { get; set; }

        public long? ProjectID { get; set; }

        public long? ContratorID { get; set; }
    }
}
