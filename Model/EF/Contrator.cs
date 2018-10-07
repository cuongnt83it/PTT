namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contrator")]
    public partial class Contrator
    {
        public long ID { get; set; }

        [DisplayName("Mã chủ đầu tư")]
        [Required(ErrorMessage = "Mời nhập mã chủ đầu tư!")]
        [StringLength(30)]
        public string ContratorID { get; set; }

        [DisplayName("Tên nhà đầu tư")]
        [Required(ErrorMessage = "Mời nhập tên chủ đầu tư!")]
        [StringLength(100)]
        public string ContraName { get; set; }

        [DisplayName("Đại diện nhà đầu tư")]
        [Required(ErrorMessage = "Mời nhập tên đại diện chủ đầu tư!")]
        [StringLength(100)]
        public string FullName { get; set; }

        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "Mời nhập địa chỉ!")]
        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(100)]
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
        public bool Status { get; set; }
    }
}
