namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Messege")]
    public partial class Messege
    {
        public long ID { get; set; }

        public long? ChildID { get; set; }

        public long ProcessID { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        [StringLength(100)]
        public string CreateBy { get; set; }

        [StringLength(100)]
        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [StringLength(500)]
        public string UsersRead { get; set; }
    }
}
