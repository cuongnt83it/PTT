namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    
    public partial class FeedbacInfokUser
    {
        public long FeedbackID { get; set; }

        public long InformationID { get; set; }

        public long? ChildID { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        [StringLength(100)]
        public string CreateBy { get; set; }

        [StringLength(100)]
        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [StringLength(500)]
        public string FullName { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
        public string UsersRead { get; set; }
    }
}
