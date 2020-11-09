namespace NHibernate.Model
{
    public class InternetClient : Client
    {
        public string IpAddress { get; set; }
        
        public override int HowMuchOrdered()
        {
            throw new System.NotImplementedException();
        }
    }
}