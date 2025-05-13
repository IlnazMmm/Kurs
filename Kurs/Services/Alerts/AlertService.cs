using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Services.Alerts
{
    public static class AlertService
    {
        public static Task ShowError(string message, string title = "Ошибка") =>
             Application.Current.MainPage.DisplayAlert(title, message, "ОК");

        public static Task ShowInfo(string message, string title = "Инфо") =>
            Application.Current.MainPage.DisplayAlert(title, message, "ОК");

        public static Task<bool> ConfirmAsync(string message, string title = "Подтверждение") =>
             Application.Current.MainPage.DisplayAlert(title, message, "Да", "Отмена");

        public static Task<string> PromptAsync(string message, string title = "Комментарий", string placeholder = "Введите текст") =>
            Application.Current.MainPage.DisplayPromptAsync(title, message, placeholder: placeholder, maxLength: 200);

        public static Task<string> ChooseAsync(string title, IEnumerable<string> options, string cancel = "Отмена")
        {
            return Application.Current.MainPage.DisplayActionSheet(title, cancel, null, options?.ToArray());
        }
    }
}
