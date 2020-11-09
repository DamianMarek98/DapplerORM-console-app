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
                    cnn.Execute("DROP table PhysicalClient;");
                    cnn.Execute("DROP table InternetClient;");
                    
                }
            }
            
            using (var cnn = DbConnection())
            {
                cnn.Open();
                cnn.Execute(@"create table Object
                (
                    ID                                  INTEGER PRIMARY KEY AUTOINCREMENT,
                    Description                           varchar(100),
                    Price                            integer,
                    InStock                         integer
                )");
                
                cnn.Execute(@"create table PhysicalClient
                (
                    ID                                  INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name                                varchar(100),
                    Address                             varchar(100)
                )");
                
                cnn.Execute(@"create table InternetClient
                (
                    ID                                  INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name                                varchar(100),
                    Address                             varchar(100),
                    IpAddress                           varchar(100)
                )");
            }
        }
    }
}