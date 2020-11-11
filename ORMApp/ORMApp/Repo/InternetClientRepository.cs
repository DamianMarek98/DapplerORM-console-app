using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Dapper;
using NHibernate.Model;
using NHibernate.Services;

namespace NHibernate.Repo
{
    public class InternetClientRepository
    {
        public void AddInternetClient(InternetClient InternetClient)
        {
            if (!File.Exists(BaseRepo.DbFIle))
            {
                BaseRepo.CreateDatabase();
            }

            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                string sql =
                    "INSERT INTO Client (Name, Address) Values (@Name, @Address);";

                cnn.Execute(sql, InternetClient);
                SQLiteCommand Command = new SQLiteCommand("select last_insert_rowid()", cnn);
                Int64 LastRowID64 = (Int64) Command.ExecuteScalar();
                int id = (int) LastRowID64;

                sql = "INSERT INTO InternetClient (ClientId, IpAddress) Values (@ClientId, @IpAddress)";
                cnn.Execute(sql, new {ClientId = id, IpAddress = InternetClient.IpAddress,});
            }
        }
    }
}