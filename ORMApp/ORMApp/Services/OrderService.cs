using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using NHibernate.Model;
using NHibernate.Repo;

namespace NHibernate.Services
{
    public class OrderService
    {
        public static bool placeOrder(Order order)
        {
            if (!File.Exists(BaseRepo.DbFIle))
            {
                BaseRepo.CreateDatabase();
            }
            

            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                using (var transaction = cnn.BeginTransaction())
                {
                    var objectRepository = new ObjectRepository();
                    foreach (var orderObject in order.Objects)
                    {
                        var obj = cnn.Query<Object>(
                            @"SELECT * FROM Object WHERE Id = @id", new { id = orderObject.ObjectId }).FirstOrDefault();
                        if (obj.InStock < orderObject.Amount)
                        {
                            transaction.Rollback();
                            return false;
                        }
                        else
                        {
                            cnn.Execute(@"UPDATE Object SET InStock = @InStock WHERE ID = @Id;",
                                new {InStock = obj.InStock - orderObject.Amount, Id = obj.Id});
                        }
                    }
		
                    cnn.Execute(@"UPDATE PlaceOrder SET Completed = @Completed WHERE ID = @Id;",
                        new {Completed = 1, Id = order.Id});
                    transaction.Commit();
                }
            }

            return true;
        }
    }
}