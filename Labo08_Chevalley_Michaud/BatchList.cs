using System.Windows;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Labo06
{
    public class BatchList
    {
        private List<Batch> batches = new List<Batch>();

        // Add a batch to the list
        public void AddBatch(Batch batch)
        {
            batches.Add(batch);
        }

        // Get the list of batches
        public IEnumerable<Batch> GetBatches()
        {
            return batches;
        }

        // Convert the batch list to an observable collection
        public static BatchList FromObservableCollection(ObservableCollection<Batch> observableBatches)
        {
            BatchList batchList = new BatchList();
            foreach (var batch in observableBatches)
            {
                batchList.AddBatch(batch);
            }
            return batchList;
        }

        // Convert the batch list to an observable collection
        public void XmlWrite(XmlWriter writer)
        {
            writer.WriteStartElement("BatchList");

            foreach (var batch in batches)
            {
                batch.XmlWrite(writer);
            }

            writer.WriteEndElement();
        }

        // Read the batch list from an XML reader
        public void XmlRead(XmlReader reader)
        {
            try
            {
                if (reader.ReadToFollowing("Batch"))
                {
                    do
                    {
                        Batch batch = new Batch();
                        batch.XmlRead(reader);
                        batches.Add(batch);

                    } while (reader.ReadToFollowing("Batch"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading BatchList: {ex.Message}");
            }
        }
    }
}
