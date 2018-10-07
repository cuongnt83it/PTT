namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    
    public partial class ProcessUser
    {
        public long ProcessID { get; set; }

        public long? ProjectID { get; set; }

        
        public string Name { get; set; }

      
        public string Description { get; set; }

        public DateTime? CreateDate { get; set; }

       
        public string CreateBy { get; set; }

       
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
    }
}
