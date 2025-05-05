using Kurs.Services;

namespace Kurs
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; private set; }

        public App()
        {
            InitializeComponent();
            Database = new DatabaseService();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override async void OnStart()
        {
            await Database.InitAsync();
        }
    }
}