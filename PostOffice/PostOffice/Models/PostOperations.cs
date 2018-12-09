using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PostOffice.App_Data;

namespace PostOffice.Models
{
    public class PostOperations
    {
        public static string UserName;
        
        public static List<USERS> GetUsersEf()
        {
            var context = new POSTContext();
            var owners = context.USERS.ToList();
            return owners;
        }
        public static List<ReceivedPackage> GetReceivedPackageEf()
        {
            var context = new POSTContext();
            var ReceivedPackage = context.ReceivedPackage.ToList();
            return ReceivedPackage;
        }
        public static List<INDEX> GetIndexEf()
        {
            var context = new POSTContext();
            var Indexes = context.INDEX.ToList();
            return Indexes;
        }
        public static List<Subscribe> GetSubscribeEf()
        {
            var context = new POSTContext();
            var Subscribes = context.Subscribe.ToList();
            return Subscribes;
        }
        public static List<Package> GetPackageEf()
        {
            var context = new POSTContext();
            var Packages = context.Package.ToList();
            return Packages;
        }
        public static List<Edition> GetEditionEf()
        {
            var context = new POSTContext();
            var Editions = context.Edition.ToList();
            return Editions;
        }
        public static List<StatusPack> GetStatusPacksEf()
        {
            var context = new POSTContext();
            var StatusPacks = context.StatusPack.ToList();
            return StatusPacks;
        }
        public static List<SentPackage> GetSentPackageEf()
        {
            var context = new POSTContext();
            var SentPackages = context.SentPackage.ToList();
            return SentPackages;
        }
        public static List<Clients> GetClientsEf()
        {
            var context = new POSTContext();
            var clients = context.Clients.ToList();
            return clients;
        }
    }
}