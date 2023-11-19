using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Text;

using IRLib.IRIndexer; 
using IRLib.IRIndexer.Enums;
using IRLib.IRLib.IRIndexer;

namespace IRLib.IRIndexer.Tests
{
    public class BSBITest
    {
        [Fact]
        public void Test1()
        {
            BSBIndexer indexer = new BSBIndexerBuilder("BSBI/", "BSBI/")
                .Build();
            
            try {
                // make the invoker.
                IndexerInvoker invoker = new IndexerInvoker(indexer);
                invoker.Fire();
            } catch (Exception e) {
                Assert.False(true, $"{e}");
            }

        }
    }
}