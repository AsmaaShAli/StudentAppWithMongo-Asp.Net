using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace WebApplicationMongoDb.Models
{
    public class MongoHelper
    {
        public static IMongoClient Client {get; set;}
        public static IMongoDatabase database { get; set; }

        public static string MongoConnection = "mongodb+srv://dbUser:dbPassword@cluster0.pt2i0.mongodb.net/<dbname>?retryWrites=true&w=majority";
        public static string MongoDatabase = "DemoDbWithMongo";

        public static IMongoCollection<Models.Student> student_collection { get; set; }
        public static void ConnectToMongoService ()
        {
            try
            {
                Client = new MongoClient(MongoConnection);
                database = Client.GetDatabase(MongoDatabase);
            } catch (Exception)
            {
                throw;
            }
        }

    }
}