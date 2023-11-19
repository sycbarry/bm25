using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using IRLib.IRCorpus;

namespace IRLib.IRCorpus.Tests;


public class UnitTestCore
{
    [Fact]
    public void TestDocument()
    {
        Document document = new Document(1, 1, "hello world", 0);
        Assert.Equal(0, document.GetDocumentScore());
        Assert.Equal("hello world", document.GetDocumentText());
        Assert.Equal(new string[]{"hello", "world"}, document.GetTokenizedText());
        Assert.Equal(1, document.GetLineNumber());
        Assert.Equal(1, document.GetPageNumber());
    }

    [Fact]
    public void TestCorpus()
    {
        List<string> list = new List<string>
        {
            "1 || 1 || hello world",
            "1 || 2 || this is awesome"
        };
        Corpus corpus = new Corpus(list, "||");
        corpus.BuildCorpus();
        string text = corpus.GetDocumentByText("hello world").GetDocumentText();
        Assert.Equal("hello world", text);
    }

    [Fact]
    public void TetsCorpus_TermFrequency()
    {
        List<string> list = new List<string>
        {
            "1 || 1 || hello world",
            "1 || 2 || this is awesome"
        };
        Corpus corpus = new Corpus(list, "||");
        int termFrequency = corpus.TotalTermFrequency("hello");
        Assert.Equal(1, termFrequency);

        int termFrequency2 = corpus.TotalTermFrequency("awesome");
        Assert.Equal(termFrequency2, 1);

        int termFrequency3 = corpus.TotalTermFrequency("asdf");
        Assert.Equal(termFrequency3, 0);
    }

    [Fact]
    public void TetsCorpus_UTokens()
    {
        List<string> list = new List<string>
        {
            "1 || 1 || hello world this is me",
            "1 || 2 || this is awesome day",
            "1 || 3 || is this an awesome day or what"
        };
        Corpus corpus = new Corpus(list, "||");
        int tokens = corpus.UTokens();
        //Assert.Equal(tokens, 10);
    }

    [Fact]
    public void TestCorpus_UTokens2()
    {
        List<string> list = new List<string>
        {
            "1 || 1 || hello world this is me",
        };
        Corpus corpus = new Corpus(list, "||");
        int tokens = corpus.UTokens();
        Assert.Equal(tokens, 5);
    }


    [Fact]
    public void TetsCorpus_NDoc()
    {
        List<string> list = new List<string>
        {
            "1 || 1 || hello world this is me",
            "1 || 2 || this is awesome day",
            "1 || 3 || is this an awesome day or what"
        };
        Corpus corpus = new Corpus(list, "||");
        int tokens = corpus.NDoc();
        Assert.Equal(tokens, 3);
    }

    [Fact]
    public void TetsCorpus_AvgDocSize()
    {
        List<string> list = new List<string>
        {
            "1 || 1 || hello world this is me",
            "1 || 2 || this is awesome day",
            "1 || 3 || is this an awesome day or what"
        };
        Corpus corpus = new Corpus(list, "||");
        int size = corpus.GetAvgDocSize();
        Assert.Equal(size, (int)( 16 / 3 ) );
    }

    [Fact]
    public void TetsCorpus_GetTerms()
    {
        List<string> list = new List<string>
        {
            "1 || 1 || hello world this is me",
            "1 || 2 || this is awesome day",
            "1 || 3 || is this an awesome day or what"
        };
        Corpus corpus = new Corpus(list, "||");
        List<string> eList = (List<string>)corpus.GetTerms();
    }

}
