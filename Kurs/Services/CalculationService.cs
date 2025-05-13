using Kurs.Enums;
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
        public static List<WorkSummary> CalculatePerWorkSummary(
             List<Employee> employees,
             List<ExtraWork> works,
             List<WorkType> types)
        {
            var summaries = new List<WorkSummary>();

            foreach (var work in works)
            {
                var emp = employees.FirstOrDefault(e => e.Id == work.EmployeeId);
                var type = types.FirstOrDefault(t => t.Id == work.WorkTypeId);
                if (emp == null || type == null) continue;
                if (work.Status == WorkStatus.Approved) continue; // скрываем

                int days = (work.EndDate - work.StartDate).Days + 1;
                decimal total = days * type.RatePerDay;
                    
                summaries.Add(new WorkSummary
                {
                    Name = emp.FullName,
                    DaysWorked = days,
                    TotalPay = total,
                    RawWork = work,
                    WorkDetails = type.Description
                });
            }

            return summaries;
        }
        public class WorkSummary
        {
            public string Name { get; set; }
            public int DaysWorked { get; set; }
            public decimal TotalPay { get; set; }
            public string WorkDetails { get; set; }

            public string WorkDetailsString => WorkDetails;

            public ExtraWork RawWork { get; set; } // Ссылка на запись
            public WorkStatus Status => RawWork?.Status ?? WorkStatus.NotStarted;
            public string DateRange => $"{RawWork?.StartDate:dd.MM.yyyy} — {RawWork?.EndDate:dd.MM.yyyy}";

            public string StatusColor => Status switch
            {
                WorkStatus.NotStarted => "LightGray",
                WorkStatus.InProgress => "Orange",
                WorkStatus.Completed => "SkyBlue",
                WorkStatus.PendingReview => "MediumPurple",
                WorkStatus.Approved => "LightGreen",
                WorkStatus.Rejected => "Tomato",
                _ => "White"
            };
            public bool IsPendingReview =>
                Status == WorkStatus.Completed || Status == WorkStatus.PendingReview;
            public bool CanReview =>
                RawWork?.CreatedByUserId == App.Auth.CurrentUser?.Id &&
                Status == WorkStatus.Completed;
            public bool IsMine =>
                 App.Auth?.CurrentUser?.EmployeeId == RawWork?.EmployeeId;
            public bool ShowTakeButton => IsMine && Status == WorkStatus.NotStarted;
            public bool ShowCompleteButton => IsMine && Status == WorkStatus.InProgress;
            public string? RejectionComment =>
                Status == WorkStatus.Rejected || Status == WorkStatus.InProgress
                    ? RawWork?.RejectionComment
                    : null;
            public bool ShowRejectionComment => !string.IsNullOrWhiteSpace(RejectionComment);

        }
    }
}