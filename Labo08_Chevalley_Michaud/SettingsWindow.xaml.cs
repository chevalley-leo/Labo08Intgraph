using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Labo08_Chevalley_Michaud.Views
{
    public partial class SettingsWindow : Window
    {
        private const string PasswordFilePath = "passwords.txt";
        private Dictionary<string, string> storedHashedPasswords = new();

        public SettingsWindow()
        {
            InitializeComponent();
            LoadPasswords();
        }

        private void LoadPasswords()
        {
            if (!File.Exists(PasswordFilePath))
                return;

            var lines = File.ReadAllLines(PasswordFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    storedHashedPasswords[parts[0]] = parts[1];
                }
            }

            // Password fields are left empty for security reasons.
        }

        private void SavePasswords_Click(object sender, RoutedEventArgs e)
        {
            var newPasswords = new Dictionary<string, string>
    {
        { "Opérateur", PwdOperator.Password },
        { "Chef d'atelier", PwdManager.Password },
        { "Administrateur", PwdAdmin.Password }
    };

            int changesCount = 0;

            foreach (var role in new List<string> { "Opérateur", "Chef d'atelier", "Administrateur" })
            {
                if (!string.IsNullOrEmpty(newPasswords[role]))
                {

                        storedHashedPasswords[role] = PasswordHasher.HashPassword(newPasswords[role]);
                        changesCount++; // Incrémente le compteur de changements
                   
                }
            }

            if (changesCount > 0)
            {
                // Mise à jour du fichier de mots de passe
                using (var writer = new StreamWriter(PasswordFilePath))
                {
                    foreach (var pair in storedHashedPasswords)
                    {
                        writer.WriteLine($"{pair.Key}:{pair.Value}");
                    }
                }

                // Affichage du message approprié en fonction du nombre de changements
                string message = changesCount == 1
                    ? "Le mot de passe a été changé."
                    : "Les mots de passe ont été changés.";
                MessageBox.Show(message, "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // Ferme la fenêtre de paramètres
                this.Close();
            }
            else
            {
                MessageBox.Show("Aucun changement détecté dans les mots de passe.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
