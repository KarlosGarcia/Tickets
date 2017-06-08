using Tickets.Views;
using System.Threading.Tasks;

namespace Tickets.Services
{
    public class NavigationService
    {
        public async Task Navigate(string pageName)
        {
            switch (pageName)
            {
                case "ValidationPage":
                    await App.Current.MainPage.Navigation.PushAsync(new ValidationPage());
                    break;
                default:
                    break;
            }
        }

        public async Task Back()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }

    }
}
