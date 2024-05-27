using CsvHelper;
using System.Globalization;
using System.Linq;
using Tools;

namespace Zielonomi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cluster = LoadClusterData();

            Console.WriteLine("-----------------------===CHECK REPORTED ADDRESSES===----------------------------");
            if (!File.Exists(Constants.InitialListFilePath))
            {
                Console.WriteLine($"File {Constants.InitialListFilePath} needs to exist in order to check already reported addresses");
            }
            var initialAddresses = File.ReadAllLines(Constants.InitialListFilePath).ToList();

            foreach (var address in cluster.Wallets.Keys)
            {
                if (initialAddresses.Contains(address, StringComparer.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"{address} already reported");
                }
                else
                {
                    Console.WriteLine($"{address}");
                }
            }
            //Used to find new interactions in cluster
            //Console.WriteLine("-----------------------===FIND NEW CONNECTIONS===----------------------------");
            //foreach (var addr in cluster.TryGetNewConnections().OrderBy(a => a.vol))
            //{
            //    Console.WriteLine($"{addr.addr} {addr.vol.ToString("N2")}");
            //}
            Console.WriteLine("-----------------------===CHECK ADDRESSES AGE===----------------------------");
            cluster.WriteWalletsAge();
            Console.WriteLine("-----------------------===CHECK CLUSTER INTERACTIONS===----------------------------");
            cluster.WriteInteractionsInCluster(10);
            Console.WriteLine("-----------------------===WRITE CLUSTER INTERACTIONS GRAPH===----------------------------");
            cluster.WriteGraph(10);
            Console.WriteLine("-----------------------===CHECK ADDRESSES FUNDING===----------------------------");
            Console.WriteLine("Script is returning 0xbc55 for 0x48d7 but it should be changed to 0x45ab. This is becasue only last 10K transactions could be downloaded from DeBank and address 0x48d7 has way more transactions resulting in wrong address being detected as funding.");
            cluster.WriteFunding();
            Console.WriteLine("Script is returning 0xbc55 for 0x48d7 but it should be changed to 0x45ab. This is becasue only last 10K transactions could be downloaded from DeBank and address 0x48d7 has way more transactions resulting in wrong address being detected as funding.");
            Console.WriteLine("-----------------------===WRITE ADDRESSES FUNDING GRAPH===----------------------------");
            cluster.WriteFundingGraph();

            Console.WriteLine("-----------------------===CHECK L0 INTERACTIONS===----------------------------");
            if(!Directory.Exists(Constants.CsvFilePath))
            {
                Console.WriteLine($"Directory {Constants.CsvFilePath} needs to exist with CSV data in order to check L0 transactions");
            }
            var l0TransactionService = new L0TransactionsService();
            var foundTransactions = l0TransactionService.GetTransactions(cluster.Wallets.Values.ToList());
            l0TransactionService.WriteStats(foundTransactions);
        }

        static Cluster LoadClusterData()
        {
            var cluster = new Cluster();
            foreach (var file in Directory.EnumerateFiles("WalletsData"))
            {
                var address = Path.GetFileName(file)[0..42];
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<TransactionMap>();

                    try
                    {
                        var records = csv.GetRecords<Transaction>().ToList();
                        cluster.Wallets[address] = new Wallet(address, records);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            foreach (var file in Directory.EnumerateFiles("CexsData"))
            {
                var address = Path.GetFileName(file)[0..42];
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<TransactionMap>();

                    try
                    {
                        var records = csv.GetRecords<Transaction>().ToList();
                        cluster.Cexs[address] = new Wallet(address, records);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            foreach (var file in Directory.EnumerateFiles("OtherData"))
            {
                var address = Path.GetFileName(file)[0..42];
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<TransactionMap>();

                    try
                    {
                        var records = csv.GetRecords<Transaction>().ToList();
                        cluster.Other[address] = new Wallet(address, records);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return cluster;
        }
    }
}