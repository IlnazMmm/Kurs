using SQLite;

namespace Kurs.Models
{
    public class WorkType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal RatePerDay { get; set; }
    }
}