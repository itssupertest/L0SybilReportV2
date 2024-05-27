using CsvHelper;
using System.Globalization;
using Tools;

namespace Cryptoplaneta
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
            cluster.WriteInteractionsInCluster(0);
            Console.WriteLine("-----------------------===WRITE CLUSTER INTERACTIONS GRAPH===----------------------------");
            cluster.WriteGraph(0);
            Console.WriteLine("-----------------------===CHECK ADDRESSES FUNDING===----------------------------");
            cluster.WriteFunding(0);
            Console.WriteLine("-----------------------===WRITE ADDRESSES FUNDING GRAPH===----------------------------");
            cluster.WriteFundingGraph(0);
            Console.WriteLine("-----------------------===WRITE SPECIFIC TRANSACTIONCS===----------------------------");


            Console.WriteLine("-----------------------===CHECK L0 INTERACTIONS===----------------------------");
            if (!Directory.Exists(Constants.CsvFilePath))
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

            return cluster;
        }
    }
}

