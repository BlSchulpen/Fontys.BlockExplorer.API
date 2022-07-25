namespace Fontys.BlockExplorer.API.IntegrationTests
{
    using Fontys.BlockExplorer.Domain.Models;
    using System.Collections.Generic;

    public class MockDbFactory
    {
        public List<Block> MockBlocks(int nrBlocks)
        {
            var newBlocks = new List<Block>();
            for (int i = 0; i < nrBlocks; i++)
            {
                newBlocks.Add(new Block() { Height = i, Hash = i.ToString() });
            }
            return newBlocks;
        }
    }
}
