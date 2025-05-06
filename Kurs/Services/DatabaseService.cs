using SQLite;
using Kurs.Models;

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

        public Task<int> AddUserAsync(User user) => _db.InsertAsync(user);
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



        //    public async Task<int> UpdateEmployeeAsync(Employee emp)
        //    {
        //        if (emp == null)
        //            return 0;

        //        // Проверяем, существует ли сотрудник
        //        var existingEmployee = await _db.Table<Employee>().Where(e => e.Id == emp.Id).FirstOrDefaultAsync();
        //        if (existingEmployee != null)
        //        {
        //            // Если сотрудник существует, обновляем его
        //            return await _db.UpdateAsync(emp);
        //        }

        //        return 0;
        //    }

        //    // Удалить сотрудника
        //    public async Task<int> DeleteEmployeeAsync(Employee emp)
        //    {
        //        if (emp == null)
        //            return 0;

        //        // Удаляем сотрудника по Id
        //        return await _db.DeleteAsync(emp);
        //    }
        //}
    }
}