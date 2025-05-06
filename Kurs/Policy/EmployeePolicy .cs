using Kurs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Policy
{
    public class EmployeePolicy : IRolePolicy
    {
        private readonly DatabaseService _db;
        private readonly int _employeeId;

        public EmployeePolicy(DatabaseService db, int employeeId)
        {
            _db = db;
            _employeeId = employeeId;
        }

        public async Task<List<int>> GetVisibleEmployeeIdsAsync()
        {
            var myWorks = await _db.GetExtraWorkByEmployeeIdAsync(_employeeId);
            var sharedWorkTypeIds = myWorks.Select(w => w.WorkTypeId).Distinct();

            var all = await _db.GetExtraWorksAsync();
            return all
                .Where(w => sharedWorkTypeIds.Contains(w.WorkTypeId))
                .Select(w => w.EmployeeId)
                .Distinct()
                .ToList();
        }

        public bool CanAccessEmployee(int employeeId)
        {
            return GetVisibleEmployeeIdsAsync().Result.Contains(employeeId);
        }
        public bool CanAccessFeature(AppFeature feature)
        {
            return feature switch
            {
                AppFeature.ViewReports => true,
                _ => false
            };
        }
    }

}
