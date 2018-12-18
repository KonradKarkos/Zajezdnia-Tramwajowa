using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Zajezdnia_Tramwajowa.MongoDB
{
    public class MongoDBClient
    {
        private static MongoClient Client { get; set; }
        private static MongoDatabaseBase DataBase { get; set; }
        private static MongoCollectionBase<ErrorMessage> Collection { get; set; }

        public static void Initialize()
        {
            Client = new MongoClient();
            DataBase = (MongoDatabaseBase)Client.GetDatabase("ZajezdniaTramwajowaErrors");
            Collection = (MongoCollectionBase<ErrorMessage>)DataBase.GetCollection<ErrorMessage>("ErrorMessage");
        }

        public static void InsertError(ErrorMessage ErrorM)
        {
            Collection.InsertOne(ErrorM);
        }
    }

    public class ErrorMessage
    {
        public String Message { get; set; }
        public DateTime Date { get; set; }

        public ErrorMessage(string message, DateTime date)
        {
            Message = message;
            Date = date;
        }
    }
}