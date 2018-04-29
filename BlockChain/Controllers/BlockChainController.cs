using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockChain.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlockChain.Controllers
{
    [Route("api/[controller]")]
    public class BlockChainController : Controller
    {
        private Core.IBlockChain _blockChain;
        public BlockChainController(Core.IBlockChain blockChain)
        {
            _blockChain = blockChain;
        }

        // GET blockchain
        [HttpGet]
        public string Get()
        {
            return _blockChain.GetBlockChain();
        }

        // Mine 
        [HttpPut]
        public string Put()
        {
            return _blockChain.MineBlockChain();
        }


        // POST create a random transaction
        [HttpPost]
        public Transaction Post()
        {
            var senderAddress = "0x23AE2303434";            
            var sender = System.Text.Encoding.Unicode.GetBytes(senderAddress);
            var recipientAddress = "0x43B7QS12";
            var recipient = System.Text.Encoding.Unicode.GetBytes(recipientAddress);
            return _blockChain.GenerateTransaction(sender,recipient,1);
        }
    }
}
