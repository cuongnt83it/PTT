using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTT.Models
{
    public class GroupUserGrand
    {

         public Guid GroupID { get; set; }
        public string GroupName { get; set; }


        public bool isGranted { get; set; }

    }

}