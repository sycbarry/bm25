using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRLib.IRIndexer;

namespace IRLib.IRLib.IRIndexer
{
    public class BSBIndexerBuilder
    {
       private string targetFolder; 
       private string sourceFolder; 

        public BSBIndexerBuilder(string targetFolder, string sourceFolder)
        {
            this.targetFolder = targetFolder; 
            this.sourceFolder = sourceFolder; 
        }

        public BSBIndexer Build()
        {
            return new BSBIndexer(this.sourceFolder, this.targetFolder);
        }
    }
}