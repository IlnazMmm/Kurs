using Kurs.Models;
using Kurs.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Kurs.ViewModels
{
    public class WorkTypeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<WorkType> WorkTypes { get; set; } = new();

        private WorkType _selectedWorkType = new WorkType();
        public WorkType SelectedWorkType
        {
            get => _selectedWorkType;
            set
            {
                _selectedWorkType = value;
                OnPropertyChanged();
            }
        }

        private string _rateError;
        public string RateError
        {
            get => _rateError;
            set
            {
                _rateError = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public WorkTypeViewModel()
        {
            SaveCommand = new Command(async () => await SaveAsync());
            EditCommand = new Command<WorkType>(Edit);
            DeleteCommand = new Command<WorkType>(async (wt) => await DeleteAsync(wt));
            LoadAsync();
        }

        public async Task LoadAsync()
        {
            var list = await App.Database.GetWorkTypesAsync();
            WorkTypes.Clear();
            foreach (var item in list)
                WorkTypes.Add(item);
        }

        public async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedWorkType?.Description))
                return;

            if (SelectedWorkType.RatePerDay <= 0)
            {
                RateError = "—тавка должна быть положительным числом";
                return;
            }

            RateError = string.Empty;

            if (SelectedWorkType.Id == 0)
                await App.Database.AddWorkTypeAsync(SelectedWorkType);
            else
                await App.Database.UpdateWorkTypeAsync(SelectedWorkType);

            SelectedWorkType = new WorkType();
            await LoadAsync();
        }

        public void Edit(WorkType wt)
        {
            SelectedWorkType = new WorkType
            {
                Id = wt.Id,
                Description = wt.Description,
                RatePerDay = wt.RatePerDay
            };
        }

        public async Task DeleteAsync(WorkType wt)
        {
            await App.Database.DeleteWorkTypeAsync(wt);
            await LoadAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
