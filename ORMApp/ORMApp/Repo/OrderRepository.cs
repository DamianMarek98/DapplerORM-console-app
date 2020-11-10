using System;
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
                    @"SELECT * FROM Order WHERE Id = @id", new {id}).FirstOrDefault();
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
                    "INSERT INTO Order (ClientId, Completed) Values (@ClientId, @Completed);";

                var orderId = cnn.Execute(sql, new {ClientId = order.ClientId, Completed = 0});
                
                sql = "INSERT INTO OrderObject (Amount, ObjectId, OrderId) Values (@Amount, @ObjectId, @OrderId);";
                foreach (var orderObj in order.Objects)
                {
                    cnn.Execute(sql, new {Amount = orderObj.Amount, ObjectId = orderObj.ObjectId, OrderId = orderId});
                }
            }
        }

        public static int GetNumberOfObjects(int orderId)
        {
            var amount = 0;

            if (!File.Exists(BaseRepo.DbFIle)) return amount;

            using (var cnn = BaseRepo.DbConnection())
            {
                var sql =
                    "INSERT SUM(Amount) FROM OrderObject WHERE OrderId = @OrderId";
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
                    "INSERT * FROM OrderObject WHERE OrderId = @OrderId";
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