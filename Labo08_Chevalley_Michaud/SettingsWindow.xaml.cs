using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Labo08_Chevalley_Michaud.Views
{
    public partial class SettingsWindow : Window
    {
        private const string PasswordFilePath = "passwords.txt";

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
            var passwords = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    passwords[parts[0]] = parts[1];
                }
            }

            if (passwords.ContainsKey("Opérateur"))
                PwdOperator.Password = passwords["Opérateur"];
            if (passwords.ContainsKey("Chef d'atelier"))
                PwdManager.Password = passwords["Chef d'atelier"];
            if (passwords.ContainsKey("Administrateur"))
                PwdAdmin.Password = passwords["Administrateur"];
        }

        private void SavePasswords_Click(object sender, RoutedEventArgs e)
        {
            var passwords = new Dictionary<string, string>
    {
        { "Opérateur", PasswordHasher.HashPassword(PwdOperator.Password) },
        { "Chef d'atelier", PasswordHasher.HashPassword(PwdManager.Password) },
        { "Administrateur", PasswordHasher.HashPassword(PwdAdmin.Password) }
    };

            using (var writer = new StreamWriter(PasswordFilePath))
            {
                foreach (var pair in passwords)
                {
                    writer.WriteLine($"{pair.Key}:{pair.Value}");
                }
            }

            MessageBox.Show("Les mots de passe ont été sauvegardés avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
