using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IRLib.IRIndexer.Enums;
using IRLib.IRIndexer.Interfaces;
using IRLib.IRLib.IRIndexer;

namespace IRLib.IRIndexer
{


    public class SFIndexerBuilder 
    {
        private MemoryStream fileStream;
        private FileDestination fileDestination;
        private FileType fileType;
        private string fileName;
        private string folderName;

        public SFIndexerBuilder (MemoryStream fileStream, string fileName)
        {
            this.fileStream = fileStream;
            this.fileName = fileName;
        }

        public SFIndexerBuilder AddFileDestination(FileDestination destination, string folderName)
        {
            this.folderName = folderName;
            this.fileDestination = destination;
            return this;
        }

        public SFIndexerBuilder AddFileType(FileType fileType)
        {
            this.fileType = fileType;
            return this;
        }

        public SFIndexer Build()
        {
            SFIndexer i = new SFIndexer(
                this.fileStream,
                this.fileName,
                this.fileDestination,
                this.folderName,
                this.fileType
            );
            return i;
        }
    }

}
