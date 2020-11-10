namespace NHibernate.Model
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int HowMuchOrdered()
        {
            return 0;
        }
    }
}