using System;
using System.Collections.Generic;
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
            prepareBasicData();
            new UserInterface().Start();
        }

        public static void prepareBasicData()
        {
            PhysicalClient physicalClient = new PhysicalClient();
            physicalClient.Address = "Wieszczycka 2";
            physicalClient.Name = "Klient 1";
            InternetClient internetClient = new InternetClient();
            internetClient.IpAddress = "123.231.23.2";
            internetClient.Name = "Klient internetowy 1";
            internetClient.Address = "test";
            new InternetClientRepository().AddClient(internetClient);
            new PhysicalClientRepository().AddClient(physicalClient);
        }
    }
}