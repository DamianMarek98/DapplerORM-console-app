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
        private readonly OrderRepository _orderRepository = new OrderRepository();
        private readonly ObjectRepository _objectRepository = new ObjectRepository();

        public void Start()
        {
            while (true)
            {
                DisplayBasicMenu();

                var currentKey = Console.ReadKey(true);
                switch (currentKey.Key)
                {
                    case ConsoleKey.C:
                        ClientSelection();
                        break;
                    case ConsoleKey.X:
                        _activeClient = null;
                        break;
                    case ConsoleKey.P:
                        if (_activeClient != null)
                        {
                            ClientOrderCreator();
                        }
                        break;
                    case ConsoleKey.O:
                        if (_activeClient != null)
                        {
                            OrderSelection();
                        }
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
                Console.WriteLine("");
                Console.WriteLine("Press number to choose client or letter to apply filter");
                Console.WriteLine("Press esc to comeback to menu");
                Console.WriteLine("");
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

        private void ClientOrderCreator()
        {
            var internetClient = ClientService.GetInternetClient(_activeClient);
            var internetOrder = internetClient != null;
            var exit = false;
            var objects = _objectRepository.GetAllObjects();
            var order = new Order();
            order.ClientId = _activeClient.Id;
            var orderObjects = new List<OrderObject>();
            var orderInfo = internetOrder ? "Client ip: " + internetClient.IpAddress + " " : "";
            orderInfo += "products: ";

            while (!exit)
            {
                Console.WriteLine("");
                Console.WriteLine("Press s to type number of product to add to order");
                Console.WriteLine("Press a to get all clients who ordered product");
                Console.WriteLine("Press p to place order");
                Console.WriteLine("Press esc to comeback to menu");
                Console.WriteLine("");
                Console.WriteLine(orderInfo);
                int iter = 0;
                foreach (var obj in objects)
                {
                    Console.Out.WriteLine(iter + ". name: " + obj.Description + " price: " + obj.Price + " (In stock: " + obj.InStock + ")");
                    iter++;
                }

                ConsoleKeyInfo currentKey = Console.ReadKey(true);
                switch (currentKey.Key)
                {
                    case ConsoleKey.S:
                        Console.Out.WriteLine("Type product number:");
                        string input = Console.ReadLine();
                        int number;
                        if(!Int32.TryParse(input, out number))
                        {
                            Console.Out.WriteLine("Not a valid number - try again!");
                            System.Threading.Thread.Sleep(1000);
                        }
                        else
                        {
                            if (number >= 0 && number < objects.Count)
                            {
                                var obj = objects[number];
                                Console.Out.WriteLine("Type amount:");
                                input = Console.ReadLine();
                                int amount;
                                if(!Int32.TryParse(input, out amount))
                                {
                                    Console.Out.WriteLine("Not a valid amount - try again!");
                                    System.Threading.Thread.Sleep(1000);
                                }
                                else
                                {
                                    var orderObj = new OrderObject();
                                    orderObj.ObjectId = obj.Id;
                                    orderObj.Amount = ((amount > 0) ? amount :  1);
                                    orderObjects.Add(orderObj);
                                    if (orderObjects.Count != 1) orderInfo += ", ";
                                    orderInfo += obj.Description + " - " + amount;
                                }
                            }
                        }
                        break;
                    case ConsoleKey.A:
                        Console.Out.WriteLine("Type product number:");
                        string inp = Console.ReadLine();
                        int num;
                        if(!Int32.TryParse(inp, out num))
                        {
                            Console.Out.WriteLine("Not a valid number - try again!");
                            System.Threading.Thread.Sleep(1000);
                        }
                        else
                        {
                            if (num >= 0 && num < objects.Count)
                            {
                                var obj = objects[num];
                                var objectClients = ClientService.GetAllClientsWhoOrdered(obj);
                                var toPrint = (objectClients.Count > 0) ? "Clients who ordered " + obj.Description + ": " : "Noone ordered " + obj.Description;
                                foreach (var c in objectClients)
                                {
                                    if (objectClients.IndexOf(c) != 0)
                                    {
                                        toPrint += ", ";
                                    }

                                    toPrint += c.Name;
                                }
                                
                                Console.WriteLine(toPrint);
                                System.Threading.Thread.Sleep(3000);
                            }
                        }
                        break;
                    case ConsoleKey.P:
                        order.Objects = orderObjects;
                        if (order.Objects.Count > 0)
                        {
                            _orderRepository.AddOrder(order);
                            Console.Out.WriteLine("Placed order successfully!");
                        }
                        else
                        {
                            Console.Out.WriteLine("Order not placed - no products selected!!");
                        }
                        exit = true;
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void OrderSelection()
        {
            var exit = false;
            var iterFrom = 0; //to display from 1-5
            var iterTo = 5;
            var orders = _orderRepository.GetAllOrders();

            while (!exit)
            {
                Console.WriteLine("");
                Console.WriteLine("Press n for next page");
                Console.WriteLine("Press p for previous page");
                Console.WriteLine("Press c to complete order");
                Console.WriteLine("Press esc to comeback to menu");
                Console.WriteLine("");
                foreach (var order in orders)
                {
                    var index = orders.IndexOf(order);
                    if (index >= iterFrom && index < iterTo)
                    {
                        Client client = _clientRepository.GetClient(order.ClientId);
                        Console.WriteLine(index + 1 + ". Number of objects: " + order.GetNumberOfObjects() + " Total price: " + order.GetTotalPrice() + " Client: " + client.Name 
                                          + ((ClientService.isInternetClient(client) ? " (internet) " : " (not internet) ")) + ((order.Completed ? "(completed) " : "(not completed) ")));
                    }
                }
                
                ConsoleKeyInfo currentKey = Console.ReadKey(true);
                switch (currentKey.Key)
                {
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                    case ConsoleKey.N:
                        if (iterFrom + 5 < orders.Count)
                        {
                            iterFrom += 5;
                            iterTo += 5;
                        }
                        break;
                    case ConsoleKey.P:
                        if (iterFrom > 0)
                        {
                            iterFrom -= 5;
                            iterTo -= 5;
                        }
                        break;
                    case ConsoleKey.C:
                        Console.Out.WriteLine("Type order number:");
                        string input = Console.ReadLine();
                        int number;
                        if(!Int32.TryParse(input, out number))
                        {
                            Console.Out.WriteLine("Not a valid number - try again!");
                            System.Threading.Thread.Sleep(1000);
                        }
                        else
                        {
                            var index = number - 1;
                            if (index < orders.Count && iterFrom <= index && iterTo > index)
                            {
                                var order = orders[index];
                                if (order.Completed == false)
                                {
                                    var result = OrderService.placeOrder(order);
                                    if (result)
                                    {
                                        Console.Out.WriteLine("Order completed successfully!");
                                        orders = _orderRepository.GetAllOrders();
                                    }
                                    else
                                    {
                                        Console.Out.WriteLine(
                                            "Order completed unsuccessfully - not enough products in stock!");
                                    }
                                }
                                else
                                {
                                    Console.Out.WriteLine(
                                        "Cannot complete completed order!");
                                }
                            }
                            else
                            {
                                Console.Out.WriteLine("Type only numbers that are visible!");
                            }
                            
                            System.Threading.Thread.Sleep(1000);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        
        private void DisplayBasicMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("Active client - " + ((_activeClient != null) ? _activeClient.Name : "not selected!"));
            Console.WriteLine("Press p to place order");
            Console.WriteLine("Press c for selecting active client");
            Console.WriteLine("Press o to search orders");
            Console.WriteLine("Press x to unselect client");
        }
    }
}