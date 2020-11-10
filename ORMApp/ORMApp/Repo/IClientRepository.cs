using System.Collections.Generic;
using NHibernate.Model;

namespace NHibernate.Repo
{
    public interface IClientRepository
    {
        Client GetClient(int id);
        void AddClient(Client client);
        
        
    }
}