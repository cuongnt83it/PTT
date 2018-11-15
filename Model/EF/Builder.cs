namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Builder")]
    public partial class Builder
    {
        public long ID { get; set; }

        [DisplayName("Mã nhà thầu")]
        [StringLength(30)]
        public string BuilderID { get; set; }

        [DisplayName("Mã số thuế")]
        [Required(ErrorMessage = "Mời nhập mã số thuế!")]
        [StringLength(100)]
        public string TaxID { get; set; }

        [DisplayName("Tên nhà thầu")]
        [Required(ErrorMessage = "Mời nhập tên nhà thầu!")]
        [StringLength(100)]
        public string BuilderName { get; set; }


        [DisplayName("Đại diện nhà thầu")]
        [Required(ErrorMessage = "Mời nhập tên đại diện nhà thầu!")]
        [StringLength(100)]
        public string FullName { get; set; }

        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "Mời nhập địa chỉ!")]
        [StringLength(500)]
        public string Address { get; set; }


        [StringLength(100)]
        [Required(ErrorMessage = "Mời nhập email!")]
        public string Email { get; set; }

        [DisplayName("Ảnh đại diện")]
        [StringLength(250)]
        public string Image { get; set; }

        [DisplayName("Điện thoại")]
        [Required(ErrorMessage = "Mời nhập điện thoại!")]
        [StringLength(100)]
        public string Phone { get; set; }

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
