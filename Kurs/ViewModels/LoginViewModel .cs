using Kurs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kurs.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            var success = await App.Auth.LoginAsync(Username, Password);
            if (success)
            {
                App.Current.MainPage = new NavigationPage(new MainPage());
                //await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Ошибка", "Неверный логин или пароль", "OK");
            }
        }
    }

}
