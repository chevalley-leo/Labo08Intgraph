using System;
using System.Windows;
using Microsoft.Win32;

namespace Labo06
{
    public class FileDialogService
    {
        // Open a file dialog and return the selected file path
        public string OpenFile(string filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*")
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = filter
            };

            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }

        // Show a dialog asking the user if they want to save changes
        public bool ShowSaveChangesDialog()
        {
            var result = MessageBox.Show(
                "Vous avez des modifications non sauvegardées. Voulez-vous les sauvegarder avant d'ouvrir un autre fichier ?",
                "Modifications non sauvegardées",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning);

            return result != MessageBoxResult.Cancel;
        }

        // Show a save file dialog and return the selected file path
        public string ShowSaveFileDialog(string filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*")
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = filter
            };

            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }
    }
}
