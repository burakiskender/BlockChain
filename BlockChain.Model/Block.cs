using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace BlockChain.Model
{
    public class Block
    {
        public Block()
        {
            this.Transactions = new List<Transaction>();
            this.Timestamp = DateTime.UtcNow;
            this.Id = System.Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public long Index { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Transaction> Transactions { get; set; }

        public string PrevHash { get; set; }

        public long ProofValue { get; set; }

        public int TransactionCount
        {
            get {
                return Transactions.Count;
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
