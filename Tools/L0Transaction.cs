using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class L0Transaction
    {
        //"NATIVE_DROP_USD","STARGATE_SWAP_USD"
        public string SenderWallet {  get; set; }
        public string SourceChain {  get; set; }
        public string SourceTransactionHash { get; set; }
        public string SourceContract { get; set; }
        public string DestinationChain { get; set; }
        public string DestinationTransactionHash { get; set; }
        public string DestinationContract { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Project {  get; set; }
        public float? NativeDropUsd { get; set; }
        public float? StargateSwapUsd { get; set; }

        public override string ToString()
        {
            return $"Wallet {SenderWallet} at {TimeStamp} from {SourceChain} to {DestinationChain} using {Project} with ${NativeDropUsd} USD";
        }
    }

    public class L0TransactionMap : ClassMap<L0Transaction>
    {
        public L0TransactionMap()
        {
            Map(m => m.SourceChain).Name("SOURCE_CHAIN");
            Map(m => m.SourceTransactionHash).Name("SOURCE_TRANSACTION_HASH");
            Map(m => m.SourceContract).Name("SOURCE_CONTRACT");
            Map(m => m.DestinationChain).Name("DESTINATION_CHAIN");
            Map(m => m.DestinationTransactionHash).Name("DESTINATION_TRANSACTION_HASH");
            Map(m => m.DestinationContract).Name("DESTINATION_CONTRACT");
            Map(m => m.SenderWallet).Name("SENDER_WALLET");
            Map(m => m.TimeStamp).Name("SOURCE_TIMESTAMP_UTC");
            Map(m => m.Project).Name("PROJECT");
            Map(m => m.NativeDropUsd).Name("NATIVE_DROP_USD");
            Map(m => m.StargateSwapUsd).Name("STARGATE_SWAP_USD");

        }
    }
}
