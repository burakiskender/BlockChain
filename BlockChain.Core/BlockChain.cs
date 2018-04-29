using System;
using System.Collections.Generic;
using BlockChain.Model;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace BlockChain.Core
{
    public class BlockChain : IBlockChain
    {
        private List<Block> _blockList = new List<Block>();
        private List<Node> _nodeList = new List<Node>();

        public byte[] Node { get; private set; }
        public Block CurrentBlock
        {
            get
            {
                return _blockList.Last();
            }
        }

        public BlockChain()
        {
            this.Node = Guid.NewGuid().ToByteArray();

            //Genesis Block
            GenerateBlock(proofValue: 1, prevHash: "genesisblock");
        }


        public long CreatePoW(long prevProof, string previousHash)
        {
            long proof = 0;
            while (!ValidatePoW(prevProof, proof, previousHash))
            {
                proof = proof + 1;

            }
            return proof;
        }

        public Block GenerateBlock(long proofValue, string prevHash = null)
        {
            var block = new Block
            {
                Index = _blockList.Count,
                ProofValue = proofValue,
                PrevHash = prevHash ?? GetHash(_blockList.Last())
            };
            _blockList.Add(block);
            return block;
        }

        public Transaction GenerateTransaction(byte[] sender, byte[] recipient, decimal amount)
        {
            var transaction = new Transaction
            {
                TxId = System.Guid.NewGuid(),
                Sender = sender,
                Recipient = recipient,
                Amount = amount
            };

            CurrentBlock.Transactions.Add(transaction);

            return transaction;
        }

        public string GetBlockChain()
        {
            var response = new
            {
                chain = _blockList.ToArray(),
                length = _blockList.Count
            };

            return JsonConvert.SerializeObject(response);
        }

        public string GetHash(Block block)
        {
            var json = JsonConvert.SerializeObject(block);
            return GenerateHash(json);
        }

        /// <summary>
        /// Create new block/ mining operation
        /// </summary>
        /// <returns>The transaction.</returns>
        public string MineBlockChain()
        {
            long proof = CreatePoW(this.CurrentBlock.ProofValue, this.CurrentBlock.PrevHash);

            GenerateTransaction(new byte[1], recipient: this.Node, amount: 1);
            var block = GenerateBlock(proof,null);

            var response = new
            {
                Message = "New Block Mined",
                Index = block.Index,
                Transactions = block.Transactions.ToArray(),
                Proof = block.ProofValue,
                PrevHash = block.PrevHash
            };

            return JsonConvert.SerializeObject(response);
        }

        public void RegisterNode(byte[] address)
        {
            _nodeList.Add(new Node { Address = new Uri(address.ToString()) });

        }

        public bool ValidatePoW(long prevProof, long proof, string prevHash)
        {
            string guess = $"{prevProof}{proof}{prevHash}";
            string result = GenerateHash(guess);
            return result.StartsWith("0000", StringComparison.Ordinal);
        }





        #region Private Functions


        private string GenerateHash(string data)
        {
            var sha256 = new SHA256Managed();
            var stringBuilder = new System.Text.StringBuilder();

            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(data);
            byte[] hash = sha256.ComputeHash(bytes);

            foreach (byte b in hash)
                stringBuilder.Append($"{b:x2}");

            return stringBuilder.ToString();
        }



        private bool ValidateBlockChain(List<Block> blockChain)
        {
            Block block = null;
            int currentIndex = 1;
            Block lastBlock = blockChain.First();
            while (currentIndex < blockChain.Count)
            {
                block = blockChain.ElementAt(currentIndex);

                //Check that the hash of the block is correct
                if (block.PrevHash != GetHash(lastBlock))
                    return false;

                //Check that the Proof of Work is correct
                if (!ValidatePoW(lastBlock.ProofValue, block.ProofValue, lastBlock.PrevHash))
                    return false;

                lastBlock = block;
                currentIndex++;
            }

            return true;
        }



        #endregion
    }
}
