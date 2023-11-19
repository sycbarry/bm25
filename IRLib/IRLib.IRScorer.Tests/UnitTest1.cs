using System;
using IRLib.IRCorpus;
using IRLib.IRRetriever;
using IRLib.IRIndexer;
using IRLib.IRIndexer.Enums;
using IRLib.IRScorer.ProbabilisticModels;

namespace IRLib.IRScorer.Tests;

public class UnitTest1
{
    private string fileName = "Maximo_Business_Value.pdf";

    [Fact]
    public void Test1()
    {
        // build the memory stream.
        MemoryStream ms = new MemoryStream();
        var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        using(FileStream fs = new FileStream(path, FileMode.Open))
        {
            fs.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
        }

        // build the indexer
        SFIndexer indexer = new SFIndexerBuilder(ms, fileName)
            .AddFileType(FileType.PDF)
            .AddFileDestination(FileDestination.Local, "Documents")
            .Build();

        // fire the invoker ( to index the file )
        IndexerInvoker invoker = new IndexerInvoker(indexer);
        invoker.Fire();


        // use a retreiver to build the corpus into memory.
        Retriever r = new Retriever();
        Corpus _corpus = r.RetrieveFile(indexer.GetFolderName() + "/" + fileName + ".txt");

        // build the query.
        string query = "ibm maximo and artificial intelligence";

        BM25Ranker ranker = new BM25Ranker();

        Document[] docs = _corpus.GetDocuments().ToArray();
        int queryTermLength = query.Split(" ").Length;

        List<Document> records = ranker.GetTopN(_corpus, query, docs, queryTermLength);

        Console.WriteLine(records[0].GetDocumentScore() + ": " + records[0].GetDocumentText());

    }
}
