using System.Collections.ObjectModel;
using System.Windows.Input;
using Kurs.Models;
using Kurs.Services;

namespace Kurs.ViewModels
{
    public class WorkTypeViewModel : BaseViewModel
    {
        public ObservableCollection<WorkType> WorkTypes { get; } = new();

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _ratePerDay;
        public string RatePerDay
        {
            get => _ratePerDay;
            set => SetProperty(ref _ratePerDay, value);
        }

        public ICommand SaveCommand { get; }

        public WorkTypeViewModel()
        {
            SaveCommand = new Command(async () => await SaveAsync());
            LoadAsync();
        }

        private async Task SaveAsync()
        {
            if (!decimal.TryParse(RatePerDay, out var rate)) return;

            var workType = new WorkType
            {
                Description = Description,
                RatePerDay = rate
            };

            await App.Database.AddWorkTypeAsync(workType);
            await LoadAsync();

            Description = string.Empty;
            RatePerDay = string.Empty;
        }

        public async Task LoadAsync()
        {
            var list = await App.Database.GetWorkTypesAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                WorkTypes.Clear();
                foreach (var wt in list)
                    WorkTypes.Add(wt);
            });
        }
    }
}
