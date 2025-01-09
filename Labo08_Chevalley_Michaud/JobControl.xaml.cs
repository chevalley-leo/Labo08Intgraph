using Labo06; // Assurez-vous d'inclure les bons espaces de noms
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace Labo08_Chevalley_Michaud.Views
{
    public partial class JobControl : UserControl
    {
        private BatchFileService batchFileService;
        private ObservableCollection<Batch> Batches;
        private const string BatchFilePath = "batches.xml";
        private int NextBatchID { get; set; } = 1;


        public JobControl()
        {
            InitializeComponent();
            SetupBatchListView();

            batchFileService = new BatchFileService();
            Batches = new ObservableCollection<Batch>();

        }

        private void SetupBatchListView()
        {
            BatchListView.SelectionChanged += BatchListView_SelectionChanged;
            BatchListView.Items.Clear();
            BatchListView.ItemsSource = Batches;
        }


        private void StartJobButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour démarrer un job
            MessageBox.Show("Le job a démarré !");
        }

        private void LoadJobButton_Click(object sender, RoutedEventArgs e)
        {
            // Créez une instance de FileDialogService pour ouvrir le dialogue de sélection de fichier
            var fileDialogService = new FileDialogService();

            // Ouvrir le dialogue de sélection de fichier et obtenir le chemin du fichier sélectionné
            string filePath = fileDialogService.OpenFile("XML Files (*.xml)|*.xml|All Files (*.*)|*.*");

            // Vérifiez si un fichier a été sélectionné
            if (!string.IsNullOrEmpty(filePath))
            {
                // Créez une instance de BatchFileService pour charger le fichier
                var batchFileService = new BatchFileService();

                // Charger la liste des batches depuis le fichier sélectionné
                BatchList batchList = batchFileService.LoadBatchListFromFile(filePath);

                // Vérifiez si la liste des batches est bien chargée
                if (batchList != null)
                {
                    // Remplir les données dans le contrôle (ou autre affichage)
                    DisplayBatchInfo(batchList);
                }
                else
                {
                    MessageBox.Show("Impossible de charger les données du fichier sélectionné.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Aucun fichier sélectionné.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void DisplayBatchInfo(BatchList batchList)
        {
            if (batchList == null || batchList.GetBatches() == null)
            {
                ShowErrorMessage("La liste des batches est vide ou nulle.");
                return;
            }


            Batches = Batches ?? [];

            ClearBatchListView();

            try
            {
                LoadBatches(batchList.GetBatches());
                NextBatchID = Batches.Max(batch => batch.ID) + 1;
                BatchListView.ItemsSource = Batches;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Une erreur est survenue lors du chargement des batches : {ex.Message}");
            }
        }

        private void LoadBatches(IEnumerable<Batch> batches)
        {
            foreach (var batch in batches)
            {
                if (batch != null)
                {
                    Batches.Add(batch);
                }
            }
        }
        private void ClearBatchListView()
        {
            BatchListView.ItemsSource = null;
            Batches.Clear();
        }
        private void BatchListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            bool hasSelection = BatchListView.SelectedItem != null;

        }


        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }


    }
}
