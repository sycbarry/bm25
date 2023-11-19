using IRLib.IRRetriever.Interfaces;
using IRLib.IRCorpus;
using System.Text;
using System;
using System.Linq.Expressions;

namespace IRLib.IRRetriever;
public class Retriever
{

        public Retriever(){}
        // get the 
        public MemoryStream PullFileFromDisk(string fileName)
        {
                // make a new memory stream.
                MemoryStream ms = new MemoryStream();
                // build the path.
                var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                // write the filestream of the file (in path) to the memory stream.
                using(FileStream fs = new FileStream(path, FileMode.Open))
                {
                        fs.CopyTo(ms);
                        ms.Seek(0, SeekOrigin.Begin);
                }


                return ms;
        }


        // parse the file from the memory stream. 
        // builds a list of strings, each string corresponding to a document 
        // for our corpus. 
        public List<string> ParseFile(MemoryStream ms)
        {
                //
                List<string> list = new List<string>();
                StreamReader sr = new StreamReader(ms);
                try {
                        while( ! sr.EndOfStream )
                        {
                                list.Add(sr.ReadLine() ?? "");
                        }
                } catch (Exception e)
                {
                        throw new Exception("Error during the parsing of a file: " + e.Message);
                }
                finally {
                        sr.Close();
                        sr.Dispose();
                }
                return list;
        }

        // main method that invokes the reading of the file from disk.
        public Corpus RetrieveFile(string fileName)
        {
                // extract the file
                MemoryStream ms = this.PullFileFromDisk(fileName);
                return new Corpus(
                        this.ParseFile(ms), "||"
                );
        }


}
