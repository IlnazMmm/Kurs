using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Services
{
    public static class NavigationService
    {
        public static Task NavigateToAsync(Page page)
        {
            var nav = Shell.Current?.Navigation ?? Application.Current?.MainPage?.Navigation;
            return nav != null
                ? nav.PushAsync(page)
                : Task.CompletedTask;
        }
    }
}
