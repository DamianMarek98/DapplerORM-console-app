using NHibernate.Model;

namespace NHibernate.Repo
{
    public interface IObjectRepository
    {
        Object GetObject(int id);
        void AddObject(Object obj);
        void IncreaseInStockAmount(int id, int number);
    }
}