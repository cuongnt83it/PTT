namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Information
    {
        public long InformationID { get; set; }

        [DisplayName("Tên dự án")]
        [Required(ErrorMessage = "Mời nhập tên!")]
        [StringLength(250)]
        public string Name { get; set; }

        [DisplayName("Địa chỉ")]
        [StringLength(500)]
        public string Address { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(500)]
        public string Description { get; set; }

        [DisplayName("Chủ đầu tư")]
        public long? ContratorID { get; set; }

        [DisplayName("Nhà thầu")]
        public long? BuilderID { get; set; }

        [DisplayName("Nhà cung ứng")]
        public long? SupplierID { get; set; }

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

        [DisplayName("Ngày duyệt kết thúc")]
        public DateTime? DateLine { get; set; }
        [DisplayName("Trạng thái")]
        public int? Status { get; set; }
        [DisplayName("Nghi chú")]
        [StringLength(500)]
        public string Note { get; set; }
    }
}
