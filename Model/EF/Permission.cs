namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Permission")]
    public partial class Permission
    {
        public int PermissionID { get; set; }
        [DisplayName("Quyền hạn")]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Description { get; set; }
        [DisplayName("Tên nghiệp vụ")]
        [StringLength(64)]
        public string BusinessID { get; set; }
    }
}
