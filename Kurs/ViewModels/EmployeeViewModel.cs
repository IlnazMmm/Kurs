using System.Collections.ObjectModel;
using System.Windows.Input;
using Kurs.Models;
using Kurs.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kurs.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Employee> Employees { get; set; } = new();

        private Employee _selectedEmployee = new Employee();
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }
        private string _salaryError;
        public string SalaryError
        {
            get => _salaryError;
            set
            {
                _salaryError = value;
                OnPropertyChanged();
            }
        }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public EmployeeViewModel()
        {
            SaveCommand = new Command(async () => await SaveAsync());
            DeleteCommand = new Command<Employee>(async (emp) => await DeleteAsync(emp));
            EditCommand = new Command<Employee>((emp) => Edit(emp));
            LoadAsync();
        }

        public async Task LoadAsync()
        {
            var list = await App.Database.GetEmployeesAsync();
            Employees.Clear();
            foreach (var e in list)
                Employees.Add(e);
        }

        public async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedEmployee?.LastName))
                return;
            if (SelectedEmployee == null)
                return;

            if (SelectedEmployee.Salary <= 0)
            {
                SalaryError = "Оклад должен быть положительным числом";
                return;
            }

            SalaryError = string.Empty;

            if (SelectedEmployee.Id == 0)
            {
                await App.Database.AddEmployeeAsync(SelectedEmployee);
            }
            else
            {
                await App.Database.UpdateEmployeeAsync(SelectedEmployee);
            }

            SelectedEmployee = new Employee();
            await LoadAsync();
        }

        public void Edit(Employee emp)
        {
            SelectedEmployee = new Employee
            {
                Id = emp.Id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                MiddleName = emp.MiddleName,
                Salary = emp.Salary
            };
        }

        public async Task DeleteAsync(Employee emp)
        {
            await App.Database.DeleteEmployeeAsync(emp);
            await LoadAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}