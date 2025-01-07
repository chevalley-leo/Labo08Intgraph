using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Labo06; // Assurez-vous d'inclure les bons espaces de noms

namespace Labo08_Chevalley_Michaud.Views
{
    public partial class JobControl : UserControl
    {
        private BatchFileService batchFileService;
        private ObservableCollection<Batch> batches;

        public JobControl()
        {
            InitializeComponent();
            batchFileService = new BatchFileService();
            batches = new ObservableCollection<Batch>();
        }

        private void StartJobButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour démarrer un job
            MessageBox.Show("Le job a démarré !");
        }

        private void LoadJobButton_Click(object sender, RoutedEventArgs e)
        {
            // Charger la liste des batches depuis un fichier XML
            string filePath = @"path\to\your\batchfile.xml";  // Remplacez ce chemin par le fichier XML que vous voulez charger
            BatchList batchList = batchFileService.LoadBatchListFromFile(filePath);

            // Remplir les données dans le contrôle
            DisplayBatchInfo(batchList);
        }

        private void DisplayBatchInfo(BatchList batchList)
        {
            // Affiche les informations de tous les Batchs
            batches.Clear();
            foreach (var batch in batchList.GetBatches())
            {
                batches.Add(batch);
            }

            // Si vous voulez afficher la liste dans une ListBox :
            BatchListBox.ItemsSource = batches;  // Assurez-vous d'avoir une ListBox dans votre XAML
        }
    }
}
