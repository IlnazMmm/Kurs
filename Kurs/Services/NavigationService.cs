using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Services
{
    public static class NavigationService
    {
        [Obsolete("NavigateToAsync is deprecated, please use NavigateToRouteAsync instead.", true)]
        public static Task NavigateToAsync(Page page)
        {
            var nav = Shell.Current?.Navigation ?? Application.Current?.MainPage?.Navigation;
            return nav != null
                ? nav.PushAsync(page)
                : Task.CompletedTask;
        }

        public static Task NavigateToRouteAsync(string route)
        {
            if (string.IsNullOrWhiteSpace(route))
                return Task.CompletedTask;

            return Shell.Current.GoToAsync(route);
        }

        public static Task NavigateToRootRouteAsync(string route)
        {
            if (string.IsNullOrWhiteSpace(route))
                return Task.CompletedTask;
            return Shell.Current.GoToAsync($"//{route}");
        }
    }
}
