using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTT.Models
{
    public class PermissionAction
    {

        public int PermissionID { get; set; }


        public string Name { get; set; }


        public string Description { get; set; }


        public bool isGranted { get; set; }

    }

}