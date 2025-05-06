using Kurs.Enums;
using Kurs.Models;
using Kurs.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Services
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseService _db;
        private IRolePolicy _policy;

        public User CurrentUser { get; private set; }

        public AuthService(DatabaseService db)
        {
            _db = db;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = await _db.GetUserAsync(username, password);
            if (user == null)
                return false;

            CurrentUser = user;

            _policy = user.Role switch
            {
                UserRole.Admin => new AdminPolicy(_db),
                UserRole.Employee => new EmployeePolicy(_db, user.EmployeeId),
                _ => throw new NotImplementedException($"Policy not found for role: {user.Role}")
            };

            return true;
        }

        public Task LogoutAsync()
        {
            CurrentUser = null;
            _policy = null;
            return Task.CompletedTask;
        }

        public Task<List<int>> GetVisibleEmployeeIdsAsync()
            => _policy.GetVisibleEmployeeIdsAsync();
        public bool HasAccessToEmployee(int employeeId)
            => _policy.CanAccessEmployee(employeeId);
        public bool CanAccessFeature(AppFeature feature)
            => _policy?.CanAccessFeature(feature) ?? false;
    }
}
