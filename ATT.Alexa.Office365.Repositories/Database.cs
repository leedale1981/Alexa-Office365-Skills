using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace ATT.Alexa.Office365.Repositories
{
    public class Database<T> where T : Models.Document
    {
        private string collectionName = string.Empty;
        private readonly string databaseName = WebConfigurationManager.AppSettings["alexa:DatabaseName"];
        private readonly string endpointUri = WebConfigurationManager.AppSettings["alexa:EndpointUri"];
        private readonly string primaryKey = WebConfigurationManager.AppSettings["alexa:PrimaryKey"];
        private DocumentClient client = null;

        public Database(string collectionNameKey)
        {
            this.collectionName = WebConfigurationManager.AppSettings[collectionNameKey];
            this.client = new DocumentClient(new Uri(endpointUri), primaryKey);

            Task.Factory.StartNew(async () => 
            {
                await this.CreateDatabaseIfNotExists(databaseName);
                await this.CreateDocumentCollectionIfNotExists(databaseName, collectionName);
            });
        }

        private async Task CreateDatabaseIfNotExists(string databaseName)
        {
            try
            {
                await this.client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName));
            }
            catch (DocumentClientException exception)
            {
                if (exception.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDatabaseAsync(new Microsoft.Azure.Documents.Database { Id = databaseName });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateDocumentCollectionIfNotExists(string databaseName, string collectionName)
        {
            try
            {
                await this.client.ReadDocumentCollectionAsync(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName));
            }
            catch (DocumentClientException exception)
            {
                if (exception.StatusCode == HttpStatusCode.NotFound)
                {
                    DocumentCollection collectionInfo = new DocumentCollection();
                    collectionInfo.Id = collectionName;
                    collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

                    await this.client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(databaseName),
                        collectionInfo,
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<T> GetDocument(T document)
        {
            try
            {
                var response = await this.client.ReadDocumentAsync(
                    UriFactory.CreateDocumentUri(databaseName, collectionName, document.Id));

                return JsonConvert.DeserializeObject<T>(response.Resource.ToString());
            }
            catch (DocumentClientException)
            {
                return null;
            }
        }

        public async Task<bool> UpdateDocument(T document)
        {
            try
            {
                var response = await this.client.UpsertDocumentAsync(
                    UriFactory.CreateDocumentUri(databaseName, collectionName, document.Id), document);

                return true;
            }
            catch (DocumentClientException)
            {
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAllDocuments()
        {
            try
            {
                var db = (await client.ReadDatabaseFeedAsync()).Single(d => d.Id == databaseName);
                var col = (await client.ReadDocumentCollectionFeedAsync(db.CollectionsLink)).Single(c => c.Id == collectionName);
                var docs = client.CreateDocumentQuery<T>(col.DocumentsLink).ToList();

                return docs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<T> CreateDocument(T document)
        {
            try
            {
                var response = await this.client.CreateDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), document);

                return JsonConvert.DeserializeObject<T>(response.Resource.ToString());
            }
            catch (DocumentClientException exception)
            {
                // Log this error.
            }

            return null;
        }
    }
}
