using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Dapper;
using NHibernate.Model;
using NHibernate.Services;


namespace NHibernate.Repo
{
    public class OrderRepository : IOrderRepository
    {
        public Order GetOrder(int id)
        {
            if (!File.Exists(BaseRepo.DbFIle)) return null;

            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                var result = cnn.Query<Order>(
                    @"SELECT * FROM PlaceOrder WHERE Id = @id", new {id}).FirstOrDefault();
                return result;
            }
        }

        public void AddOrder(Order order)
        {
            if (!File.Exists(BaseRepo.DbFIle))
            {
                BaseRepo.CreateDatabase();
            }

            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                var sql =
                    "INSERT INTO PlaceOrder (ClientId, Completed) Values (@ClientId, @Completed);";

                cnn.Execute(sql, new {ClientId = order.ClientId, Completed = 0});

                SQLiteCommand Command = new SQLiteCommand("select last_insert_rowid()", cnn);
                Int64 LastRowID64 = (Int64) Command.ExecuteScalar();
                int orderId = (int) LastRowID64;

                sql = "INSERT INTO OrderObject (Amount, ObjectId, OrderId) Values (@Amount, @ObjectId, @OrderId);";
                foreach (var orderObj in order.Objects)
                {
                    cnn.Execute(sql, new {Amount = orderObj.Amount, ObjectId = orderObj.ObjectId, OrderId = orderId});
                }
            }
        }

        public List<Order> GetAllOrders()
        {
            if (!File.Exists(BaseRepo.DbFIle))
            {
                BaseRepo.CreateDatabase();
            }

            var orders = new List<Order>();

            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                var ordersWithoutProducts = cnn.Query<Order>("SELECT * FROM PlaceOrder").ToList();

                foreach (Order order in ordersWithoutProducts)
                {
                    order.Objects = cnn.Query<OrderObject>("SELECT * FROM OrderObject WHERE OrderId = @OrderId",
                            new {OrderId = order.Id})
                        .ToList();

                    orders.Add(order);
                }
            }

            return orders;
        }

        public static int GetNumberOfObjects(int orderId)
        {
            var amount = 0;

            if (!File.Exists(BaseRepo.DbFIle)) return amount;

            using (var cnn = BaseRepo.DbConnection())
            {
                var sql =
                    "SELECT SUM(Amount) FROM OrderObject WHERE OrderId = @OrderId";
                amount = (int) cnn.Query<Int64>(sql, new {orderId}).FirstOrDefault();
            }

            return amount;
        }

        public static int GetTotalPrice(int orderId)
        {
            var total = 0;

            if (!File.Exists(BaseRepo.DbFIle)) return total;

            using (var cnn = BaseRepo.DbConnection())
            {
                var sql =
                    "SELECT * FROM OrderObject WHERE OrderId = @OrderId";
                var objectRepository = new ObjectRepository();
                foreach (var orderObj in cnn.Query<OrderObject>(sql, new {orderId}).ToList())
                {
                    total += orderObj.Amount * objectRepository.GetObject(orderObj.ObjectId).Price;
                }
            }

            return total;
        }
    }
}