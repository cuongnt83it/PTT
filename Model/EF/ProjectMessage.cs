using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EF
{
    public class ProjectMessage
    {
        public long ProcessID { get; set; }

        public long? ProjectID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }


        public string CreateBy { get; set; }


        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public long ID { get; set; }

        public long? ChildID { get; set; }


        public string ContentMsg { get; set; }

        public DateTime CreateDateMsg { get; set; }


        public string CreateByMsg { get; set; }


        public string ModifiedByMsg { get; set; }

        public DateTime ModifiedDateMsg { get; set; }

        public string FullName { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
        public string UsersRead { get; set; }
    }
}
