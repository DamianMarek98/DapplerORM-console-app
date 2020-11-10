using System;
using System.Collections.Generic;
using System.Configuration;
using NHibernate.Model;
using NHibernate.Repo;
using NHibernate.Services;

namespace NHibernate
{
    public class UserInterface
    {
        private Client _activeClient;
        private List<Client> _clients = new List<Client>();
        private readonly ClientRepository _clientRepository = new ClientRepository();

        public void Start()
        {
            while (true)
            {
                DiplayBasicMenu();

                var currentKey = Console.ReadKey(true);
                switch (currentKey.Key)
                {
                    case ConsoleKey.C:
                        ClientSelection();
                        break;
                    case ConsoleKey.X:
                        _activeClient = null;
                        break;
                    default:
                        Console.Out.WriteLine("You pressed: " + currentKey.KeyChar + " which does nothing!");
                        break;
                }
            }
        }

        private void ClientSelection()
        {
            var exit = false;
            var findLetters = "";
            while (!exit)
            {
                _clients = ClientService.GetAllClientsContaining(findLetters);
                int iter = 0;
                Console.WriteLine("10 Clients: (Filter string: "+ findLetters +")");
                foreach (var client in _clients)
                {
                    Console.Out.WriteLine(iter + ". name: " + client.Name);
                    iter++;
                }
                
                ConsoleKeyInfo currentKey = Console.ReadKey(true);
                switch (currentKey.Key)
                {
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                    case ConsoleKey.Backspace:
                        if (findLetters.Length > 0)
                        {
                            findLetters = findLetters.Remove(findLetters.Length - 1);
                        }
                        break;
                    default:
                        if (char.IsNumber(currentKey.KeyChar))
                        {
                            int num = (int) char.GetNumericValue(currentKey.KeyChar);
                            if (_clients.Count > num)
                            {
                                exit = true;
                                _activeClient = _clients[num];
                            }
                        }
                        findLetters += currentKey.KeyChar;
                        break;
                }
            }
        }
        
        
        private void DiplayBasicMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("Active client - " + ((_activeClient != null) ? _activeClient.Name : "not selected!"));
            Console.WriteLine("Press c for selecting active client");
            Console.WriteLine("Press x to unselect client");
        }
    }
}