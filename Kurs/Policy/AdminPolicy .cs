using Kurs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Policy
{
    public class AdminPolicy : IRolePolicy
    {
        private readonly DatabaseService _db;

        public AdminPolicy(DatabaseService db)
        {
            _db = db;
        }

        public async Task<List<int>> GetVisibleEmployeeIdsAsync()
        {
            var all = await _db.GetEmployeesAsync();
            return all.Select(e => e.Id).ToList();
        }

        public bool CanAccessEmployee(int employeeId) => true;
        public bool CanAccessFeature(AppFeature feature) => true;
    }

}
