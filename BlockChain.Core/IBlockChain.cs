using System;
namespace BlockChain.Core
{
    public interface IBlockChain
    {
        void RegisterNode(byte[] address);

        long CreatePoW(long prevProof, string previousHash);
        bool ValidatePoW(long prevProof, long proof, string prevHash);
        string GetHash(Model.Block block);

        Model.Transaction GenerateTransaction(byte[] sender, byte[] recipient, decimal amount);
        Model.Block GenerateBlock(long proofValue, string prevHash = null);

        string MineBlockChain();
        string GetBlockChain();
    }
}
