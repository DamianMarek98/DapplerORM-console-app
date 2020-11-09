using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using NHibernate.Model;
using NHibernate.Services;

namespace NHibernate.Repo
{
    public class PhysicalClientRepository : IClientRepository<PhysicalClient>
    {
        public PhysicalClient GetClient(int id)
        {
            if (!File.Exists(BaseRepo.DbFIle)) return null;
            
            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                var result = cnn.Query<PhysicalClient>(
                    @"SELECT * FROM PhysicalClient WHERE Id = @id", new { id }).FirstOrDefault();
                return result;
            }
        }

        public void AddClient(PhysicalClient client)
        {
            if (!File.Exists(BaseRepo.DbFIle))
            {
                BaseRepo.CreateDatabase();
            }
            
            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                string sql =
                    "INSERT INTO PhysicalClient (Name, Address) Values (@Name, @Address);";

                cnn.Execute(sql, client);
            }
        }

        public List<PhysicalClient> GetAllClientsContaining(string str)
        {
            if (!File.Exists(BaseRepo.DbFIle))
            {
                BaseRepo.CreateDatabase();
            }
            
            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                string like = "'%" + str + "%'";
                string sql =
                    "SELECT * FROM PhysicalClient WHERE Name LIKE " + like + ";";

                return cnn.Query<PhysicalClient>(sql).ToList();
            }
        }
    }
}