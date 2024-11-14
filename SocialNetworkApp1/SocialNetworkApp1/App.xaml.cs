using System.Windows;

namespace SocialNetworkApp1
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Відкриваємо LoginWindow спочатку
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
