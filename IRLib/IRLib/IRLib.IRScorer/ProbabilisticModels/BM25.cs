
using IRLib.IRCorpus;
using IRLib.IRTokenizer;

namespace IRLib.IRScorer.ProbabilisticModels;

public class BM25Ranker
{
    private double k1;
    private double b;

    private double delta;

    public BM25Ranker()
    {
        this.k1 = 2.0; // 1.2;
        this.b = 0.75;
        this.delta = 1.0;
    }

    public BM25Ranker(double k1, double b, double delta)
    {
        if(k1 < 0)
            throw new System.ArgumentException("Negative k1");

        if(b < 0 || b > 1)
            throw new System.ArgumentException($"Invalid b = ${b}");

        if(delta < 0)
            throw new System.ArgumentException($"Invalid delta = ${delta}");

        this.k1 = k1;
        this.b = b;
        this.delta = delta;
    }

    public double Score(int termFreq, int docSize, double avgDocSize, int titleTermFreq,
    int titleSize, double avgTitleSize, int anchorTermFreq, int anchorSize,
    double avgAnchorSize, long N, long n)
    {
        if(termFreq <= 0)
            return 0.0;

        double kf = 4.9;
        double bTitle = 0.6;
        double bBody = 0.5;
        double bAnchor = 0.6;
        double wTitle = 13.5;
        double wBody = 1.0;
        double wAnchor = 11.5;
        double tf = wBody * termFreq / (1.0 + bBody * (docSize / avgDocSize - 1.0 ));

        if(titleTermFreq > 0)
            tf += wTitle * titleTermFreq / (1.0 + bTitle * ( titleSize / avgTitleSize - 1.0 ));

        if(anchorTermFreq > 0)
            tf += wAnchor * anchorTermFreq / (1.0 + bAnchor * (anchorSize / avgAnchorSize - 1.0));

        tf = tf / (kf + tf);
        double idf = Math.Log((N - n + 0.5) / (n + 0.5) + 1);
        return (tf + delta) * idf;

    }

    public double Score(double freq, long N, long n )
    {
        if(freq <= 0) return 0.0;
        double tf = (k1 + 1) * freq / (freq + k1);
        double idf = Math.Log( ( N - n + 0.5 ) / (n + 0.5) + 1);

        return (tf + delta) * idf;
    }

    public double Score(double freq, int docSize, double avgDocSize, long N, long n) {
        if (freq <= 0) return 0.0;
        double tf = freq * (k1 + 1) / (freq + k1 * (1 - b + b * docSize / avgDocSize));
        double idf = Math.Log((N - n + 0.5) / (n + 0.5) + 1);

        return (tf + delta) * idf;
    }

    public double Rank(Corpus corpus, string doc, string term, int tf, int n) {
        if (tf <= 0) return 0.0;
        int N = corpus.NDoc();
        int docSize = doc.Length;
        int avgDocSize = corpus.GetAvgDocSize();

        return Score(tf, docSize, avgDocSize, N, n);
    }

    public double Rank(Corpus corpus, string[] doc, string[] terms, int[] tf, int n) {
        // number of docs in the corpus.
        int N = corpus.NDoc();
        int docSize = doc.Length;
        int avgDocSize = corpus.GetAvgDocSize();
        double r = 0.0;

        // Summation function. score for each token in the query.
        for (int i = 0; i < terms.Length; i++) {
            // doc frequency for term
            int df = corpus.TotalTermFrequency(terms[i]);
            r += Score(tf[i], docSize, avgDocSize, N, df);
        }

        return r;
    }


    #region Core Query Methods
    /*
    string[] docs = a list of documents in the corpus.
    string[] query = a list of tokens from the query.
    int queryTermLength = the length of the query
    */
    public List<Document> GetTopN(Corpus corpus, string query, Document[] docs, int queryTermLength)
    {
        // tokenize the query.
        string[] queryTokens = new Tokenizer(query).Tokenize();

        List<Document> topRecords = new List<Document>();
        // for each doc
        for( int i = 0 ; i < docs.Length ; i++ )
        {
            // calculate the tf for the term in the query for the document and zero out the query.
            // useful to token frequency values and idf scoring.
            int[] tf = new int[queryTokens.Length];
            Array.Clear(tf, 0, tf.Length);

            // get the document text
            string[] docTokens = docs[i].GetTokenizedText();

            for(int j = 0; j < queryTokens.Length; j++)
                tf[j] = corpus.GetDocTokenFrequency(docTokens, queryTokens[j]);

            // for each query element.
            double score = Rank(
                corpus,
                docTokens ,
                queryTokens,
                tf,
                queryTokens.Length
            );

            if(score > 0)
            {
                string rRecord = $"{score} || {docs[i]}";
                Document _document = new Document(
                    docs[i].GetPageNumber(),
                    docs[i].GetLineNumber(),
                    docs[i].GetDocumentText(),
                    score
                    );
                topRecords.Add(_document);
                topRecords.Sort( (x, y) => (
                    (int)y.GetDocumentScore()
                        .CompareTo(
                    (int)x.GetDocumentScore()
                            )
                        ));
            }
        }

        return topRecords;
    }
    #endregion Core Query Regions



}
