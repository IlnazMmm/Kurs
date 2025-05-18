using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Kurs.Models;
using Kurs.Services;
using Kurs.Services.Alerts;
using Kurs.Views;
using Microcharts;
using SkiaSharp;
using Microcharts.Maui;
using Kurs.Enums;

namespace Kurs.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private int _notificationCount;
        private List<ExtraWork> _worksForChart;
        private List<WorkType> _workTypeChart;

        public string UserName => App.Auth?.CurrentUser?.Username ?? "Пользователь";
        public string CurrentDate => DateTime.Now.ToString("dd MMMM yyyy");
        public string EmployeeCountText { get; set; } = "— сотрудников";
        public string WorkCountText { get; set; } = "— работ";
        public string ReportCountText { get; set; } = "— отчетов";
        public List<string> ChartTypes { get; } = new() { "Bar", "Pie", "Line", "Donut" };

        private string _selectedChartType = "Bar";
        public string SelectedChartType
        {
            get => _selectedChartType;
            set
            {
                if (_selectedChartType != value)
                {
                    _selectedChartType = value;
                    OnPropertyChanged();
                    UpdateDateChart();
                }
            }
        }
        public Chart WorkChart { get; set; }
        public int NotificationCount
        {
            get => _notificationCount;
            set
            {
                _notificationCount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasNotifications));
            }
        }

        public bool HasNotifications => NotificationCount > 0;

        public ICommand LoadNotificationsCommand { get; }
        public ICommand OpenNotificationsCommand { get; }
        public Chart DateChart { get; set; }
        public MainPageViewModel()
        {
            LoadNotificationsCommand = new Command(async () => await LoadAsync());
            OpenNotificationsCommand = new Command(async () => await ShowNotificationsAsync());

            LoadNotificationsCommand.Execute(null);
        }

        public async Task LoadAsync()
        {
            NotificationCount = await NotificationService.GetNotificationCountAsync();

            var allWorks = await App.Database.GetExtraWorksAsync() ?? new List<ExtraWork>();
            var allTypes = await App.Database.GetWorkTypesAsync() ?? new List<WorkType>();
            var allEmployees = await App.Database.GetEmployeesAsync() ?? new List<Employee>();

            var visibleIds = await App.Auth.GetVisibleEmployeeIdsAsync() ?? new List<int>();

            _worksForChart = allWorks.Where(w => visibleIds.Contains(w.EmployeeId)).ToList();
            _workTypeChart = allTypes;

            var myEmployees = allEmployees.Where(e => visibleIds.Contains(e.Id)).ToList();

            EmployeeCountText = $"{myEmployees.Count} сотрудник(ов)";
            WorkCountText = $"{_worksForChart.Count} работ";
            ReportCountText = $"{_worksForChart.Count(w => w.Status == WorkStatus.Approved)} выполнено";

            OnPropertyChanged(nameof(EmployeeCountText));
            OnPropertyChanged(nameof(WorkCountText));
            OnPropertyChanged(nameof(ReportCountText));

            UpdateWorkTypeChart();
            UpdateDateChart();
        }

        private void UpdateWorkTypeChart()
        {
            if (_workTypeChart == null) return;

            WorkChart = ChartBuilderService.BuildWorkTypeChart(_worksForChart, _workTypeChart);
            OnPropertyChanged(nameof(WorkChart));
        }
            

        private void UpdateDateChart()
        {
            if (_worksForChart == null) return;

            DateChart = ChartBuilderService.BuildDateChart(_worksForChart, SelectedChartType);
            OnPropertyChanged(nameof(DateChart));
        }
        
        public async Task ShowNotificationsAsync()
        {
            var list = await NotificationService.GetPendingNotificationsAsync();

            if (list.Count == 0)
            {
                await AlertService.ShowInfo("Нет новых уведомлений.", "Уведомления");
                return;
            }

            var items = list.Select(w => $"{w.StartDate:dd.MM}–{w.EndDate:dd.MM} ({w.RejectionComment})");
            var choice = await AlertService.ChooseAsync("Ваши задачи", items);

            if (!string.IsNullOrWhiteSpace(choice) && choice != "Отмена")
                await NavigationService.NavigateToRootRouteAsync(nameof(SummaryPage));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
