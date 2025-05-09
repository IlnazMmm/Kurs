using Kurs.Enums;
using Kurs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Services.Filters
{
    public static class ReportFilterService
    {
        public static List<ExtraWork> ApplyFilter(
            List<ExtraWork> allWorks,
            User currentUser,
            ReportFilter selectedFilter)
        {
            if (currentUser == null)
                return new List<ExtraWork>();

            return selectedFilter switch
            {
                ReportFilter.All => allWorks
                    .Where(w => w.Status != WorkStatus.Approved)
                    .ToList(),

                ReportFilter.OnlyMine => allWorks
                    .Where(w => w.EmployeeId == currentUser.EmployeeId &&
                                w.Status != WorkStatus.Approved)
                    .ToList(),

                ReportFilter.ToReview => allWorks
                    .Where(w => w.Status == WorkStatus.Completed &&
                                w.CreatedByUserId == currentUser.Id)
                    .ToList(),

                ReportFilter.NotCompleted => allWorks
                    .Where(w => w.Status is WorkStatus.NotStarted
                                         or WorkStatus.InProgress
                                         or WorkStatus.Rejected)
                    .ToList(),

                _ => new List<ExtraWork>()
            };
        }
    }
}
