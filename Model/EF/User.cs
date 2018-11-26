namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [Key]
        public long LoginID { get; set; }

        [DisplayName("Tài khoản")]
        [Required(ErrorMessage = "Mời nhập user name.")]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mời nhập mật khẩu.")]
        [DisplayName("Mật khẩu")]
        [StringLength(100)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mời nhập họ tên")]
        [DisplayName("Họ tên")]
        [StringLength(100)]
        public string FullName { get; set; }

        [DisplayName("Địa chỉ")]
        [StringLength(500)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Mời nhập email")]
        [DisplayName("Email")]
        [StringLength(100)]
        public string Email { get; set; }

        [DisplayName("Người tạo")]
        [StringLength(100)]
        public string CreateBy { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }

        [DisplayName("Trạng thái")]
        public bool? LockedUser { get; set; }

        [DisplayName("Ngày khóa")]
        public DateTime? LockedDate { get; set; }

        [DisplayName("Lý do khóa")]
        [StringLength(4000)]
        public string LockedReason { get; set; }

        [DisplayName("Ngày đăng nhập")]
        public DateTime? LastLogIn { get; set; }

        [DisplayName("Ngày đổi mật khẩu")]
        public DateTime? LastChangedPassword { get; set; }

        [DisplayName("Hạn dùng")]
        public DateTime? DeadlineOfUsing { get; set; }

        [DisplayName("Ngày sinh")]
        public DateTime? BirthDay { get; set; }

        [DisplayName("Giới tính")]
        [StringLength(50)]
        public string Gender { get; set; }

        [DisplayName("Aavata")]
        [StringLength(250)]
        public string Image { get; set; }

        [DisplayName("Điện thoại")]
        [StringLength(100)]
        public string Phone { get; set; }
    }
}
