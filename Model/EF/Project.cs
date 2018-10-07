namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Project")]
    public partial class Project
    {
        public long ProjectID { get; set; }


        [DisplayName("Tên dự án")]
        [Required(ErrorMessage = "Mời nhập tên!")]
        [StringLength(250)]
        public string Name { get; set; }

        [DisplayName("Mã dự án")]
        [Required(ErrorMessage = "Mời nhập mã dự án!")]
        [StringLength(30)]
        public string Code { get; set; }

        [DisplayName("Thành phố")]
        [StringLength(25)]
        public string CityID { get; set; }

        [DisplayName("Quận huyện")]
        [StringLength(25)]
        public string DistrictID { get; set; }

        [StringLength(250)]
        public string MetaTite { get; set; }

        [DisplayName("Địa chỉ")]
        [StringLength(500)]
        public string Address { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(500)]
        public string Description { get; set; }

        [DisplayName("Chủ đầu tư")]
        [Required(ErrorMessage = "Mời nhập chủ đầu tư!")]
        public long? ContratorID { get; set; }

        [DisplayName("Loại dự án")]
        [Required(ErrorMessage = "Mời nhập loại dự án!")]
        public long? CategoryID { get; set; }

        [DisplayName("Nhà thầu")]
        [Required(ErrorMessage = "Mời nhập nhà thầu!")]
        public long? BuilderID { get; set; }


        [DisplayName("Nhà cung ứng")]
        public long? SupplierID { get; set; }

        [DisplayName("Nguồn vốn")]
        public long? ResourceID { get; set; }
        
        [DisplayName("Giá trị dự án")]
        public long? PriceID { get; set; }

        [DisplayName("Tổng giá")]
        public decimal? Value { get; set; }
        [DisplayName("Thứ tự")]
        public int? DisplayOrder { get; set; }

        [DisplayName("Ngày bắt đầu")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Ngày kết thúc")]
        public DateTime? EndDate { get; set; }

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

        [DisplayName("Kết thúc khởi tạo")]
        public DateTime? EndCreate { get; set; }

        [DisplayName("Ngày duyệt kết thúc")]
        public DateTime? DateLine { get; set; }

        [DisplayName("Trạng thái")]
        public int? Status { get; set; }

        [DisplayName("Dự án công khai")]
        public bool IsPublic { get; set; }

        [DisplayName("Dự án nhóm")]
        public bool IsGroup { get; set; }

     

        [DisplayName("Nghi chú")]
        [StringLength(500)]
        public string Note { get; set; }
        [DisplayName("Nghi chú khởi tạo")]
        [StringLength(500)]
        public string NotePass { get; set; }
    }
}
