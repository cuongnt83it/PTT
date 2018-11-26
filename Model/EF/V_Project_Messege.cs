namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_Project_Messege
    {
        [StringLength(50)]
        public string Code { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ProjectID { get; set; }

        [StringLength(25)]
        public string CityID { get; set; }

        [StringLength(25)]
        public string DistrictID { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string MetaTite { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public long? CategoryID { get; set; }

        public long? ResourceID { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        public decimal? Value { get; set; }

        public int? DisplayOrder { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(100)]
        public string CreateBy { get; set; }

        public long? PriceID { get; set; }

        [StringLength(100)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? EndCreate { get; set; }

        public DateTime? DateLine { get; set; }

        public int? Status { get; set; }

        public bool? IsPublic { get; set; }

        public bool? IsGroup { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        [StringLength(255)]
        public string CityName { get; set; }

        [StringLength(250)]
        public string CategoryName { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ProcessID { get; set; }

        [StringLength(500)]
        public string DescriptionProcess { get; set; }

        [StringLength(50)]
        public string NameProcess { get; set; }

        public DateTime? CreateDateProcess { get; set; }

        [StringLength(100)]
        public string CreateByProcess { get; set; }

        public long? ChildID { get; set; }

        [StringLength(500)]
        public string DescriptionMessege { get; set; }

        public DateTime? CreateDateMessege { get; set; }

        [StringLength(100)]
        public string CreateByMessege { get; set; }
    }
}
