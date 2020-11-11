using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using NHibernate.Services;
using Object = NHibernate.Model.Object;

namespace NHibernate.Repo
{
    public class ObjectRepository : IObjectRepository
    {
        public Object GetObject(int id)
        {
            if (!File.Exists(BaseRepo.DbFIle)) return null;

            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                var result = cnn.Query<Object>(
                    @"SELECT * FROM Object WHERE Id = @id", new {id}).FirstOrDefault();
                return result;
            }
        }

        public void AddObject(Object obj)
        {
            if (!File.Exists(BaseRepo.DbFIle))
            {
                BaseRepo.CreateDatabase();
            }

            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                string sql =
                    "INSERT INTO Object (Description, Price, InStock) Values (@Description, @Price, @InStock);";

                cnn.Execute(sql, obj);
            }
        }

        public void IncreaseInStockAmount(int id, int number)
        {
            var obj = GetObject(id);

            if (obj != null)
            {
                int newNumber = obj.InStock + number;
                using (var cnn = BaseRepo.DbConnection())
                {
                    cnn.Open();
                    string sql = "UPDATE Object SET InStock = @InStock WHERE Id = @Id";
                    cnn.Execute(sql, new {InStock = newNumber, Id = id});
                }
            }
        }

        public List<Object> GetAllObjects()
        {
            if (!File.Exists(BaseRepo.DbFIle))
            {
                BaseRepo.CreateDatabase();
            }

            var objects = new List<Object>();

            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                objects = cnn.Query<Object>("SELECT * FROM Object").ToList();
            }


            return objects;
        }
    }
}