using System.Windows;
using System.Windows.Controls;
using Labo08_Chevalley_Michaud.Views;

namespace Labo08_Chevalley_Michaud
{
    public partial class MainWindow : Window
    {
        private string currentUserProfile = null;

        public MainWindow()
        {
            InitializeComponent();
            LoadLoginPage();
        }

        private void LoadLoginPage()
        {
            var loginPage = new LoginControl();
            var OverviewPage = new Overview();
            loginPage.OnLoginSuccess += HandleLoginSuccess;
            MainContent.Content = loginPage;
            OverviewContent.Content = OverviewPage;
            Connexion.Content = loginPage;
            TxtStatus.Text = "Non connecté";
        }

        private void LoadJobPage()
        {
            var jobPage = new JobControl(); // Instancier JobControl
            Job.Content = jobPage;  // L'ajouter au ContentControl
        }


        private void HandleLoginSuccess(string profile)
        {
            currentUserProfile = profile;
            TxtStatus.Text = $"Connecté en tant que : {profile}";
            LoadHomePage();
        }

        private void LoadHomePage()
        {
            LoadJobPage();

            if (currentUserProfile == "Administrateur")
            {
                // Charge la page d'options si admin
              //  MainContent.Content = new OptionsPage();
            }
            else
            {
                // Charge une page d'accueil pour les autres profils
               // MainContent.Content = new HomePage();
            }
        }

<<<<<<< HEAD

=======
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ThreadMachine.Instance.StopThread=false;
        }
>>>>>>> 6deef6806d008a979bcd5b23228828e71aca1a31
    }
}
