using SQLite;
using Kurs.Models;
using Kurs.Enums;

namespace Kurs
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _db;

        public async Task InitAsync()
        {
            if (_db != null) return;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "extrawork.db");
            _db = new SQLiteAsyncConnection(dbPath);
            await _db.CreateTableAsync<Employee>();
            await _db.CreateTableAsync<WorkType>();
            await _db.CreateTableAsync<ExtraWork>();
            await _db.CreateTableAsync<User>();
        }
        public Task<List<Employee>> GetEmployeesAsync() => _db.Table<Employee>().ToListAsync();
        public Task<List<WorkType>> GetWorkTypesAsync() => _db.Table<WorkType>().ToListAsync();
        public Task<List<ExtraWork>> GetExtraWorksAsync() => _db.Table<ExtraWork>().ToListAsync();

        public Task<int> AddEmployeeAsync(Employee emp) => _db.InsertAsync(emp);
        public Task<int> UpdateEmployeeAsync(Employee emp) => _db.UpdateAsync(emp);
        public Task<int> DeleteEmployeeAsync(Employee emp) => _db.DeleteAsync(emp);

        public Task<int> AddWorkTypeAsync(WorkType type) => _db.InsertAsync(type);
        public Task<int> UpdateWorkTypeAsync(WorkType type) => _db.UpdateAsync(type);
        public Task<int> DeleteWorkTypeAsync(WorkType type) => _db.DeleteAsync(type);

        public Task<int> AddExtraWorkAsync(ExtraWork work) => _db.InsertAsync(work);
        public Task<int> DeleteExtraWorkAsync(ExtraWork work) => _db.DeleteAsync(work);
        public Task<int> UpdateExtraWorkAsync(ExtraWork work) => _db.UpdateAsync(work);
        public Task<int> AddUserAsync(User user) => _db.InsertAsync(user);

        public async Task<User> AddUserByEmployeeAsync(Employee emp)
        {
            var baseUsername = emp.FullName.Trim().Replace(" ", "_"); ;
            var uniqueUsername = $"{baseUsername}_{Guid.NewGuid().ToString().Substring(0, 8)}";

            var user = new User
            {
                Username = uniqueUsername,
                Password = "1234",
                Role = UserRole.Employee,
                EmployeeId = emp.Id
            };

            await _db.InsertAsync(user);
            return user;
        }
        public Task<int> DeleteUserAsync(User user) => _db.DeleteAsync(user);
        public Task<List<User>> GetUsersAsync() => _db.Table<User>().ToListAsync();
        public Task<User> GetUserByUsernameAsync(string username)
        {
            return _db.Table<User>()
                            .Where(u => u.Username == username)
                            .FirstOrDefaultAsync();
        }
        public async Task<List<ExtraWork>> GetExtraWorkByEmployeeIdAsync(int employeeId)
        {
            return await _db.Table<ExtraWork>()
                .Where(w => w.EmployeeId == employeeId)
                .ToListAsync();
        }
        public async Task<User> GetUserAsync(string username, string password)
        {
            return await _db.Table<User>()
                .Where(u => u.Username == username && u.Password == password)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteEmployeeWithWorksAsync(int employeeId)
        {
            // Удаляем все связанные записи ExtraWork
            var extraWorks = await GetExtraWorkByEmployeeIdAsync(employeeId);
            foreach (var work in extraWorks)
            {
                await _db.DeleteAsync(work);
            }

            // Удаляем самого сотрудника
            var employee = await _db.FindAsync<Employee>(employeeId);
            if (employee != null)
            {
                await _db.DeleteAsync(employee);
            }
        }
        public async Task DeleteWorkTypeWithAssignmentsAsync(int workTypeId)
        {
            // Удаляем все связанные ExtraWork
            var assignments = await _db.Table<ExtraWork>()
                .Where(w => w.WorkTypeId == workTypeId)
                .ToListAsync();

            foreach (var a in assignments)
            {
                await _db.DeleteAsync(a);
            }

            // Удаляем сам WorkType
            var type = await _db.FindAsync<WorkType>(workTypeId);
            if (type != null)
            {
                await _db.DeleteAsync(type);
            }
        }
        public async Task DeleteUserByEmployeeIdAsync(int employeeId)
        {
            var user = await _db.Table<User>()
                .Where(u => u.EmployeeId == employeeId)
                .FirstOrDefaultAsync();
            await _db.DeleteAsync(user);
        }
    }
}