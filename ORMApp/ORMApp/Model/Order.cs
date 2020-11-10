using System.Collections.Generic;
using NHibernate.Repo;

namespace NHibernate.Model
{
    public class Order
    {
        private int Id { get; set; }
        public int ClientId { get; set; }
        public bool Completed { get; set; }

        public List<OrderObject> Objects = new List<OrderObject>();

        public List<OrderObject> GetOrderObjects()
        {
            return Objects;
        }

        public int GetTotalPrice()
        {
            return OrderRepository.GetTotalPrice(Id);
        }

        public int GetNumberOfObjects()
        {
            return OrderRepository.GetNumberOfObjects(Id);
        }
    }
}