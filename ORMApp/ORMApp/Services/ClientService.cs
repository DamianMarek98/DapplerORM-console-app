using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        
        public static List<Client> GetAllClients()
        {
            if (!File.Exists(BaseRepo.DbFIle))
            {
                BaseRepo.CreateDatabase();
            }

            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();
                string sql =
                    "SELECT * FROM Client ORDER BY Name";
                   

                return cnn.Query<Client>(sql).AsList();
            }
        }

        public static InternetClient GetInternetClient(Client client)
        {
            if (!File.Exists(BaseRepo.DbFIle))
            {
                BaseRepo.CreateDatabase();
            }
            
            using (var cnn = BaseRepo.DbConnection())
            {
                cnn.Open();

                var ipAddress = cnn.Query<string>("SELECT IpAddress FROM InternetClient WHERE ClientId = @ClientId",
                    new {ClientId = client.Id}).FirstOrDefault();

                if (ipAddress != default)
                {
                    return Map(client, ipAddress);
                }

            }

            return null;
        }

        public static bool isInternetClient(Client client)
        {
            return GetInternetClient(client) != null;
        }

        private static InternetClient Map(Client client, string ip)
        {
            var internetClient = new InternetClient();
            internetClient.Id = client.Id;
            internetClient.Address = client.Address;
            internetClient.IpAddress = ip;
            internetClient.Name = client.Name;

            return internetClient;
        }
    }
}