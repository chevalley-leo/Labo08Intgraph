using System.Globalization;
using System.Windows;
using System.Xml;




namespace Labo06
{
    // Recipe class representing pigmentation of a bucket
    public class Recipe
    {
        public double PigmentA { get; set; }
        public double PigmentB { get; set; }
        public double PigmentC { get; set; }
        public double PigmentD { get; set; }

        private const int MaxTotalPigment = 100;

        public double Total => PigmentA + PigmentB + PigmentC + PigmentD;

        // Method to validate total pigment amount
        public bool ValidateTotalPigment()
        {
            double total = PigmentA + PigmentB + PigmentC + PigmentD;
            return total <= MaxTotalPigment;
        }

        // Method to write Recipe to XML
        public void XmlWrite(XmlWriter writer)
        {
            writer.WriteStartElement("Recipe");
            writer.WriteElementString("PigmentA", PigmentA.ToString("F2", CultureInfo.InvariantCulture)); // Formats to two decimals
            writer.WriteElementString("PigmentB", PigmentB.ToString("F2", CultureInfo.InvariantCulture));
            writer.WriteElementString("PigmentC", PigmentC.ToString("F2", CultureInfo.InvariantCulture));
            writer.WriteElementString("PigmentD", PigmentD.ToString("F2", CultureInfo.InvariantCulture));
            writer.WriteEndElement();
        }



        // Method to read Recipe from XML
        public void XmlRead(XmlReader reader)
        {
            try
            {
                // Ensure we are positioned at the correct node for each pigment
                if (reader.ReadToFollowing("PigmentA"))
                    PigmentA = double.Parse(reader.ReadElementContentAsString(), CultureInfo.InvariantCulture);

                if (reader.ReadToFollowing("PigmentB"))
                    PigmentB = double.Parse(reader.ReadElementContentAsString(), CultureInfo.InvariantCulture);

                if (reader.ReadToFollowing("PigmentC"))
                    PigmentC = double.Parse(reader.ReadElementContentAsString(), CultureInfo.InvariantCulture);

                if (reader.ReadToFollowing("PigmentD"))
                    PigmentD = double.Parse(reader.ReadElementContentAsString(), CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Error parsing pigment values: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
            }
        }

    }



}

