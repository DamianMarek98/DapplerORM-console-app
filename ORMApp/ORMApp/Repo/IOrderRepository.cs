using NHibernate.Model;

namespace NHibernate.Repo
{
    public interface IOrderRepository
    {
        Order GetOrder(int id);
        void AddOrder(Order order);
    }
}