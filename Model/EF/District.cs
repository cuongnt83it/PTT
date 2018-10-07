namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("District")]
    public partial class District
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string DistrictCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string CityID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }
    }
}
