namespace DFUNK.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Payments
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int payment_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? date { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? amount { get; set; }

        [StringLength(10)]
        public string payment_method { get; set; }

        public int contact_id { get; set; }

        public int? project_id { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual Projects Projects { get; set; }
    }
}
