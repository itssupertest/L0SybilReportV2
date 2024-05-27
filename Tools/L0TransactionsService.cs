using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Tools
{
    public class L0TransactionsService
    {
        private static readonly Object _lockObj = new Object();
        public L0Wallet GetTransactions(Wallet wallet)
        {
            var l0Wallet = new L0Wallet()
            {
                Address = wallet.Address,
                Age = wallet.GetWalletDays().Value,
                L0Transactions = new ()
            };
            var files = Directory.EnumerateFiles(Constants.CsvFilePath);
            Parallel.ForEach(files.ToList(), file =>
            {
                var fileName = Path.GetFileName(file);
                Console.WriteLine($"processing {fileName}");
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<L0TransactionMap>();

                    try
                    {
                        var transactions = csv.GetRecords<L0Transaction>().ToList();
                        lock (_lockObj)
                        {
                            l0Wallet.L0Transactions.AddRange(transactions.Where(t => t.SenderWallet == wallet.Address));
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            });

            return l0Wallet;
        }

        public List<L0Wallet> GetTransactions(List<Wallet> wallets)
        {
            var l0Wallets = wallets.Select(w => new L0Wallet { Address = w.Address, Age = w.GetWalletDays().Value, L0Transactions = new List<L0Transaction>() }).ToList();
            var l0Transactions = new List<L0Transaction>();
            var addresses = wallets.Select(w => w.Address).ToList();
            var files = Directory.EnumerateFiles(Constants.CsvFilePath);
            Parallel.ForEach(files.ToList(), file =>
            {
                var fileName = Path.GetFileName(file);
                Console.WriteLine($"processing {fileName}");
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<L0TransactionMap>();

                    try
                    {
                        var transactions = csv.GetRecords<L0Transaction>().ToList();
                        lock (_lockObj)
                        {
                            l0Transactions.AddRange(transactions.Where(t => addresses.Contains(t.SenderWallet)));

                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            });
            foreach (var transaction in l0Transactions)
            {
                l0Wallets.First(w => String.Compare(w.Address, transaction.SenderWallet, true) == 0).L0Transactions.Add(transaction);
            }

            return l0Wallets;
        }

        public List<L0Wallet> GetTransactions(List<string> wallets)
        {
            var l0Wallets = wallets.Select(w => new L0Wallet { Address = w, Age = 0, L0Transactions = new List<L0Transaction>() }).ToList();
            var l0Transactions = new List<L0Transaction>();
            var addresses = wallets.Select(w => w).ToList();
            var files = Directory.EnumerateFiles(Constants.CsvFilePath);
            Parallel.ForEach(files.ToList(), file =>
            {
                var fileName = Path.GetFileName(file);
                Console.WriteLine($"processing {fileName}");
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<L0TransactionMap>();

                    try
                    {
                        var transactions = csv.GetRecords<L0Transaction>().ToList();
                        lock (_lockObj)
                        {
                            l0Transactions.AddRange(transactions.Where(t => addresses.Contains(t.SenderWallet)));

                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            });
            foreach (var transaction in l0Transactions)
            {
                l0Wallets.First(w => String.Compare(w.Address, transaction.SenderWallet, true) == 0).L0Transactions.Add(transaction);
            }

            return l0Wallets;
        }

        public void WriteStats(List<L0Wallet> wallets)
        {
            Console.WriteLine($"WALLET; WALLET_AGE; FIRST DATE; LAST DATE; MIN_USD_STARGATE; MAX_USD_STARGATE; AVG_USD_STARGATE; TOTAL_USD_STARGATE; TOTAL_NATIVE; COUNT");
            foreach(var wallet in wallets.Where(t => t.L0Transactions.Count > 0).OrderBy(t => t.Age))
            {
                var firstDate = wallet.L0Transactions.OrderBy(t => t.TimeStamp).First().TimeStamp;
                var lastDate = wallet.L0Transactions.OrderBy(t => t.TimeStamp).Last().TimeStamp;
                var minStargate = wallet.L0Transactions.Where(t => t.StargateSwapUsd.HasValue).MinBy(t => t.StargateSwapUsd.Value).StargateSwapUsd.Value;
                var maxStargate = wallet.L0Transactions.Where(t => t.StargateSwapUsd.HasValue).MaxBy(t => t.StargateSwapUsd.Value).StargateSwapUsd.Value;
                var avgStargate = wallet.L0Transactions.Where(t => t.StargateSwapUsd.HasValue).Average(t => t.StargateSwapUsd.Value);
                var totalStzrgete = wallet.L0Transactions.Where(t => t.StargateSwapUsd.HasValue).Sum(t => t.StargateSwapUsd.Value);

                var totalNative = wallet.L0Transactions.Where(t => t.NativeDropUsd.HasValue).Sum(t => t.NativeDropUsd.Value);
                var count = wallet.L0Transactions.Count();
                Console.WriteLine($"{wallet.Address.Substring(0,6)}; {wallet.Age}; {firstDate.ToShortDateString()}; {lastDate.ToShortDateString()}; {minStargate.ToString("N2")}; {maxStargate.ToString("N2")}; {avgStargate.ToString("N2")}; {totalStzrgete.ToString("N2")}; {totalNative.ToString("N2")}; {count}");
            }
            Console.WriteLine($"WALLETS WITH ONLY testnet-bridge");

            foreach (var wallet in wallets.Where(t => t.L0Transactions.Count ==0).OrderBy(t => t.Age))
            {
                Console.WriteLine($"{wallet.Address.Substring(0, 6)}");
            }
        }
    }
}
