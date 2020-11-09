namespace NHibernate.Model
{
    public abstract class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public abstract int HowMuchOrdered();
    }
}