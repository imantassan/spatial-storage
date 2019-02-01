using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace SpatialStorage.Data
{
    /// <summary>
    /// Provides methods used to load test data from a CSV file.
    /// </summary>
    public static class DataLoader
    {
        /// <summary>
        /// Reads all records from the file under the given <paramref name="filePath"/>.
        /// </summary>
        /// <typeparam name="TRecord">The type of the records within the file.</typeparam>
        /// <param name="filePath">Full path to the file containing the data.</param>
        /// <returns>Enumerator throughout all the records within the file.</returns>
        public static IEnumerable<TRecord> ReadAll<TRecord>(string filePath)
            where TRecord : class, new()
        {
            if (!File.Exists(filePath))
            {
                yield break;
            }

            using (var fileReader = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var streamReader = new StreamReader(fileReader))
            using (var csvReader = new CsvReader(streamReader))
            {
                csvReader.Read();
                csvReader.ReadHeader();

                while (csvReader.Read())
                {
                    yield return csvReader.GetRecord<TRecord>();
                }
            }
        }
    }
}