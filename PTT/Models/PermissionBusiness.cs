using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PTT.Models
{
    public class PermissionBusiness
    {
       
        [StringLength(64)]
        public string BusinessID { get; set; }

        [DisplayName("Tên nghiệp vụ")]
        [StringLength(250)]
        public string BusinessName { get; set; }

        public int PermissionID { get; set; }
        [DisplayName("Quyền hạn")]
        [StringLength(256)]
        public string Name { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(256)]
        public string Description { get; set; }
       


    }
}