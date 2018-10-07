namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Supplier")]
    public partial class Supplier
    {
        public long ID { get; set; }


        [DisplayName("Mã nhà cung ứng")]
        [Required(ErrorMessage = "Mời nhập mã nhà cung ứng!")]
        [StringLength(30)]
        public string SupplierID { get; set; }

        [DisplayName("Tên nhà cung ứng")]
        [Required(ErrorMessage = "Mời nhập tên nhà cung ứng!")]
        [StringLength(100)]
        public string SupplierName { get; set; }

        [DisplayName("Đại diện nhà cung ứng")]
        [Required(ErrorMessage = "Mời nhập tên đại diện nhà cung ứng!")]
        [StringLength(100)]
        public string FullName { get; set; }


        [DisplayName("Quận huyện")]
        [StringLength(25)]
        public string DistrictID { get; set; }

        [DisplayName("Thành phố")]
        [StringLength(25)]
        public string CityID { get; set; }

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
        public bool? Status { get; set; }
    }
}
