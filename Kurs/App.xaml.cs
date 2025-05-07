using Kurs.Services;
using Kurs.Views;

namespace Kurs
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; private set; }
        public static AuthService Auth { get; private set; }

        public App()
        {
            InitializeComponent();
            Database = new DatabaseService();
            Auth = new AuthService(Database);
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override async void OnStart()
        {
            await Database.InitAsync();
        }
    }
}