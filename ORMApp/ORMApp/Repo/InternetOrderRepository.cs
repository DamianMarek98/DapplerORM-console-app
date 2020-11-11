using System;
using System.Data.SQLite;
using System.IO;
using Dapper;
using NHibernate.Model;
using NHibernate.Services;

namespace NHibernate.Repo
{
    public class InternetOrderRepository
    {
        public void AddOrder(InternetOrder order)
        {
            if (!File.Exists(BaseRepo.DbFIle))
            {
                BaseRepo.CreateDatabase();
            }

            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                string sql =
                    "INSERT INTO Order (ClientId, Completed, IpAddress) Values (@ClientId, @Completed, @IpAddress);";

                cnn.Execute(sql,
                    new {ClientId = order.ClientId, Completed = 0, IpAddress = order.IpAddress});
                
                SQLiteCommand Command = new SQLiteCommand("select last_insert_rowid()", cnn);
                Int64 LastRowID64 = (Int64)Command.ExecuteScalar();
                int orderId = (int) LastRowID64;


                sql = "INSERT INTO OrderObject (Amount, ObjectId, OrderId) Values (@Amount, @ObjectId, @OrderId);";
                foreach (var orderObj in order.Objects)
                {
                    cnn.Execute(sql, new {Amount = orderObj.Amount, ObjectId = orderObj.ObjectId, OrderId = orderId});
                }
            }
        }
    }
}