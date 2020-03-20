namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeCard")]
    public partial class EmployeeCard
    {
        public int EmployeeCardId { get; set; }

        [Required]
        [StringLength(16)]
        public string CardNumber { get; set; }

        public int EmployeeId { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset ExpiredDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
