using Tickets.Views;
using Xamarin.Forms;

namespace Tickets
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var mainPage = new NavigationPage(new IndexPage());
            mainPage.BarBackgroundColor = Color.Black;
            MainPage = mainPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
