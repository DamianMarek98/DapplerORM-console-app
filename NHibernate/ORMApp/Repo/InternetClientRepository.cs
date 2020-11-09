using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using NHibernate.Model;
using NHibernate.Services;

namespace NHibernate.Repo
{
    public class InternetClientRepository : IClientRepository<InternetClient>
    {
        public InternetClient GetClient(int id)
        {
            if (!File.Exists(BaseRepo.DbFIle)) return null;

            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                var result = cnn.Query<InternetClient>(
                    @"SELECT * FROM InternetClient WHERE Id = @id", new {id}).FirstOrDefault();
                return result;
            }
        }

        public void AddClient(InternetClient client)
        {
            if (!File.Exists(BaseRepo.DbFIle))
            {
                BaseRepo.CreateDatabase();
            }

            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                string sql =
                    "INSERT INTO InternetClient (Name, Address, IpAddress) Values (@Name, @Address, @IpAddress);";

                cnn.Execute(sql, client);
            }
        }
        
        public List<InternetClient> GetAllClientsContaining(string str)
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
                    "SELECT * FROM InternetClient WHERE Name LIKE " + like + ";";

                return cnn.Query<InternetClient>(sql).ToList();
            }
        }
    }
}