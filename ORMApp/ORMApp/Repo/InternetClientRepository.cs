using System.IO;
using System.Linq;
using Dapper;
using NHibernate.Model;
using NHibernate.Services;

namespace NHibernate.Repo
{
    public class InternetClientRepository
    {
        public void AddInternetClient(InternetClient InternetClient)
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

                int id = cnn.Execute(sql, InternetClient);

                sql = "INSERT INTO InternetClient (ClientId, IpAddress) Values (@ClientId, @IpAddress)";
                cnn.Execute(sql, new {ClientId = id, IpAddress = InternetClient.IpAddress,});
            }
        }
    }
}