using System.Collections.Generic;
using System.IO;
using Dapper;
using NHibernate.Model;

namespace NHibernate.Services
{
    public class ClientService
    {
        public static List<Client> GetAllClientsContaining(string str)
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
                    "SELECT * FROM Client WHERE Name LIKE " + like
                    + " ORDER BY Name"
                    + " LIMIT 10;";
                   

                return cnn.Query<Client>(sql).AsList();
            }
        }
    }
}