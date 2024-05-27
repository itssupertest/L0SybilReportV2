using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class Wallet
    {
        public string Address { get; set; }
        public List<Transaction> Transactions { get; set; }

        public Wallet(string address, List<Transaction> txs)
        {
            this.Address = address;
            this.Transactions = txs;
        }

        public IEnumerable<Transaction> GetTokenTransfers(double minUsdValue = 10)
        {
            return Transactions
                    .Where(t => t.AssetType.ToLower() == "token")
                    .Where(t => t.TxType.ToLower() == "receive" || t.TxType.ToLower() == "send")
                    .Where(t => !String.IsNullOrWhiteSpace(t.RelatedAddress) && !Constants.WhitelistedAddresses.Contains(t.RelatedAddress))
                    .Where(t => t.AssetUsdValueChange.HasValue && Math.Abs(t.AssetUsdValueChange.Value) > minUsdValue);
        }

        public IEnumerable<Transaction> GetTokenTransactions(DateTime date)
        {
            return Transactions
                    .Where(t => t.AssetType.ToLower() == "token")
                    .Where(t => t.TxType.ToLower() == "receive" || t.TxType.ToLower() == "send")
                    .Where(t => t.Date.HasValue && t.Date.Value == date);
        }

        public IEnumerable<(string address, double volume)> WriteAddressesVolume(double minUsdValue = 10)
        {
            Console.WriteLine($"{Address} address interactions:");
            var volumes = GetTokenTransfers(minUsdValue).GroupBy(t => t.RelatedAddress).Select(g => (address:g.Key, volume:g.Sum(gt => Math.Abs(gt.AssetUsdValueChange.Value))));
            foreach (var volume in volumes.OrderByDescending(t => t.volume))
            {
                Console.WriteLine($"    {volume.address} ${volume.volume}");
            }
            return volumes;
        }

        public IEnumerable<(string address, double volume)> GetAddressesVolume(double minUsdValue = 10)
        {
            var volumes = GetTokenTransfers(minUsdValue).GroupBy(t => t.RelatedAddress).Select(g => (address: g.Key, volume: g.Sum(gt => Math.Abs(gt.AssetUsdValueChange.Value))));
            return volumes;
        }

        public int? GetWalletDays()
        {
            return (DateTime.Now - Transactions.Where(t => t.Date.HasValue).Last().Date).HasValue ? (DateTime.Now - Transactions.Where(t => t.Date.HasValue).Last().Date).Value.Days : null;
        }
    }
}
