using System.Windows;
using System.Windows.Controls;

namespace Labo08_Chevalley_Michaud.Views
{
    public partial class LoginControl : UserControl
    {
        public event Action<string> OnLoginSuccess; // Pour informer la MainWindow

        // Mots de passe par défaut
        private readonly Dictionary<string, string> defaultPasswords = new()
        {
            { "Opérateur", "Operator" },
            { "Chef d'atelier", "Manager" },
            { "Administrateur", "Administrator" }
        };

        public LoginControl()
        {
            InitializeComponent();

        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string selectedProfile = ((ComboBoxItem)CmbProfiles.SelectedItem).Content.ToString();
            string inputPassword = PwdBox.Password;

            if (defaultPasswords.ContainsKey(selectedProfile) && defaultPasswords[selectedProfile] == inputPassword)
            {
                OnLoginSuccess?.Invoke(selectedProfile); // Informe MainWindow
            }
            else
            {
                MessageBox.Show("Mot de passe incorrect", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
