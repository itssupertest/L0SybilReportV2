using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Tools
{
    public class Cluster
    {
        public Dictionary<string, Wallet> Wallets = new();
        public Dictionary<string, Wallet> Cexs = new();
        public Dictionary<string, Wallet> Other = new();
        public List<(string addr, double vol)> TryGetNewConnections()
        {
            var newAddresses = new List<(string addr, double vol)>();
            foreach (var wallet in Wallets)
            {
                var volumes = wallet.Value.GetAddressesVolume(0).Where(av => !Cexs.ContainsKey(av.address) && !Wallets.Keys.Contains(av.address));
                newAddresses.AddRange(volumes);
            }
            return newAddresses.GroupBy(av => av.addr).Select(av => (av.Key, av.Sum(avg => avg.vol))).ToList();
        }

        public void WriteWallets()
        {
            foreach (var wallet in Wallets)
            {
                Console.WriteLine(wallet.Key.ToString());
            }
        }

        public void WriteInteractionsInCluster(double minUsdValue = 10)
        {
            var clusterWallets = Wallets.Keys.ToList();
            foreach (var wallet in Wallets)
            {
                Console.WriteLine($"- Direct connections with {wallet.Key.Substring(0, 6)}");
                var transactionsGroup = wallet.Value.GetTokenTransfers(minUsdValue).GroupBy(t => t.RelatedAddress).Where(t => clusterWallets.Contains(t.Key));
                foreach (var group in transactionsGroup)
                {
                    Console.WriteLine($"    - {group.Key.Substring(0, 6)} - {group.Count()} interaction(s)");
                }
            }

            foreach (var wallet in Cexs)
            {
                Console.WriteLine($"- CEX deposits for {wallet.Key.Substring(0, 6)}");
                var transactionsGroup = wallet.Value.GetTokenTransfers(minUsdValue).GroupBy(t => t.RelatedAddress).Where(t => clusterWallets.Contains(t.Key));
                foreach (var group in transactionsGroup)
                {
                    Console.WriteLine($"    - {group.Key.Substring(0, 6)} - {group.Count()} interaction(s)");
                }
            }

            foreach (var wallet in Other)
            {
                Console.WriteLine($"- OTHER deposits for {wallet.Key.Substring(0, 6)}");
                var transactionsGroup = wallet.Value.GetTokenTransfers(minUsdValue).GroupBy(t => t.RelatedAddress).Where(t => clusterWallets.Contains(t.Key));
                foreach (var group in transactionsGroup)
                {
                    Console.WriteLine($"    - {group.Key.Substring(0, 6)} - {group.Count()} interaction(s)");
                }
            }
        }

        public void WriteGraph(double minUsdValue = 10)
        {
            var clusterWallets = Wallets.Keys.ToList();
            Console.WriteLine("{");
            Console.WriteLine();
            foreach (var wallet in Wallets)
            {
                Console.Write($"    \"{wallet.Key.Substring(0, 6)}\": [");
                var transactionsGroup = wallet.Value.GetTokenTransfers(minUsdValue).GroupBy(t => t.RelatedAddress).Where(t => clusterWallets.Contains(t.Key));
                var strBuilder = new StringBuilder();
                foreach (var group in transactionsGroup)
                {
                    strBuilder.Append($"(\"{group.Key.Substring(0, 6)}\", {group.Count()}),");
                }
                if (strBuilder.Length > 0)
                {
                    strBuilder.Remove(strBuilder.Length - 1, 1);
                }
                Console.Write(strBuilder.ToString());
                Console.Write("],");
                Console.WriteLine("");
            }
            Console.WriteLine("}");


            Console.WriteLine("{");
            Console.WriteLine();
            foreach (var wallet in Cexs)
            {
                Console.Write($"    \"CEX {wallet.Key.Substring(0, 6)}\": [");
                var transactionsGroup = wallet.Value.GetTokenTransfers(minUsdValue).GroupBy(t => t.RelatedAddress).Where(t => clusterWallets.Contains(t.Key));
                var strBuilder = new StringBuilder();
                foreach (var group in transactionsGroup)
                {
                    strBuilder.Append($"(\"{group.Key.Substring(0, 6)}\", {group.Count()}),");
                }
                if (strBuilder.Length > 0)
                {
                    strBuilder.Remove(strBuilder.Length - 1, 1);
                }
                Console.Write(strBuilder.ToString());
                Console.Write("],");
                Console.WriteLine("");
            }
            Console.WriteLine("}");

            Console.WriteLine("{");
            Console.WriteLine();
            foreach (var wallet in Other)
            {
                Console.Write($"    \"Distribution address {wallet.Key.Substring(0, 6)}\": [");
                var transactionsGroup = wallet.Value.GetTokenTransfers(minUsdValue).GroupBy(t => t.RelatedAddress).Where(t => clusterWallets.Contains(t.Key));
                var strBuilder = new StringBuilder();
                foreach (var group in transactionsGroup)
                {
                    strBuilder.Append($"(\"{group.Key.Substring(0, 6)}\", {group.Count()}),");
                }
                if (strBuilder.Length > 0)
                {
                    strBuilder.Remove(strBuilder.Length - 1, 1);
                }
                Console.Write(strBuilder.ToString());
                Console.Write("],");
                Console.WriteLine("");
            }
            Console.WriteLine("}");
        }

        public void WriteVolumesInCluster(double minUsdValue = 10)
        {
            var clusterWallets = Wallets.Keys.ToList();
            foreach (var wallet in Wallets)
            {
                Console.WriteLine($"- Direct connections with {wallet.Key.Substring(0, 6)}");
                var transactionsGroup = wallet.Value.GetTokenTransfers(minUsdValue).GroupBy(t => t.RelatedAddress).Where(t => clusterWallets.Contains(t.Key));
                foreach(var group in transactionsGroup)
                {
                    Console.WriteLine($"    - {group.Key.Substring(0, 6)} ${group.Sum(t => t.AssetUsdValueChange.Value).ToString("N2")}");
                }
            }

            foreach (var wallet in Cexs)
            {
                Console.WriteLine($"- CEX deposits for {wallet.Key}");
                var transactionsGroup = wallet.Value.GetTokenTransfers(minUsdValue).GroupBy(t => t.RelatedAddress).Where(t => clusterWallets.Contains(t.Key));
                foreach (var group in transactionsGroup)
                {
                    Console.WriteLine($"    - {group.Key} ${group.Sum(t => t.AssetUsdValueChange.Value).ToString("N2")}");
                }
            }
        }

        public void WriteTxsForDay(DateTime date, double? txMinValue = null)
        {
            var clusterWallets = Wallets.Keys.ToList();
            var txs = new List<AddressTransaction>();
            foreach (var wallet in Wallets)
            {
                var transactions = wallet.Value.GetTokenTransactions(date);
                if (txMinValue.HasValue)
                {
                    transactions = transactions.Where(t => Math.Abs(t.AssetUsdValueChange.Value) > txMinValue.Value);
                }
                txs.AddRange(transactions.Select(t => new AddressTransaction(wallet.Key, t)));
            }

            foreach (var tx in txs.Where(t => t.IsScam == "no").OrderBy(t => t.Date).ThenBy(t => t.Time))
            {
                Console.WriteLine($"{tx.Time.Value} {tx.Address.Substring(0, 6)} {tx.AssetUsdValueChange.Value.ToString("N2")} {tx.Link}");
            }
        }

        public void WriteFunding(double minUsdValue = 1)
        {
            var clusterWallets = Wallets.Keys.ToList();
            foreach (var wallet in Wallets)
            {
                Console.WriteLine($"- {wallet.Key.Substring(0, 6)}");
                var fundings = wallet.Value.GetTokenTransfers(minUsdValue).Where(t => t.TxType == "receive").TakeLast(2).Where(t => clusterWallets.Contains(t.RelatedAddress));
                foreach (var funding in fundings)
                {
                    Console.WriteLine($"    - {funding.RelatedAddress.Substring(0, 6)}");
                }
            }
        }

        public void WriteFundingGraph(double minUsdValue = 1)
        {
            var clusterWallets = Wallets.Keys.ToList();
            Console.WriteLine("{");
            Console.WriteLine();
            foreach (var wallet in Wallets)
            {
                Console.Write($"    \"{wallet.Key.Substring(0, 6)}\": [");
                var fundings = wallet.Value.GetTokenTransfers(minUsdValue).Where(t => t.TxType == "receive").TakeLast(2).Where(t => clusterWallets.Contains(t.RelatedAddress));
                var strBuilder = new StringBuilder();
                foreach (var funding in fundings)
                {
                    strBuilder.Append($"(\"{funding.RelatedAddress.Substring(0, 6)}\", 1),");
                }
                if (strBuilder.Length > 0)
                {
                    strBuilder.Remove(strBuilder.Length - 1, 1);
                }
                Console.Write(strBuilder.ToString());
                Console.Write("],");
                Console.WriteLine("");
            }
            Console.WriteLine("}");
        }



        public void WriteWalletsAge()
        {
            foreach (var wallet in Wallets.OrderBy(w => w.Value.GetWalletDays()))
            {
                var age = wallet.Value.GetWalletDays();
                Console.WriteLine($"- {wallet.Key} - {age} days old");

            }
        }
    }
}
