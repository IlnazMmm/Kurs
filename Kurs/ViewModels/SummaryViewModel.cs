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
            var auth = App.Auth;

            // Проверка на авторизованного пользователя и политику доступа
            if (auth == null || auth.CurrentUser == null)
                return;

            var allowedIds = await auth.GetVisibleEmployeeIdsAsync();

            // Грузим все нужные данные
            var allWorks = await App.Database.GetExtraWorksAsync();
            var allEmployees = await App.Database.GetEmployeesAsync();
            var workTypes = await App.Database.GetWorkTypesAsync();

            // Фильтрация по доступным сотрудникам
            var visibleEmployees = allEmployees.Where(e => allowedIds.Contains(e.Id)).ToList();
            var visibleWorks = allWorks.Where(w => allowedIds.Contains(w.EmployeeId)).ToList();


            //var works = await App.Database.GetExtraWorksAsync();
            //var types = await App.Database.GetWorkTypesAsync();
            //var employees = await App.Database.GetEmployeesAsync();

            var summaries = CalculationService.CalculateSummary(visibleEmployees, visibleWorks, workTypes);

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
