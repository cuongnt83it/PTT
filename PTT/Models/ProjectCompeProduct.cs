using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTT.Models
{
    public class ProjectCompeProduct
    {

        public long ID { get; set; }

        public long? ProjectID { get; set; }

        public long? CompetiorID { get; set; }
        public long ProductID { get; set; }

        public double? Discount { get; set; }
        public double? DiscountVAT { get; set; }

    }

}