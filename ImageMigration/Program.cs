using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Data.SqlClient;

class program
{
    static async Task Main(string[] args)
    {
        string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFeqNj1Hg==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";


        string sourceContainerName = "userprofile";
        string destinationContainerName = "images";

        string databaseConnectionString = "server=ZERO_BOOK_13; Database=POCIsummation; Integrated Security=true; TrustServerCertificate=True";

        BlobContainerClient sourceContainer = new BlobContainerClient(connectionString, sourceContainerName);
        BlobContainerClient destinationContainer = new BlobContainerClient(connectionString, destinationContainerName);
        await destinationContainer.CreateIfNotExistsAsync(PublicAccessType.Blob);
        var imageRecords = await GetImageRecordsFromDatabaseAsync(databaseConnectionString);

        foreach (var record in imageRecords)
        {
            try
            {
                string blobName = record.ImageName;
                BlobClient sourceBlob = sourceContainer.GetBlobClient(blobName);

                string folderName = record.ID.ToString();
                string newBlobName = $"{folderName}/{blobName}";
                BlobClient destinationBlob = destinationContainer.GetBlobClient(newBlobName);


                await destinationBlob.StartCopyFromUriAsync(sourceBlob.Uri);

                Console.WriteLine($"Migrated: {blobName} to {newBlobName}");

                await UpdateDatabaseAsync(databaseConnectionString, record.ID, newBlobName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error migrating {record.ImageName}: {ex.Message}");
            }
        }

        Console.WriteLine("Migration completed.");
    }


    static async Task<List<(int ID, string ImageName)>> GetImageRecordsFromDatabaseAsync(string connectionString)
    {
        var records = new List<(int, string)>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string query = "SELECT ID, profileimage FROM mst_user";
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32(0);
                    // string imageName = reader.GetString(1);
                    string imageName = reader.IsDBNull(1) ? null : reader.GetString(1);
                    records.Add((id, imageName));
                }
            }
        }

        return records;
    }

    static async Task UpdateDatabaseAsync(string connectionString, int id, string newBlobPath)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string query = "UPDATE mst_user SET profileimage = @NewPath WHERE ID = @ID"; 
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NewPath", newBlobPath);
                command.Parameters.AddWithValue("@ID", id);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}


