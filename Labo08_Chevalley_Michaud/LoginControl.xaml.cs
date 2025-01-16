using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Labo08_Chevalley_Michaud.Views
{
    public partial class LoginControl : UserControl
    {
        public event Action<string> OnLoginSuccess; // Pour informer la MainWindow
        public event Action OnLogout; // Déclenché lors de la déconnexion
        private readonly Dictionary<string, string> defaultPasswords = new();

        public LoginControl()
        {
            InitializeComponent();
            LoadPasswordsFromFile("passwords.txt");
        }

        private void LoadPasswordsFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Fichier de mots de passe introuvable.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        defaultPasswords[parts[0]] = parts[1];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des mots de passe : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string selectedProfile = ((ComboBoxItem)CmbProfiles.SelectedItem).Content.ToString();
            string inputPassword = PwdBox.Password;
            string hashedInputPassword = PasswordHasher.HashPassword(inputPassword);

            if (defaultPasswords.ContainsKey(selectedProfile) && defaultPasswords[selectedProfile] == hashedInputPassword)
            {
                OnLoginSuccess?.Invoke(selectedProfile); // Informe MainWindow
                BtnLogin.Visibility = Visibility.Collapsed;
                BtnLogout.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Mot de passe incorrect", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            // Réinitialiser les champs et réafficher le formulaire de connexion
            BtnLogin.Visibility = Visibility.Visible;
            BtnLogout.Visibility = Visibility.Collapsed;
            PwdBox.Clear();
            CmbProfiles.SelectedIndex = 0;
            LoadPasswordsFromFile("passwords.txt"); // Recharger les mots de passe

            OnLogout?.Invoke(); // Informe MainWindow de la déconnexion
        }


    }
}
