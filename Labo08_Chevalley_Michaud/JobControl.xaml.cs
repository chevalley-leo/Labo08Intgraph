using Labo06; // Assurez-vous d'inclure les bons espaces de noms
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Labo08_Chevalley_Michaud.Views
{
    public partial class JobControl : UserControl
    {
        private BatchFileService batchFileService;
        //private ObservableCollection<Batch> Batches;
        private const string BatchFilePath = "batches.xml";
        private int NextBatchID { get; set; } = 1;


        public JobControl()
        {
            InitializeComponent();
            SetupBatchListView();

            batchFileService = new BatchFileService();
            //Batches = new ObservableCollection<Batch>();

        }

        private void SetupBatchListView()
        {
            BatchListView.SelectionChanged += BatchListView_SelectionChanged;
            BatchListView.Items.Clear();
            BatchListView.ItemsSource = ThreadMachine.Instance.Baches;
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

            // Clear any previous batches
            ClearBatchListView();

            try
            {
                var batches = batchList.GetBatches();
                foreach (var batch in batches)
                {
                    // Access the Pigment values from the Recipe
                    var pigmentA = batch.Recipe.PigmentA;
                    var pigmentB = batch.Recipe.PigmentB;
                    var pigmentC = batch.Recipe.PigmentC;
                    var pigmentD = batch.Recipe.PigmentD;

                    // Calculate the resulting color for each batch
                    var resultingColor = CalculateResultingColor(pigmentA, pigmentB, pigmentC, pigmentD);

                    // Assign the resulting color to the Batch
                    batch.ResultingColor = resultingColor;  // Assign the color to ResultingColor

                }

                // Update the ListView to reflect changes
                BatchListView.ItemsSource = batches;
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
                    ThreadMachine.Instance.Baches.Add(batch);
                }
            }
        }
        private void ClearBatchListView()
        {
            BatchListView.ItemsSource = null;
            ThreadMachine.Instance.Baches.Clear();
        }
        private void BatchListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            bool hasSelection = BatchListView.SelectedItem != null;

        }


        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private SolidColorBrush CalculateResultingColor(double pigmentA, double pigmentB, double pigmentC, double pigmentD)
        {
            // Parse HEX colors to RGB
            var colorA = (R: 0x50, G: 0x8D, B: 0xC2); // #508DC2
            var colorB = (R: 0x84, G: 0xBC, B: 0x71); // #84BC71
            var colorC = (R: 0xF5, G: 0xE9, B: 0x09); // #F5E909
            var colorD = (R: 0xF3, G: 0x98, B: 0x14); // #F39814

            // Calculate the total weight
            double total = pigmentA + pigmentB + pigmentC + pigmentD;
            if (total == 0) total = 1; // Avoid division by zero

            // Weighted RGB calculation
            byte r = (byte)((pigmentA * colorA.R + pigmentB * colorB.R + pigmentC * colorC.R + pigmentD * colorD.R) / total);
            byte g = (byte)((pigmentA * colorA.G + pigmentB * colorB.G + pigmentC * colorC.G + pigmentD * colorD.G) / total);
            byte b = (byte)((pigmentA * colorA.B + pigmentB * colorB.B + pigmentC * colorC.B + pigmentD * colorD.B) / total);

            // Return the mixed color as a SolidColorBrush
            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }



    }
}
