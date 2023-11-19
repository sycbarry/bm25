using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRLib.IRIndexer.Interfaces
{
    public interface ISFIndexer  
    {
        // returns the folder name 
        public string GetFolderName();

        // returns the file destination 
        // this is determined by the filedestinationtype.
        // 1. Local - the complete folder path
        // 2. TODO
        public string GetFileDestination();

        // build the file destination
        // 1. Local - builds the folder if it doesn't exist. 
        // 2. S3 - tests the connection and the folder path
        //    builds the folder if it doesn't exist 
        public void BuildDestinationFolder();

        // check if the destination folder exists. 
        // 1. Local - checks the current directory 
        // 2. S3 - TODO
        public bool FolderExists();

        // returns the file size in memory stream.
        public double GetFileStreamSize();

        // getter for the memory stream.
        public MemoryStream GetFileStream();

        // get a stream reader for the memory stream.
        public StreamReader GetFileStreamReader();

        // parse memory stream into a readable text based format 
        public void ParseMemoryStream();

        // parse the stream reader into a readable pdf format.
        public void ParsePDFFromMemoryStream(MemoryStream ms);

        // parses the sentences from the text on the page.
        public string[] ParseSentences(string text);

        // index the sentences with the page numbers and sentence number
        // convention: 
        // Line: 'page Number || sentence Number || sentence Text'
        public void IndexSentences(string[] sentences, int pageNum);

        // write the sentences to the file. 
        public void WriteSentences();


    }
}