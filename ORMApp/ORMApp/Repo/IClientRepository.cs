using System.Collections.Generic;
using NHibernate.Model;

namespace NHibernate.Repo
{
    public interface IClientRepository<T> where T : Client
    {
        T GetClient(int id);
        void AddClient(T client);

        List<T> GetAllClientsContaining(string str);
    }
}