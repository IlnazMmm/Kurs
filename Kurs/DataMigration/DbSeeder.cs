using Kurs.Enums;
using Kurs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.DataMigration
{
    public static class DbSeeder
    {
        public static async Task RunSeedScriptAsync(DatabaseService db)
        {
            var employees = await db.GetEmployeesAsync();
            if (employees.Any())
                return;

            // Сотрудники
            var emp1 = new Employee { FirstName = "Анна", LastName = "Смирнова", MiddleName = "Сергеевна", Salary = 60000 };
            var emp2 = new Employee { FirstName = "Олег", LastName = "Кузнецов", MiddleName = "Игоревич", Salary = 58000 };

            await db.AddEmployeeAsync(emp1);
            await db.AddUserByEmployeeAsync(emp1);
            await db.AddEmployeeAsync(emp2);
            await db.AddUserByEmployeeAsync(emp1);

            // Виды работ
            var t1 = new WorkType { Description = "Дежурство", RatePerDay = 1200 };
            var t2 = new WorkType { Description = "Смена выходного дня", RatePerDay = 1700 };
            await db.AddWorkTypeAsync(t1);
            await db.AddWorkTypeAsync(t2);

            // Администратор
            var admin = new User
            {
                Username = "admin",
                Password = "admin",
                Role = UserRole.Admin
            };
            await db.AddUserAsync(admin);

            // Дополнительная работа
            await db.AddExtraWorkAsync(new ExtraWork
            {
                EmployeeId = emp1.Id,
                WorkTypeId = t1.Id,
                StartDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today
            });

            await db.AddExtraWorkAsync(new ExtraWork
            {
                EmployeeId = emp2.Id,
                WorkTypeId = t2.Id,
                StartDate = DateTime.Today.AddDays(-2),
                EndDate = DateTime.Today
            });
        }
    }

}
