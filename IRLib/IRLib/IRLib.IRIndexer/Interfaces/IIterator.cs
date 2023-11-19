using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRLib.IRIndexer.Interfaces
{
    public interface IIterator
    {
        bool HasNext();
        IndexerComponent MoveNext();
    }
}