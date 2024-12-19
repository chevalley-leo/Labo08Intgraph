using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Collections.ObjectModel;

namespace Labo06
{
    public class BatchFileService

    {
        // Save the batch list to a file
        public void SaveBatchListToFile(BatchList batchList, string filePath)
        {

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineOnAttributes = false,
                NewLineChars = "\r\n", 
                OmitXmlDeclaration = false 
            };

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                batchList.XmlWrite(writer);
            }
        }

        // Load the batch list from a file
        public BatchList LoadBatchListFromFile(string filePath)
        {
            BatchList batchList = new BatchList();
            if (File.Exists(filePath))
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    batchList.XmlRead(reader);
                }
            }
            return batchList;
        }

        // Save the current batch list to a file
        public void SaveCurrentBatchList(ObservableCollection<Batch> batches, string filePath)
        {
            BatchList batchList = BatchList.FromObservableCollection(batches);
            SaveBatchListToFile(batchList, filePath);

        }

        // Load the batch list from a file and convert it to an observable collection
        public BatchList ConvertObservableCollectionToBatchList(ObservableCollection<Batch> batches)
        {
            BatchList batchList = new BatchList();
            foreach (var batch in batches)
            {
                batchList.AddBatch(batch);
            }
            return batchList;
        }
    }

}
