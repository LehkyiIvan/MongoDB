using SocialNetworkApp1.Services;
using System.Windows;

namespace SocialNetworkApp1
{
    public partial class LoginWindow : Window
    {
        private readonly DatabaseService _databaseService;

        public LoginWindow()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            var user = _databaseService.Login(email, password);

            if (user != null)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.SetUserId(user["userId"].AsString); 
                mainWindow.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid email or password.");
            }
        }
    }
}
