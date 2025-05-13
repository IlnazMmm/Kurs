using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Kurs.Models;
using Kurs.Services;

namespace Kurs.ViewModels
{
    public class WorkAssignmentViewModel : BaseViewModel
    {
        public ObservableCollection<Employee> Employees { get; } = new();
        private List<Employee> _allEmployees = new();

        public ObservableCollection<WorkType> WorkTypes { get; } = new();

        private WorkType _selectedWorkType;
        public WorkType SelectedWorkType
        {
            get => _selectedWorkType;
            set
            {
                _selectedWorkType = value;
                OnPropertyChanged(nameof(SelectedWorkType));
            }
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                FilterEmployees();
            }
        }

        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today;

        public ICommand AssignCommand { get; }

        public WorkAssignmentViewModel()
        {
            AssignCommand = new Command(async () => await AssignWorkAsync());
            LoadAsync();
        }

        private async Task AssignWorkAsync()
        {
            var selected = Employees.Where(e => e.IsSelected).ToList();
            if (SelectedWorkType == null || !selected.Any())
                return;

            foreach (var employee in selected)
            {
                var work = new ExtraWork
                {
                    EmployeeId = employee.Id,
                    WorkTypeId = SelectedWorkType.Id,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    CreatedByUserId = App.Auth.CurrentUser.Id
                };

                await App.Database.AddExtraWorkAsync(work);
            }
            await App.Current.MainPage.DisplayAlert("Готово", "Работа назначена для выбранных сотрудников", "OK");
        }

        public async Task LoadAsync()
        {
            _allEmployees = (await App.Database.GetEmployeesAsync()).ToList();
            FilterEmployees();

            WorkTypes.Clear();
            foreach (var t in await App.Database.GetWorkTypesAsync())
                WorkTypes.Add(t);
        }

        private void FilterEmployees()
        {   
            Employees.Clear();  

            var filtered = string.IsNullOrWhiteSpace(SearchQuery)
                ? _allEmployees
                : _allEmployees.Where(e =>
                    e.FullName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();

            var topThree = filtered.Take(3).ToList();

            Employees.Clear();
            foreach (var emp in topThree)
                Employees.Add(emp);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
