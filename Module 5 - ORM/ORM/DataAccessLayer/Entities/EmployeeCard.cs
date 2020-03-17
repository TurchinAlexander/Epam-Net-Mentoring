using System;

namespace DataAccessLayer.Entities
{
    public class EmployeeCard
    {
        public int EmployeeCardId { get; set; }

        public string CardNumber { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ExpiredDatetime { get; set; }
    }
}