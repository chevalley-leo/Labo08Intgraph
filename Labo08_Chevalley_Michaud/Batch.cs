using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace Labo06
{
    // Batch class representing a production batch
    public class Batch
    {
        public int ID { get; set; }
        public Recipe Recipe { get; set; } = new Recipe();
        public int BucketCount { get; set; }

        public SolidColorBrush ResultingColor { get; set; }
        public string RecipeSummary =>
            $"A: {Recipe.PigmentA}, B: {Recipe.PigmentB}, C: {Recipe.PigmentC}, D: {Recipe.PigmentD}";

        // Constructor
        public void XmlWrite(XmlWriter writer)
        {
            writer.WriteStartElement("Batch");
            writer.WriteElementString("ID", ID.ToString(CultureInfo.InvariantCulture)); // Write ID
            writer.WriteElementString("BucketCount", BucketCount.ToString(CultureInfo.InvariantCulture));
            Recipe.XmlWrite(writer); // Write Recipe details
            writer.WriteEndElement();
        }

        // Read batch data from XML
        public void XmlRead(XmlReader reader)
        {
            try
            {
                // Read ID for this batch
                if (reader.ReadToFollowing("ID"))
                    ID = int.Parse(reader.ReadElementContentAsString());

                // Read BucketCount for this batch
                if (reader.ReadToFollowing("BucketCount"))
                    BucketCount = int.Parse(reader.ReadElementContentAsString());

                // Read the Recipe section
                if (reader.ReadToFollowing("Recipe"))
                    Recipe.XmlRead(reader);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading batch data: {ex.Message}");
            }
        }
    }



}

