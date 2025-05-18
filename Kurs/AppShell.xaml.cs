using Kurs.Enums;
using Kurs.Views;

namespace Kurs
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            ApplyRoleAccess();
            Routing.RegisterRoute(nameof(EmployeePage), typeof(EmployeePage));
            Routing.RegisterRoute(nameof(WorkTypePage), typeof(WorkTypePage));
            Routing.RegisterRoute(nameof(WorkAssignmentPage), typeof(WorkAssignmentPage));
            Routing.RegisterRoute(nameof(SummaryPage), typeof(SummaryPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
        private void ApplyRoleAccess()
        {
            EmployeesButton.IsVisible = App.Auth.CanAccessFeature(AppFeature.ViewEmployees);
            WorkTypesButton.IsVisible = App.Auth.CanAccessFeature(AppFeature.ViewWorkTypes);
            AssignmentsButton.IsVisible = App.Auth.CanAccessFeature(AppFeature.AssignWork);
            ReportsButton.IsVisible = App.Auth.CanAccessFeature(AppFeature.ViewReports);
        }
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await App.Auth.LogoutAsync();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
