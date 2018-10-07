namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        public long CategoryID { get; set; }

        [DisplayName("Tên loại")]
        [Required(ErrorMessage = "Mời nhập tên loại dự án!")]
        [StringLength(250)]
        public string Name { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(250)]
        public string MetaTite { get; set; }

        [DisplayName("Thứ tự")]
        public int? DisplayOrder { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime? CreateDate { get; set; }

        [DisplayName("Người tạo")]
        [StringLength(100)]
        public string CreateBy { get; set; }

        [DisplayName("Người sửa")]
        [StringLength(100)]
        public string ModifiedBy { get; set; }

        [DisplayName("Ngày sửa")]
        public DateTime? ModifiedDate { get; set; }

        [DisplayName("Trạng thái")]
        public bool? Status { get; set; }
    }
}
