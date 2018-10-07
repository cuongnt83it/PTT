namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public long ProductID { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "Mời nhập tên sản phẩm!")]
        [StringLength(250)]
        public string Name { get; set; }

        [DisplayName("Mã sản phẩm")]
        [Required(ErrorMessage = "Mời nhập mã sản phẩm!")]
        [StringLength(30)]
        public string Code { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(250)]
        public string MetaTite { get; set; }

        [DisplayName("Ghi chú")]
        [StringLength(500)]
        public string Description { get; set; }

        [DisplayName("Giá sản phẩm")]
        public decimal? Price { get; set; }

        [DisplayName("Chiết khấu sản phẩm")]
        public double? Discount { get; set; }

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
