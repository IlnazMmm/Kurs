using System.Collections.ObjectModel;
using System.Windows.Input;
using Kurs.Enums;
using Kurs.Models;
using Kurs.Services;
using Kurs.Services.Alerts;
using Kurs.Services.Filters;
using static Kurs.Services.CalculationService;

namespace Kurs.ViewModels
{
    public class SummaryViewModel : BaseViewModel
    {
        public ObservableCollection<WorkSummary> Summary { get; } = new();

        public ICommand LoadCommand { get; }
        public ICommand TakeCommand { get; }
        public ICommand CompleteCommand { get; }
        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        public List<ReportFilter> Filters { get; } = Enum.GetValues(typeof(ReportFilter)).Cast<ReportFilter>().ToList();

        private ReportFilter _selectedFilter = ReportFilter.All;
        public ReportFilter SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged();
                LoadCommand.Execute(null);
            }
        }
        public SummaryViewModel()
        {
            LoadCommand = new Command(async () => await LoadSummaryAsync());
            TakeCommand = new Command<WorkSummary>(async (s) => await MarkInProgressAsync(s));
            CompleteCommand = new Command<WorkSummary>(async (s) => await MarkCompletedAsync(s));
            ApproveCommand = new Command<WorkSummary>(async (s) => await ApproveAsync(s));
            RejectCommand = new Command<WorkSummary>(async (s) => await RejectAsync(s));
            LoadSummaryAsync();
        }

        private async Task LoadSummaryAsync()
        {
            var auth = App.Auth;
            if (auth?.CurrentUser == null) return;


            var allowedIds = await auth.GetVisibleEmployeeIdsAsync();
            var allWorks = await App.Database.GetExtraWorksAsync();
            var allEmployees = await App.Database.GetEmployeesAsync();  
            var workTypes = await App.Database.GetWorkTypesAsync();

            var visibleEmployees = allEmployees.Where(e => allowedIds.Contains(e.Id)).ToList();
            var visibleWorks = allWorks.Where(w => allowedIds.Contains(w.EmployeeId)).ToList();

            var filteredWorks = ReportFilterService.ApplyFilter(
                visibleWorks,
                auth.CurrentUser,
                SelectedFilter
            );


            var summaries = CalculationService.CalculatePerWorkSummary(visibleEmployees, filteredWorks, workTypes);

            // Привязываем ExtraWork к каждой записи
            //foreach (var summary in summaries)
            //{
            //    summary.RawWork = visibleWorks.FirstOrDefault(w => w.EmployeeId == visibleEmployees.FirstOrDefault(e => e.FullName == summary.Name)?.Id);
            //}

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Summary.Clear();
                foreach (var s in summaries)
                    Summary.Add(s);
            });
        }

        private async Task MarkInProgressAsync(WorkSummary summary)
        {
            if (summary.RawWork == null) return;

            summary.RawWork.Status = WorkStatus.InProgress;
            summary.RawWork.DateTaken = DateTime.Now;
            await App.Database.UpdateExtraWorkAsync(summary.RawWork);
            await LoadSummaryAsync();
        }

        private async Task MarkCompletedAsync(WorkSummary summary)
        {
            if (summary.RawWork == null) return;

            summary.RawWork.Status = WorkStatus.Completed;
            summary.RawWork.DateCompleted = DateTime.Now;
            await App.Database.UpdateExtraWorkAsync(summary.RawWork);
            await LoadSummaryAsync();
        }

        private async Task ApproveAsync(WorkSummary summary)
        {
            if (summary.RawWork == null) return;

            bool confirm = await AlertService.ConfirmAsync("Вы уверены, что хотите принять эту работу?");
            if (!confirm) return;

            summary.RawWork.Status = WorkStatus.Approved;
            await App.Database.UpdateExtraWorkAsync(summary.RawWork);
            await LoadSummaryAsync();
        }

        private async Task RejectAsync(WorkSummary summary)
        {
            if (summary.RawWork == null) return;

            bool confirm = await AlertService.ConfirmAsync("Вы уверены, что хотите отклонить выполнение этой работы?");
            if (!confirm) return;

            var reason = await AlertService.PromptAsync("Укажите причину отклонения", "Комментарий");
            if (string.IsNullOrWhiteSpace(reason))
                reason = "Без комментария";

            summary.RawWork.RejectionComment = reason;
            summary.RawWork.Status = WorkStatus.Rejected;
            await App.Database.UpdateExtraWorkAsync(summary.RawWork);
            await LoadSummaryAsync();
        }
    }
}
