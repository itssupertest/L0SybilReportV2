using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class Transaction
    {
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string Chain { get; set; }
        public string TxHash { get; set; }
        public string Link { get; set; }
        public string TxType { get; set; }
        public string RelatedAddress { get; set; }
        public string Protocol { get; set; }
        public string Asset { get; set; }
        public string AssetType { get; set; }
        public double? AssetBalanceChange { get; set; }
        public double? AssetUsdValueChange { get; set; }
        public string IsScam {get; set; }
    }

    public class AddressTransaction : Transaction
    {
        public string Address { get; set; }
        public AddressTransaction(string addr, Transaction tx)
        {
            this.Address = addr;
            this.Date = tx.Date;
            this.Time = tx.Time;
            this.Chain = tx.Chain;
            this.TxHash = tx.TxHash;
            this.Link = tx.Link;
            this.TxType = tx.TxType;
            this.RelatedAddress = tx.RelatedAddress;
            this.Protocol = tx.Protocol;
            this.Asset = tx.Asset;
            this.AssetType = tx.AssetType;
            this.AssetBalanceChange = tx.AssetBalanceChange;
            this.AssetUsdValueChange = tx.AssetUsdValueChange;
            this.IsScam = tx.IsScam;
        }
    }

    public class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Map(m => m.Date).Name("Date").TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.Time).Name("Time");
            Map(m => m.Chain).Name("Chain");
            Map(m => m.TxHash).Name("Tx hash");
            Map(m => m.Link).Name("Link");
            Map(m => m.TxType).Name("Tx type");
            Map(m => m.RelatedAddress).Name("Related addr");
            Map(m => m.Protocol).Name("Protocol");
            Map(m => m.Asset).Name("Asset");
            Map(m => m.AssetType).Name("Asset type");
            Map(m => m.AssetBalanceChange).Name("Asset balance change");
            Map(m => m.AssetUsdValueChange).Name("Asset USD value change");
            Map(m => m.IsScam).Name("Is scam");
        }
    }
}
