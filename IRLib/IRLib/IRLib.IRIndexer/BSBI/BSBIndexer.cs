
/*

   This indexer will index all files in a particular folder destination.
   The intent of this class is to build an inverted index, and write the inverted index directly to disk.
   The inverted index contains all of the terms/postings in a single file.

*/


using IRLib.IRIndexer.Interfaces;
using IRLib.IRIndexer.Enums;

using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
using IRLib.IRTokenizer;

namespace IRLib.IRIndexer;


public class BSBIndexer : Indexer, IBSBIndexer
{
    // the source folder we are indexing the files from.
    private string sourceFolder;

    // the target folder name we are going to save the index to.
    private string targetFolder;

    // index
    private Dictionary< string, List<int>> index;


    public BSBIndexer(string sourceFolder, string targetFolder)
    {
        this.sourceFolder = sourceFolder;
        this.targetFolder = targetFolder;
    }


    # region validation methods.
    public void ValidateSourceFolder()
    {
        if(this.sourceFolder  == null || this.targetFolder.Length <= 0)
            throw new Exception("Invalid source folder name.");
        // check if folder exists
        if( ! System.IO.Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), this.sourceFolder)))
            throw new Exception("Source folder does not exist.");
        // check if files are in folder.
        var files = System.IO.Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), this.sourceFolder))
            .Where(x => x.Split(".")[1] != "txt");
        if(files == null || files.Count() <= 0)
            throw new Exception("Source folder does not contain indexable files");
    }

    # endregion validation methods.



    # region indexing methods.

    public void BuildIndex()
    {
        Dictionary<string, List<int>> _index = new Dictionary<string, List<int>>();
        //Dictionary<string, List<string>> *p_index = & _index;

        Console.WriteLine(Path.Combine(Directory.GetCurrentDirectory(),this.sourceFolder));

        // grab all the file names from the source folder.
        string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), this.sourceFolder));
        List<string> fileNames = files
            .ToList()
            .Where( x => x.Split(".")[1] != "txt" )
            .ToList();


        // loop through each of the files.
        for(int i = 0; i < fileNames.Count; i++)
        {

            // get path of the file.
            string path = Path.Combine(Directory.GetCurrentDirectory(), this.targetFolder, fileNames[i]);


            // get the memory stream.
            MemoryStream ms = new MemoryStream();
            using(FileStream fs = new FileStream(path, FileMode.Open))
            {
                fs.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
            }

            // check extensions of file
            string extension = System.IO.Path.GetExtension(path)
                .ToString();


            switch(extension)
            {
                case (".pdf"):
                    List<string> tokens = ParsePDF(ms);
                    AddDocumentToIndex(tokens /* token list */ , i /*the doc num */, _index );
                    break;

                // TODO add more file types here.

                default:
                    break;
            }

        }


        this.index = _index;


    }


    public void AddDocumentToIndex(List<string> tokens, int docNum, Dictionary<string, List<int>> _index)
    {
        // TODO here.
        for(int i = 0; i < tokens.Count; i++)
        {
            // if the token exists in the index.
            if(_index.ContainsKey(tokens[i]))
            {
                _index[tokens[i]].Add(docNum);
            }
            else
            {
                _index[tokens[i]] = new List<int>()
                {
                    docNum
                };

            }
            // sort the posting list
            _index[tokens[i]].Sort((x, y) => x.CompareTo(y));
        }
    }

    # endregion indexing methods.


    # region file parsers

    public List<string> ParsePDF(MemoryStream ms)
    {
        List<string> tokens = new List<string>();

        // open up the pdf
        using(PdfDocument d = PdfDocument.Open(ms.GetBuffer()))
        {
            // read through the pages.
            for(int i = 1; i < d.NumberOfPages; i++)
            {
                var page = d.GetPage(i);

                StringBuilder sb = new StringBuilder();
                foreach(Word word in page.GetWords())
                {
                    sb.Append(word.Text.Trim() + " ");
                }
                string text = sb.ToString();

                string[] pageTokens = this.ParseSentences(text);
                for(int j = 0; j < pageTokens.Length; j++)
                    tokens.Add(pageTokens[j]);
            }
        }
        return tokens;

    }

    // returns a list of tokens per page.
    public string[] ParseSentences(string text)
    {
        string[] buffer = new Tokenizer(text).Tokenize();
        for(int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = buffer[i].Trim();
            buffer[i] = buffer[i].ToLower();
        }
        return buffer;
    }

    # endregion file parsers


    // TODO test this
    public void WriteIndexToFile()
    {
        if(this.index.Count <= 0 || this.index == null)
            throw new Exception("Invalid index length..no index was created when writing");

        if(string.IsNullOrEmpty(this.targetFolder))
            throw new Exception("The target folder was not defined.");

        // check if the target folder exists.
        if(! System.IO.Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), this.targetFolder)))
            System.IO.Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), this.targetFolder));

        // write the index to the file.
        var path = Path.Combine(Directory.GetCurrentDirectory(), this.targetFolder, "bsbi.txt");
        using(StreamWriter fs = new StreamWriter(path))
        {
            // loop through the dictionary
            foreach( var (k, v) in this.index )
            {
                string postings = "";
                for(int i = 0; i < v.Count; i++)
                {
                    postings += $"{v[i]} ";
                }
                string line = $"{k} | {postings} ";

                fs.WriteLine(line);
            }
        }




    }



}
