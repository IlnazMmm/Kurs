using Kurs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Policy
{
    public interface IRolePolicy
    {
        Task<List<int>> GetVisibleEmployeeIdsAsync();
        bool CanAccessEmployee(int employeeId);
        bool CanAccessFeature(AppFeature feature);
    }
}
