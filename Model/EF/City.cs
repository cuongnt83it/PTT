namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("City")]
    public partial class City
    {
        [StringLength(20)]
        public string CityID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }
    }
}
