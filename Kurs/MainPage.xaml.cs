using Kurs.Views;

namespace Kurs
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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
    }
}
