using System;
using System.Collections.Generic;
using System.Configuration;
using NHibernate.Services;

namespace NHibernate
{
    public class UserInterface
    {
        private Tuple<Int64, string> _activeClient;
        private List<Tuple<Int64, string>> _clients = new List<Tuple<long, string>>();
        private ClientService _clientService = new ClientService();

        public void Start()
        {
            while (true)
            {
                DiplayBasicMenu();

                ConsoleKeyInfo currentKey = Console.ReadKey(true);
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
            bool exit = false;
            string findLetters = "";
            while (!exit)
            {
                _clients = _clientService.GetAllClientsWithNameContaining(findLetters);
                int iter = 0;
                Console.WriteLine("10 Clients: (Filter string: "+ findLetters +")");
                foreach (var client in _clients)
                {
                    Console.Out.WriteLine(iter + ". name: " + client.Item2);
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
            Console.WriteLine("Active client - " + ((_activeClient != null) ? _activeClient.Item2 : "not selected!"));
            Console.WriteLine("Press c for selecting active client");
            Console.WriteLine("Press x to unselect client");
        }
    }
}