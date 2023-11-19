using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IRLib.IRCorpus;

namespace IRLib.IRRetriever.Interfaces
{
    public interface IRetriever
    {
        // extract from disk 
        public MemoryStream PullFileFromDisk(string fileName);

        // parse the file
        public List<string> ParseFile(MemoryStream ms);

        public Corpus RetrieveFile(string fileName);

    }
}