namespace DFUNK.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CompanyInfo")]
    public partial class CompanyInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int contact_id { get; set; }

        [StringLength(50)]
        public string contactPerson { get; set; }

        [StringLength(50)]
        public string role { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(10)]
        public string phone { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
