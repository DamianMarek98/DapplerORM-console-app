using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using NHibernate.Model;
using NHibernate.Services;

namespace NHibernate.Repo
{
    public class ClientRepository : IClientRepository
    {
        public Client GetClient(int id)
        {
            if (!File.Exists(BaseRepo.DbFIle)) return null;

            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                var result = cnn.Query<Client>(
                    @"SELECT * FROM Client WHERE Id = @id", new {id}).FirstOrDefault();
                return result;
            }
        }

        public void AddClient(Client client)
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

                cnn.Execute(sql, client);
            }
        }
        
    }
}