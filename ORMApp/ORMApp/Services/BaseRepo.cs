using System;
using System.Data.SQLite;
using System.IO;
using Dapper;

namespace NHibernate.Services
{
    public class BaseRepo
    {
        public static string DbFIle
        {
            get => Environment.CurrentDirectory + "\\..\\SimpleDb.sqlite";
        }

        public static SQLiteConnection DbConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFIle);
        }

        public static void CreateDatabase()
        {
            if (File.Exists(BaseRepo.DbFIle))
            {
                using (var cnn = DbConnection())
                {
                    cnn.Open();
                    cnn.Execute("DROP table Object;");
                    cnn.Execute("DROP table InternetClient;");
                    cnn.Execute("DROP table Client;");
                    cnn.Execute("DROP table PlaceOrder;");
                    cnn.Execute("DROP table OrderObject;");
                }
            }
            
            using (var cnn = DbConnection())
            {
                cnn.Open();
                cnn.Execute(@"create table Object
                (
                    ID                               INTEGER PRIMARY KEY AUTOINCREMENT,
                    Description                     varchar(100),
                    Price                           integer,
                    InStock                         integer
                )");
                
                cnn.Execute(@"create table Client
                (
                    ID                                  INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name                                varchar(100),
                    Address                             varchar(100)
                )");
                
                cnn.Execute(@"create table InternetClient
                (
                    ID                                  INTEGER PRIMARY KEY AUTOINCREMENT,
                    ClientId                            integer,
                    IpAddress                           varchar(100),
                    
                    FOREIGN KEY (ClientId) REFERENCES Client (ID)
                )");
                
                cnn.Execute(@"create table PlaceOrder
                (
                    ID                                  INTEGER PRIMARY KEY AUTOINCREMENT,
                    ClientId                            integer,
                    Completed                           integer,
                    
                    FOREIGN KEY (ClientId) REFERENCES Client (ID)
                )");
                
                cnn.Execute(@"create table OrderObject
                (
                    ID                                  INTEGER PRIMARY KEY AUTOINCREMENT,
                    Amount                              integer,
                    ObjectId                            integer,
                    OrderId                             integer,
                    IpAddress                           varchar(100),
                    
                    FOREIGN KEY (ObjectId) REFERENCES Object (ID),
                    FOREIGN KEY (OrderId) REFERENCES PlaceOrder (ID)
                )");
                
            }
        }
    }
}