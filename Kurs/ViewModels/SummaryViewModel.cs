using System.Collections.ObjectModel;
using System.Windows.Input;
using Kurs.Models;
using Kurs.Services;

namespace Kurs.ViewModels
{
    public class SummaryViewModel : BaseViewModel
    {
        public ObservableCollection<WorkSummary> Summary { get; } = new();

        public ICommand LoadCommand { get; }

        public SummaryViewModel()
        {
            LoadCommand = new Command(async () => await LoadSummaryAsync());
            LoadSummaryAsync();
        }

        private async Task LoadSummaryAsync()
        {
            var works = await App.Database.GetExtraWorksAsync();
            var types = await App.Database.GetWorkTypesAsync();
            var employees = await App.Database.GetEmployeesAsync();

            var summaries = CalculationService.CalculateSummary(employees, works, types);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Summary.Clear();
                foreach (var s in summaries)
                {
                    Summary.Add(s);
                }
            });
        }
    }
}
