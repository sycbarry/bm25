
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using System.Text;

using IRLib.IRIndexer.Interfaces;
using IRLib.IRIndexer.Enums;
using System.Text;

namespace IRLib.IRIndexer;




public class SFIndexer : Indexer, ISFIndexer
{

    public MemoryStream file;
    public FileDestination fileDestination;
    public FileType fileType;
    public string fileName;
    public string folderName;
    public List<string[]> sentenceList;

    public SFIndexer(MemoryStream file, string fileName, FileDestination fileDestination, string folderName, FileType fileType)
    {
        this.file = file;
        this.fileDestination = fileDestination;
        this.fileType = fileType;
        this.folderName = folderName;
        this.fileName = fileName;
        this.sentenceList = new List<string[]>();
    }

    public string GetFolderName()
    {
        return this.folderName;
    }

    public string GetFileDestination()
    {
        switch(this.fileDestination)
        {
            case FileDestination.Local:
                return Path.Combine(Directory.GetCurrentDirectory(), $"{this.folderName}" );
            default:
                return "";
        }
    }

    public bool FolderExists()
    {
        string folderName = this.GetFolderName();
        if(folderName == null || folderName.Trim().Length == 0)
            throw new Exception("Folder name not specified.");
        return System.IO.Directory.Exists(
            Path.Combine(Directory.GetCurrentDirectory(), folderName)
            );
    }

    public void BuildDestinationFolder()
    {
        try {
            switch(this.fileDestination)
            {
                case FileDestination.Local:
                    // check if the folder exists .
                    if(this.FolderExists() == false)
                        System.IO.Directory.CreateDirectory(
                            Path.Combine(Directory.GetCurrentDirectory(), this.GetFolderName())
                        );
                    break;

                // TODO add more here.

                default:
                    break;
            }
        } catch (Exception e)
        {
            throw new Exception("Error during folder creation: " + e.ToString());
        }
    }

    public string GetFileName()
    {
        return this.fileName;
    }


    public double GetFileStreamSize()
    {
        return (double) this.file.GetBuffer().Length / 1000;
    }

    public MemoryStream GetFileStream()
    {
        return this.file;
    }

    public StreamReader GetFileStreamReader()
    {
        return new StreamReader(this.file, Encoding.UTF8, true);
    }


    public void ParsePDFFromMemoryStream(MemoryStream ms)
    {
        List<string[]> sentenceBuffer = new List<string[]>();
        using(PdfDocument d = PdfDocument.Open(ms.GetBuffer()))
        {
            for(int i = 1; i < d.NumberOfPages; i++)
            {
                // get the current page in the doc.
                var page = d.GetPage(i);

                StringBuilder sb = new StringBuilder();
                foreach(Word word in page.GetWords())
                {
                    sb.Append(word.Text.Trim() + " ");
                }
                string text = sb.ToString();

                // parse the sentences.
                string[] sentences = this.ParseSentences(text);
                // index the sentence
                this.IndexSentences(
                    sentences, // the list of sentences
                    i          // the page num
                );

                sentenceBuffer.Add(sentences);
            }
        }
        this.sentenceList = sentenceBuffer;
    }

    public void WriteSentences()
    {
        if(this.sentenceList.Count <= 0)
            throw new Exception("No sentences to write.");

        using(StreamWriter sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), this.folderName, $"{this.fileName}.txt")))
        {
            foreach(var line in this.sentenceList)
            {
                for(int i = 0; i < line.Length; i++)
                {
                    string sentence = line[i];
                    if(sentence == null || sentence.Trim().Length <= 0)
                        continue;
                    sw.WriteLine(sentence);
                }
            }
        }
    }

    public void IndexSentences(string[] sentences, int pageNum)
    {
        for(int i = 0 ; i < sentences.Length; i++)
        {
            if(sentences[i] == null || sentences[i].Trim().Length <= 0)
                continue;
            sentences[i] = $"{pageNum} || {i+1} || {sentences[i]}";
        }
    }

    // returns a list of sentences.
    public string[] ParseSentences(string text)
    {
        string[] buffer = text.Split(". ");
        for(int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = buffer[i].Trim();
            buffer[i] = buffer[i].ToLower();

            string str = buffer[i];

            StringBuilder sb = new StringBuilder();

            foreach (char c in str) {
                if ( /* (c >= '0' && c <= '9') || */ (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || /* c == '.'  || */ c == '_' || c == '-' || c == ' ' )
                {
                    sb.Append(c);
                }
            }

            buffer[i] = sb.ToString();
        }

        return buffer;
    }



    public void ParseMemoryStream()
    {
        switch(this.fileType)
        {
            // get the stream reader and parse the pdf
            case FileType.PDF:
                this.ParsePDFFromMemoryStream(this.file);
                return;

            default:
                return;
        }
    }

    public string Test()
    {
        return "hello world";
    }

}
