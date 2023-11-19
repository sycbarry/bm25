namespace PDFParser.Tests;


public class UnitTest1
{
    /*
    List<string> _corpus = new List<string>
    {
        "this is a document if it is a value. it is also a value if it is cool.", 
        "this is also a document", 
        "the snow is white today", 
        "abraham Lincoln is awesome"
    };

    public void CorpusSize()
    {
        int size = _corpus.Count;
        Corpus ocorpus = new Corpus(_corpus);
        int testedSize = ocorpus.ndoc();
        Assert.Equal(size, testedSize);
    }

    public void TokenSize()
    {
        int size = 0; 
        for(int i = 0; i < _corpus.Count; i++)
        {
            string[] doc = _corpus[i].Split(" ");
            size += doc.Length;
        }
        Corpus ocorpus = new Corpus(_corpus);
        int calSize = (int)ocorpus.size();
        Assert.Equal(size, calSize);
    }

    public void Count()
    {
        Corpus corpus = new Corpus(_corpus);

        string term = "snow";
        int nterm = corpus.count(term);
        Assert.Equal(1, nterm);

        string term2 = "this";
        int nterm2 = corpus.count(term2);
        Assert.Equal(2, nterm2);
    }

    public void NTerm()
    {
        Corpus corpus = new Corpus(_corpus);
        int nterm = corpus.nterm();
        // Assert.Equal(15, nterm);
    }

    public void ScoreA()
    {
        BM25Ranker ranker = new BM25Ranker();
        Corpus corpus = new Corpus(_corpus);

        double freq = corpus.count("Lincoln is in the snow and he is cold. He is cool, because he is a Lincoln man.");
        int docSize = corpus.ndoc();
        double avgDocSize = corpus.ndoc();
        long N = corpus.ndoc();
        long n = 1;

        double score = ranker.Score(
            freq, 
            N, 
            n
        );

    }

    public void RankA()
    {
        BM25Ranker ranker = new BM25Ranker();
        Corpus corpus = new Corpus(_corpus);

        string query = "Here is the value of is. Lincoln is in the snow and he is cold. He is cool, because he is a Lincoln man.";
        string[] tokens = query.Split(" ");
        string[] docs = _corpus.ToArray();
        int queryTermLength = query.Split(" ").Length;

        List<string> records = ranker.GetTopN(corpus, tokens, docs, queryTermLength);

        foreach(var i in records) 
        {
            Console.WriteLine(i);
        }


    }
    */

}