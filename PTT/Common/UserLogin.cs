using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTT.Common
{
    [Serializable]
    public class UserLogin

    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public DateTime? LastLogIn { get; set; }
    }
}