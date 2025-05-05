using Kurs.Models;

namespace Kurs.Services
{
    public static class CalculationService
    {
        public static decimal CalculatePay(ExtraWork work, WorkType type)
        {
            var days = (work.EndDate - work.StartDate).Days + 1;
            return days * type.RatePerDay;
        }
        public static List<WorkSummary> CalculateSummary(List<Employee> employees, List<ExtraWork> works, List<WorkType> types)
        {
            var summaries = new List<WorkSummary>();

            foreach (var emp in employees)
            {
                var empWorks = works.Where(w => w.EmployeeId == emp.Id).ToList();
                decimal total = 0;
                int totalDays = 0;
                var details = new List<string>();

                foreach (var work in empWorks)
                {
                    var type = types.FirstOrDefault(t => t.Id == work.WorkTypeId);
                    if (type == null) continue;

                    int days = (work.EndDate - work.StartDate).Days + 1;
                    totalDays += days;
                    total += CalculatePay(work, type);

                    details.Add($"{type.Description}: {days} дн. × {type.RatePerDay}₽");
                }

                summaries.Add(new WorkSummary
                {
                    Name = emp.FullName,
                    DaysWorked = totalDays,
                    TotalPay = total,
                    WorkDetails = details
                });
            }

            return summaries;
        }
    }
    public class WorkSummary
    {
        public string Name { get; set; }
        public int DaysWorked { get; set; }
        public decimal TotalPay { get; set; }
        public List<string> WorkDetails { get; set; } = new();
            
        public string WorkDetailsString => string.Join(", ", WorkDetails);
    }
}