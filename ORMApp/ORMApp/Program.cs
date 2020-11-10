using NHibernate.Model;
using NHibernate.Repo;
using NHibernate.Services;

namespace NHibernate
{
    class Program
    {
        public static void Main(string[] args)
        {
            BaseRepo.CreateDatabase();
            PrepareBasicData();
            new UserInterface().Start();
        }

        private static void PrepareBasicData()
        {
            var internetClientRepository = new InternetClientRepository();
            var clientRepository = new ClientRepository();
            var objectRepository = new ObjectRepository();
            var obj = new Object();
            obj.Description = "Kabel usb";
            obj.Price = 2;
            obj.InStock = 1000;
            objectRepository.AddObject(obj);
            obj = new Object();
            obj.Description = "Pilot";
            obj.Price = 10;
            obj.InStock = 100;
            objectRepository.AddObject(obj);
            obj = new Object();
            obj.Description = "Tv 40 cali";
            obj.Price = 1000;
            obj.InStock = 15;
            objectRepository.AddObject(obj);
            obj = new Object();
            obj.Description = "Karta graficzna gtx 1060TI";
            obj.Price = 800;
            obj.InStock = 8;
            objectRepository.AddObject(obj);
            obj = new Object();
            obj.Description = "Macbook pro 16 cali";
            obj.Price = 4500;
            obj.InStock = 3;
            objectRepository.AddObject(obj);
            obj = new Object();
            obj.Description = "Mointor Dell 23,8 cala";
            obj.Price = 550;
            obj.InStock = 5;
            objectRepository.AddObject(obj);
            obj = new Object();
            obj.Description = "Słuchawki nauszne sony";
            obj.Price = 139;
            obj.InStock = 12;
            objectRepository.AddObject(obj);
            obj = new Object();
            obj.Description = "Bateria AAA";
            obj.Price = 1;
            obj.InStock = 250;
            objectRepository.AddObject(obj);
            
            var client = new Client();
            client.Address = "Wieszczycka 2";
            client.Name = "Zbyszek";
            clientRepository.AddClient(client);
            client = new Client();
            client.Address = "Pokoleń 2";
            client.Name = "Andrzej";
            clientRepository.AddClient(client);
            client = new Client();
            client.Address = "Jelitkowska 2";
            client.Name = "Krzysztof";
            clientRepository.AddClient(client);
            client = new Client();
            client.Address = "Cietrzewia";
            client.Name = "Maciej W";
            clientRepository.AddClient(client);
            client = new Client();
            client.Address = "Pokoleń 13";
            client.Name = "Adrian";
            clientRepository.AddClient(client);
            client = new Client();
            client.Address = "Grunwaldzka 123/3A";
            client.Name = "Dominik M";
            clientRepository.AddClient(client);
            
            var internetClient = new InternetClient();
            internetClient.IpAddress = "123.231.23.2";
            internetClient.Name = "Wiesław a";
            internetClient.Address = "Częstochowska 1";
            internetClientRepository.AddInternetClient(internetClient);
            internetClient = new InternetClient();
            internetClient.IpAddress = "123.222.24.2";
            internetClient.Name = "katarzyna w";
            internetClient.Address = "Głuszcza 12";
            internetClientRepository.AddInternetClient(internetClient);
            internetClient = new InternetClient();
            internetClient.IpAddress = "234.222.21.2";
            internetClient.Name = "Martyna K";
            internetClient.Address = "Jana Pawła II 23/2";
            internetClientRepository.AddInternetClient(internetClient);
            internetClient = new InternetClient();
            internetClient.IpAddress = "111.222.11.12";
            internetClient.Name = "katarzyna w";
            internetClient.Address = "Głuszcza 12";
            internetClientRepository.AddInternetClient(internetClient);
            internetClient = new InternetClient();
            internetClient.IpAddress = "142.122.24.11";
            internetClient.Name = "Tomasz K";
            internetClient.Address = "Do studzienki";
            internetClientRepository.AddInternetClient(internetClient);
            internetClient = new InternetClient();
            internetClient.IpAddress = "102.100.10.10";
            internetClient.Name = "Zuzanna K";
            internetClient.Address = "Marynarki polskiej 11/12";
            internetClientRepository.AddInternetClient(internetClient);
        }
    }
}