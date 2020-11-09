using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using NHibernate.Mapping;
using NHibernate.Model;
using NHibernate.Repo;

namespace NHibernate.Services
{
    public class ClientService
    {
        private readonly PhysicalClientRepository _physicalClientRepository = new PhysicalClientRepository();
        private readonly InternetClientRepository _internetClientRepository = new InternetClientRepository();

        public List<Tuple<Int64, string>> GetAllClientsWithNameContaining(string str)
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
                    "SELECT Id, Name FROM InternetClient WHERE Name LIKE " + like 
                     + " UNION ALL"                                                                           
                     + " SELECT Id, Name  From PhysicalClient  WHERE Name LIKE " + like 
                     + " ORDER BY Name"
                    + " LIMIT 10;";
                   

                return cnn.Query<Int64, string, Tuple<Int64, string>>(sql, System.Tuple.Create, splitOn: "*").AsList();
            }
            
            
        }
    }
}