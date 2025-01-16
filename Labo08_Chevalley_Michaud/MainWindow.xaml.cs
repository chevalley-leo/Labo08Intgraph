using System.Windows;
using Labo08_Chevalley_Michaud.Views;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace Labo08_Chevalley_Michaud
{
    public partial class MainWindow : Window
    {
        private string currentUserProfile = null;
        private readonly string passwordFilePath = "passwords.txt";


        public MainWindow()
        {
            InitializeComponent();
            //WriteDefaultPasswords(); // Appel de la méthode pour écrire les mots de passe
            LoadLoginPage();
        }

        private void LoadLoginPage()
        {
            // Charger la page de connexion
            var loginPage = new LoginControl();

            // Connecter l'événement de connexion
            loginPage.OnLoginSuccess += HandleLoginSuccess;

            // Afficher le contrôle de connexion
            Connexion.Content = loginPage;

            // Le reste de la page (Overview) ne sera pas affiché avant la connexion
            OverviewContent.Content = null;
            Job.Content = null;

            // Mettre à jour l'état de connexion
            TxtStatus.Text = "Non connecté";
        }

       /* private void WriteDefaultPasswords()
        {
            // Vérifie si le fichier existe déjà
            if (File.Exists(passwordFilePath))
                return;

            // Dictionnaire des mots de passe par défaut
            var defaultPasswords = new Dictionary<string, string>
            {
                { "Opérateur", "Operator" },
                { "Chef d'atelier", "Manager" },
                { "Administrateur", "A" }
            };

            // Hachage des mots de passe
            var hashedPasswords = new Dictionary<string, string>();
            foreach (var kvp in defaultPasswords)
            {
                string hashedPassword = HashPassword(kvp.Value);
                hashedPasswords[kvp.Key] = hashedPassword;
            }

            // Écriture dans le fichier
            using (var writer = new StreamWriter(passwordFilePath))
            {
                foreach (var kvp in hashedPasswords)
                {
                    writer.WriteLine($"{kvp.Key}:{kvp.Value}");
                }
            }
        }

        // Méthode pour hacher un mot de passe
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
       */

        private void LoadJobPage()
        {
            // Charger la page JobControl si l'utilisateur est autorisé
            var jobPage = new JobControl();
            Job.Content = jobPage;
        }

        private void HandleLoginSuccess(string profile)
        {
            currentUserProfile = profile;
            TxtStatus.Text = $"Connecté en tant que : {profile}";

            LoadHomePage();
        }

        private void LoadHomePage()
        {
            // Une fois connecté, on peut afficher la page Overview
            var overviewPage = new Overview();
            OverviewContent.Content = overviewPage;

            // Vérification du profil de l'utilisateur
            if (currentUserProfile == "Administrateur" || currentUserProfile == "Chef d'atelier")
            {
                // Si l'utilisateur est administrateur ou chef d'atelier, on lui permet d'accéder à JobControl
                LoadJobPage();

 
                // Désactiver le bouton Paramètres (Settings) pour l'opérateur
                SettingsButton.Visibility = Visibility.Visible;
            }
            else if (currentUserProfile == "Opérateur")
            {
                // Si l'utilisateur est un opérateur, on n'affiche que la page Overview
                Job.Content = null;  // On cache la page JobControl
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Arrêter le thread avant de fermer la fenêtre
            ThreadMachine.Instance.StopThread = false;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this; // Définit MainWindow comme propriétaire
            settingsWindow.ShowDialog();  // Ouvre la fenêtre en mode modal
        }

    }
}
