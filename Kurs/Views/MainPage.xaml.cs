using Kurs.Enums;
using Kurs.Services;
using Kurs.Services.Alerts;
using Kurs.Views;

namespace Kurs.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            EmployeesButton.IsVisible = App.Auth.CanAccessFeature(AppFeature.ViewEmployees);
            WorkTypesButton.IsVisible = App.Auth.CanAccessFeature(AppFeature.ViewWorkTypes);
            AssignmentsButton.IsVisible = App.Auth.CanAccessFeature(AppFeature.AssignWork);
            SummaryButton.IsVisible = App.Auth.CanAccessFeature(AppFeature.ViewReports);
        }
        private async void OnEmployeesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EmployeePage());
        }

        private async void OnWorkTypesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WorkTypePage());
        }

        private async void OnAssignmentsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WorkAssignmentPage());
        }

        private async void OnSummaryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SummaryPage());
        }
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await App.Auth.LogoutAsync();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
