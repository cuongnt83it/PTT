using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PTT.Models
{
    public class ProjectMember
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

        [DisplayName("Loại dự án")]
        [Required(ErrorMessage = "Mời nhập loại dự án!")]
        public long? CategoryID { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime? CreateDate { get; set; }

        [DisplayName("Người tạo")]
        [StringLength(100)]
        public string CreateBy { get; set; }


        [Required(ErrorMessage = "Mời nhập họ tên")]
        [DisplayName("Họ tên")]
        [StringLength(100)]
        public string FullName { get; set; }

        [DisplayName("Trạng thái")]
        public int? Status { get; set; }

        [DisplayName("Công khai")]
        public bool IsPublic { get; set; }

        [DisplayName("Dự án nhóm")]
        public bool IsGroup { get; set; }


        [DisplayName("Ngày bắt đầu")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Ngày duyệt kết thúc")]
        public DateTime? DateLine { get; set; }
        [DisplayName("Ngày kết thúc")]
        public DateTime? EndDate { get; set; }

    }
}