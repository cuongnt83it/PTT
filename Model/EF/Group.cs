namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Group")]
    public partial class Group
    {
        public Guid GroupID { get; set; }

        [DisplayName("Tên nhóm")]
        [Required(ErrorMessage = "Mời nhập tên nhóm!")]
        [StringLength(100)]
        public string GroupName { get; set; }

        [DisplayName("Ghi chú")]
        [StringLength(500)]
        public string Note { get; set; }

        [DisplayName("Thuộc Admin")]
        public bool IsAdmin { get; set; }
    }
}
