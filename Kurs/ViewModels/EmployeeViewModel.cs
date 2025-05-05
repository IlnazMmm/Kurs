using System.Collections.ObjectModel;
using System.Windows.Input;
using Kurs.Models;
using Kurs.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

namespace Kurs.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Employee> Employees { get; set; } = new();
        private List<Employee> _allEmployees = new(); // ������������ ������ ��� ����������

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

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
                FilterEmployees();
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
            _allEmployees = list.ToList();
            FilterEmployees();
        }

        private void FilterEmployees()
        {
            var filtered = string.IsNullOrWhiteSpace(SearchQuery)
                ? _allEmployees
                : _allEmployees.Where(e => e.FullName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();

            var topThree = filtered.Take(3).ToList();

            Employees.Clear();
            foreach (var e in topThree)
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
                SalaryError = "����� ������ ���� ������������� ������";
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
