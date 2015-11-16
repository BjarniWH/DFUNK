namespace DFUNK.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VolunteerInfo")]
    public partial class VolunteerInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int contact_id { get; set; }

        [StringLength(10)]
        public string tshirtSize { get; set; }

        public bool? vegetariagn { get; set; }

        [StringLength(10)]
        public string drivingLicence { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
