namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Content")]
    public partial class Content
    {
        public long ContentID { get; set; }

        [DisplayName("Tiêu đề thông báo")]
        [Required(ErrorMessage = "Mời nhập tiêu đề!")]
        [StringLength(250)]
        public string Name { get; set; }

        [DisplayName("Meta")]
        [StringLength(250)]
        public string MetaTite { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(500)]
        public string Description { get; set; }

        [DisplayName("Ảnh đại diện")]
        [StringLength(250)]
        public string Image { get; set; }

        [DisplayName("Nội dung")]
        [Column(TypeName = "ntext")]
        public string Detail { get; set; }

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

        [DisplayName("Tin hot")]
        public bool? TopHot { get; set; }
        [StringLength(500)]
        public string UsersRead { get; set; }
    }
}
