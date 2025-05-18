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
        //private async void OnEmployeesClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new EmployeePage());
        //}

        //private async void OnWorkTypesClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new WorkTypePage());
        //}

        //private async void OnAssignmentsClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new WorkAssignmentPage());
        //}

        //private async void OnSummaryClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new SummaryPage());
        //}
        //private async void OnLogoutClicked(object sender, EventArgs e)
        //{
        //    await App.Auth.LogoutAsync();
        //    Application.Current.MainPage = new NavigationPage(new LoginPage());
        //}
    }
}
