using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IRLib.IRIndexer.Enums;

namespace IRLib.IRIndexer
{
    public class IndexerComponent
    {
        EIndexerComponent component;
        public IndexerComponent(EIndexerComponent c)
        {
            this.component = c;
        }
        public EIndexerComponent GetComponent()
        {
            return this.component;
        }
    }
}
