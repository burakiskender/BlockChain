using System;
namespace BlockChain.Model
{
    public class Transaction
    {
        public Guid TxId { get; set; }
        public decimal Amount { get; set; }
        public byte[] Recipient { get; set; }
        public byte[] Sender { get; set; }
    }
}
