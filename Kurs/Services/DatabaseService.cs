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
        }

        public Task<List<Employee>> GetEmployeesAsync() => _db.Table<Employee>().ToListAsync();
        public Task<List<WorkType>> GetWorkTypesAsync() => _db.Table<WorkType>().ToListAsync();
        public Task<List<ExtraWork>> GetExtraWorksAsync() => _db.Table<ExtraWork>().ToListAsync();

        public Task<int> AddEmployeeAsync(Employee emp) => _db.InsertAsync(emp);
        public Task<int> AddWorkTypeAsync(WorkType type) => _db.InsertAsync(type);
        public Task<int> AddExtraWorkAsync(ExtraWork work) => _db.InsertAsync(work);
        public async Task<int> UpdateEmployeeAsync(Employee emp)
        {
            if (emp == null)
                return 0;

            // Проверяем, существует ли сотрудник
            var existingEmployee = await _db.Table<Employee>().Where(e => e.Id == emp.Id).FirstOrDefaultAsync();
            if (existingEmployee != null)
            {
                // Если сотрудник существует, обновляем его
                return await _db.UpdateAsync(emp);
            }

            return 0;
        }

        // Удалить сотрудника
        public async Task<int> DeleteEmployeeAsync(Employee emp)
        {
            if (emp == null)
                return 0;

            // Удаляем сотрудника по Id
            return await _db.DeleteAsync(emp);
        }
    }
}