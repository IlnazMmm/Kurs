using System.Collections;
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

            // Назначаем работу для каждого выбранного сотрудника
            foreach (var obj in selected)
            {
                if (obj is Employee employee)
                {
                    var work = new ExtraWork
                    {
                        EmployeeId = employee.Id,
                        WorkTypeId = SelectedWorkType.Id,
                        StartDate = StartDate,
                        EndDate = EndDate
                    };

                    await App.Database.AddExtraWorkAsync(work);
                }
            }

            await App.Current.MainPage.DisplayAlert("Готово", "Работа назначена для выбранных сотрудников", "OK");
        }

        public async Task LoadAsync()
        {
            Employees.Clear();
            foreach (var e in await App.Database.GetEmployeesAsync())
                Employees.Add(e);

            WorkTypes.Clear();
            foreach (var t in await App.Database.GetWorkTypesAsync())
                WorkTypes.Add(t);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
