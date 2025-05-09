using Kurs.Enums;
using Kurs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Services
{
    public static class NotificationService
    {
        public static async Task<List<ExtraWork>> GetPendingNotificationsAsync()
        {
            var user = App.Auth.CurrentUser;
            if (user == null || user.EmployeeId == 0)
                return new();

            var works = await App.Database.GetExtraWorksAsync();
            return works.Where(w =>
                w.EmployeeId == user.EmployeeId && w.Status == WorkStatus.Rejected)
                .OrderByDescending(w => w.EndDate)
                .ToList();
        }

        public static async Task<int> GetNotificationCountAsync()
        {
            var list = await GetPendingNotificationsAsync();
            return list.Count;
        }
    }
}
