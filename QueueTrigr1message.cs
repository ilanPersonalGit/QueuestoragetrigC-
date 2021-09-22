using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Queuestorage1
{
     public class complaint
    {
         public int productid { get; set; }
         public string name { get; set; }
         public int complaintcount { get; set; }
    }
    public static class QueueTrigr1message
    {
        [FunctionName("QueueTrigr1message")]
        public static void Run([QueueTrigger("complaintq", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            string[] QList = myQueueItem.Split(","); 

            MongoClient dbClient = new MongoClient("mongodb://ecommercemongo123:Y9atvLlbzO9HFz6o7EZ3HMtQQVzoHRa8wvys61LhLy7AFAtlnapMrqyIuWPeW1HgNy9WgZ9AlL3CIk4L18Vfvw==@ecommercemongo123.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@ecommercemongo123@");

            //var dbList = dbClient.ListDatabases().ToList();

            Console.WriteLine("The list of databases on this server is: ");
            var document = new BsonDocument{
                { "productid", QList[0] },
                 { "name", QList[1] },
                 { "complaintcount", 1 }
            };

              var database = dbClient.GetDatabase("organic");
            var collection = database.GetCollection<BsonDocument>("complaint1");
           
                 var filter = Builders<BsonDocument>.Filter.Eq("productid", QList[0]);
               
                 var fltcomplaint = collection.Find(filter).FirstOrDefault();
                 
                 if (fltcomplaint != null)             
               {
                    var doc = collection.Find(filter);
                  var size = doc.CountDocuments();
                var update = Builders<BsonDocument>.Update.Set("complaintcount", size+1);
                collection.UpdateOne(filter, update);
               }
               else
               {
                   collection.InsertOne(document);
               }


             
            
        }
    }
    
}
