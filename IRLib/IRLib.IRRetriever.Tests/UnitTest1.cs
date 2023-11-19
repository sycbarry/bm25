
using System.Data;
using IRLib.IRCorpus;
using IRLib.IRRetriever;

namespace IRLib.IRRetriever.Tests;

public class UnitTest1
{
    [Fact]
    public void TokenCount()
    {
        Retriever r = new Retriever();
        Corpus c = r.RetrieveFile("IBM.pdf.txt");
        Assert.Equal(859, c.GetAllTokenCount());
    }

    [Fact]
    public void TokenFrequency()
    {
        Retriever r = new Retriever();
        Corpus c = r.RetrieveFile("IBM.pdf.txt");
        //Console.WriteLine(c.GetDocumentByTokens(new string[]{"ibm"}));
        // Assert.Equal(859, c.GetAllTokenCount());
    }
}