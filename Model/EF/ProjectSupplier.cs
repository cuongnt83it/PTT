namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectSupplier")]
    public partial class ProjectSupplier
    {
        public long ID { get; set; }

        public long? ProjectID { get; set; }

        public long? SupplierID { get; set; }
    }
}
