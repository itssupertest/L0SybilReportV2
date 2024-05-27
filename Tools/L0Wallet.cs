using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class L0Wallet
    {
        public string Address { get; set; } 
        public int Age { get; set; }
        public List<L0Transaction> L0Transactions { get; set; }
    }
}
