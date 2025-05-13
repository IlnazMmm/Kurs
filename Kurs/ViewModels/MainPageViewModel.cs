using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Kurs.Models;
using Kurs.Services;
using Kurs.Services.Alerts;
using Kurs.Views;

namespace Kurs.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private int _notificationCount;

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

        public MainPageViewModel()
        {
            LoadNotificationsCommand = new Command(async () => await LoadAsync());
            OpenNotificationsCommand = new Command(async () => await ShowNotificationsAsync());

            LoadNotificationsCommand.Execute(null);
        }

        public async Task LoadAsync()
        {
            NotificationCount = await NotificationService.GetNotificationCountAsync();
        }

        public async Task ShowNotificationsAsync()
        {
            var list = await NotificationService.GetPendingNotificationsAsync();

            if (list.Count == 0)
            {
                await AlertService.ShowInfo("Нет новых уведомлений.", "Уведомления");
                return;
            }

            var items = list.Select(w => $"{w.StartDate:dd.MM}–{w.EndDate:dd.MM} ({w.Status})");
            var choice = await AlertService.ChooseAsync("Ваши задачи", items);

            if (!string.IsNullOrWhiteSpace(choice) && choice != "Отмена")
                await NavigationService.NavigateToAsync(new SummaryPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
