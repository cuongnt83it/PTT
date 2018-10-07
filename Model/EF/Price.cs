namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Price")]
    public partial class Price
    {
        public long PriceID { get; set; }

        [DisplayName("Khoảng giá")]
        [Required(ErrorMessage = "Mời nhập khoảng giá!")]
        [StringLength(250)]
        public string Name { get; set; }

        [DisplayName("Giá ban đầu")]
        [Required(ErrorMessage = "Mời nhập giá ban đầu!")]
        public decimal? PriceStart { get; set; }

        [DisplayName("Giá kết thúc")]
        [Required(ErrorMessage = "Mời nhập giá kết thúc!")]
        public decimal? PriceEnd { get; set; }

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
