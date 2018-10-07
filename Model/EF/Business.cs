namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Business")]
    public partial class Business
    {
        [StringLength(64)]
        public string BusinessID { get; set; }

        [DisplayName("Tên nghiệp vụ")]
        [StringLength(250)]
        public string BusinessName { get; set; }
    }
}
