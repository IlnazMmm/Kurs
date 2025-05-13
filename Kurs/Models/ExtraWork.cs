using Kurs.Enums;
using SQLite;

namespace Kurs.Models
{
    public class ExtraWork
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int WorkTypeId { get; set; }
        public int CreatedByUserId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public WorkStatus Status { get; set; } = WorkStatus.NotStarted;

        public DateTime? DateTaken { get; set; }
        public DateTime? DateCompleted { get; set; }

        public string RejectionComment { get; set; }

    }
}