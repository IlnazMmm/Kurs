using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Kurs.Models;
using Kurs.Services;

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

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public WorkTypeViewModel()
        {
            SaveCommand = new Command(async () => await SaveAsync());
            DeleteCommand = new Command<WorkType>(async (type) => await DeleteAsync(type));
            EditCommand = new Command<WorkType>((type) => Edit(type));

            LoadAsync();
        }

        public async Task LoadAsync()
        {
            var list = await App.Database.GetWorkTypesAsync();
            WorkTypes.Clear();
            foreach (var wt in list)
                WorkTypes.Add(wt);
        }

        public async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedWorkType.Description) || SelectedWorkType.RatePerDay <= 0)
                return;

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
        void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
