using SQLite;

namespace Kurs.Models
{
    public class ExtraWork
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int WorkTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}