using Kurs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Services
{
    public interface IAuthService
    {
        User CurrentUser { get; }
        Task<bool> LoginAsync(string username, string password);
        Task LogoutAsync();
        bool HasAccessToEmployee(int employeeId);
        Task<List<int>> GetVisibleEmployeeIdsAsync();
    }

}
