
using System.Text;

using IRLib.IRIndexer.Enums; 

namespace IRLib.IRIndexer.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("hello world" ?? ""));

        SFIndexer indexer = new SFIndexerBuilder(stream, "filename")
            .AddFileType(FileType.PDF)
            .AddFileDestination(FileDestination.Local, "Documents")
            .Build();

        Assert.Equal(indexer.Test(), "hello world");
    }

    [Fact]
    public void GetFolderName()
    {
        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("hello world" ?? ""));
        SFIndexer indexer = new SFIndexerBuilder(stream, "filename")
            .AddFileType(FileType.PDF)
            .AddFileDestination(FileDestination.Local, "Documents")
            .Build();

        string folderName = indexer.GetFolderName();
        Assert.Equal(folderName, "Documents");
    }

    [Fact]
    public void GetFileDestination() 
    {
        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("hello world" ?? ""));
        SFIndexer indexer = new SFIndexerBuilder(stream, "filename")
            .AddFileType(FileType.PDF)
            .AddFileDestination(FileDestination.Local, "Test.Documentation")
            .Build();

        string fileDestination = indexer.GetFileDestination();
        Assert.Equal(Path.Combine(Directory.GetCurrentDirectory(), $"{indexer.GetFolderName()}"), fileDestination);
    }

    [Fact]
    public void BuildFileDestination() 
    {
        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("hello world"));

        SFIndexer indexer = new SFIndexerBuilder(stream, "filename")
            .AddFileType(FileType.PDF)
            .AddFileDestination(FileDestination.Local, "ABC")
            .Build();

        try {
            indexer.BuildDestinationFolder();
            bool folderCreated = indexer.FolderExists();
            if(folderCreated == false)
                Assert.False(true, "folder was not created successfully.");
        } catch (Exception e)
        {
            Assert.False(true, $"{e}");
        }
        
    }

    [Fact]
    public void GetFileSizeTest()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "IBM.pdf");
        MemoryStream ms = new MemoryStream();
        using(FileStream stream = new FileStream(path, FileMode.Open))
        {
            stream.CopyTo(ms);
        }

        SFIndexer indexer = new SFIndexerBuilder(ms, "IBM.pdf")
            .AddFileType(FileType.PDF)
            .AddFileDestination(FileDestination.Local, "ABC")
            .Build();

        // indexer.BuildDestinationFolder();
        double fileSize = indexer.GetFileStreamSize();
        double size = (double) (ms.GetBuffer().Length) / 1000;

        Assert.Equal(size, fileSize);
    }

    [Fact]
    public void GetStreamReader()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "IBM.pdf");
        MemoryStream ms = new MemoryStream();
        using(FileStream fs = new FileStream(path, FileMode.Open))
        {
            fs.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin); // important to rewind the stream.
        }
        
        SFIndexer indexer = new SFIndexerBuilder(ms, "IBM.pdf")
            .AddFileType(FileType.PDF)
            .AddFileDestination(FileDestination.Local, "ABC")
            .Build();

        StreamReader sr = indexer.GetFileStreamReader();
        Assert.True(true, "true");
    }

    [Fact]
    public void ParsePDFMemoryStream()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "IBM.pdf");
        MemoryStream ms = new MemoryStream();
        using(FileStream fs = new FileStream(path, FileMode.Open))
        {
            fs.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
        }

        SFIndexer indexer = new SFIndexerBuilder(ms, "IBM.pdf")
            .AddFileType(FileType.PDF)
            .AddFileDestination(FileDestination.Local, "ABC")
            .Build();

        indexer.ParseMemoryStream();

    }

    // [Fact]
    public void WriteSentences()
    {
        MemoryStream ms = new MemoryStream();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "IBM.pdf");
        using(FileStream fs = new FileStream(path, FileMode.Open))
        {
            fs.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
        }
        SFIndexer indexer = new SFIndexerBuilder(ms, "IBM.pdf")
            .AddFileType(FileType.PDF)
            .AddFileDestination(FileDestination.Local, "ABC")
            .Build();

        indexer.ParseMemoryStream();
        indexer.WriteSentences();

        Console.WriteLine("Filename: " + indexer.GetFileName());

        // check if the file was written to disk.
        if(System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "ABC", "IBM.pdf.txt")) == false)
            Assert.False(true, "File was not written correctly.");
        
        Assert.True(true, "File created.");
    }

    [Fact]
    public void WriteSentencesUsingIterator()
    {
        MemoryStream ms = new MemoryStream();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "IBM.pdf");
        using(FileStream fs = new FileStream(path, FileMode.Open))
        {
            fs.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
        }
        SFIndexer indexer = new SFIndexerBuilder(ms, "IBM.pdf")
            .AddFileType(FileType.PDF)
            .AddFileDestination(FileDestination.Local, "ABC")
            .Build();


        IndexerInvoker invoker = new IndexerInvoker(indexer);
        invoker.Fire();


        // check if the file was written to disk.
        if(System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "ABC", "IBM.pdf.txt")) == false)
           Assert.False(true, "File was not written correctly.");
        
        Assert.True(true, "File created.");
    }

}