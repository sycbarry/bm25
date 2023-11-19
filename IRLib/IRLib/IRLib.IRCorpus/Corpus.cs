
using IRLib.IRCorpus.Interfaces;
using IRLib.IRTokenizer;

namespace IRLib.IRCorpus;

public class Corpus : ICorpus
{

    // this is the main inverted index data structure.
    private Dictionary<string, List<Document>> index;
    private int pageCount;
    private int docCount;
    private List<Document> documents;

    private List<string> rawData;
    private string delimeter;
    private Dictionary<string, int> tokenIndex;

    public Corpus(List<string> rawData, string delimeter)
    {
        this.rawData = rawData;
        this.delimeter = delimeter;

        // TODO make a corpus builder class.
        this.BuildCorpus();
        this.MakeTokenIndex();

    }

    public Corpus(List<Document> documents)
    {
        documents.Sort(
            (d1, d2)
            => d1.CompareTo(d2));
        this.documents = documents;
        this.SetDocCount(documents.Count);
        this.SetPageCount(documents.First().GetPageNumber());
        this.BuildCorpus();
        this.MakeTokenIndex();
    }

    #region Getter Corpus Methods

    // returns an iterator over the terms in the corpus.
    public IEnumerable<string> Terms()
    {
        return this.index
            .Keys
            .ToArray()
            .AsEnumerable();
    }

    // get corpus size - in page count.
    public int GetCorpusPageCount()
    {
        return this.pageCount;
    }

    // get documents.
    public List<Document> GetDocuments()
    {
        return this.documents;
    }

    // get a specific document at index
    public Document GetDocumentAtIndex(int i)
    {
        return this.documents[i];
    }


    // get a specific document by score
    public Document GetDocumentByScore(long score)
    {
        if(this.documents.Exists(d => d.GetDocumentScore() == score))
            return this.documents.Find(d => d.GetDocumentScore() == score);
        throw new FileNotFoundException("Score not found: " + score.ToString());
    }

    // get a specific document by text
    public Document GetDocumentByText(string text)
    {
        text = text.Trim();
        if(this.documents.Exists(d => d.GetDocumentText() == text))
            return this.documents.Find(d => d.GetDocumentText() == text);
        throw new FileNotFoundException("No text in corpus: " + text);
    }

    //// get a specific document by tokenized text.
    //public Document GetDocumentByTokens(string[] text)
    //{
        //if(this.documents.Exists(d => d.GetTokenizedText() == text))
            //return this.documents.Find(d => d.GetTokenizedText() == text);
        //throw new KeyNotFoundException("Tokenized string not found: " + text.ToString());
    //}

    // get a document by line number
    public Document GetDocumentByLineNumber(int lineNumber)
    {
        if(this.documents.Exists(d => d.GetLineNumber() == lineNumber))
            return this.documents.Find(d => d.GetLineNumber() == lineNumber);
        throw new Exception("Line Number does not exist: " + lineNumber.ToString());
    }

    public List<Document> GetDocumentsByPageNumber(int pageNumber)
    {
        List<Document> d = new List<Document>();
        foreach(var document in this.documents)
        {
            if(document.GetPageNumber() == pageNumber)
                d.Add(document);
        }
        return d;
    }

    public int GetAllTokenCount()
    {
        int length = 0;
        for(int i = 0 ; i < this.documents.Count; i++)
        {
            string[] tokens = this.documents[i].GetTokenizedText();
            length += tokens.Length;
        }
        return length;
    }

    public int NDoc()
    {
        return this.documents.Count;
    }

    public int UTokens()
    {
        // return this.tokenIndex.Count;
        return this.index.Count;
    }

    public int TotalTermFrequency(string term)
    {
        term = term.Trim();
        // get the total times this term
        // shows up in our corpus of documents.
        if(this.index.ContainsKey(term))
            return this.index[term].Count;
        return 0;
    }
    public IEnumerable<string> GetTerms()
    {
        List<string> tokens = this.tokenIndex.Select(kvp => kvp.Key).ToList();
        return tokens;
    }

    public int GetDocTokenFrequency(string[] doc, string term)
    {
        int counter = 0;
        for(int i = 0; i < doc.Length; i++)
        {
            if(doc[i] == term)
                counter += 1;
        }
        return counter;
    }

    public int GetAvgDocSize()
    {
        int size = this.GetAllTokenCount();
        return (int) ( size / this.NDoc() );
    }

    public IEnumerable<string> Search(string term)
    {
        List<string> bmDocuments = new List<string>();
        for(int i = 0; i < this.documents.Count; i++)
        {
            if(this.documents[i].GetDocumentText().Contains(term))
                bmDocuments.Add(this.documents[i].GetDocumentText());
        }
        return bmDocuments.AsEnumerable();
    }


    #endregion



    #region Setter Methods

    public void SetDocuments(List<Document> documents)
    {
        this.documents = documents;
    }

    public void SetPageCount(int pageCount)
    {
        this.pageCount = pageCount;
    }

    public void SetDocCount(int docCount)
    {
        this.docCount = docCount;
    }
    public void SetIndex(Dictionary<string, List<Document>> index)
    {
        this.index = index;
    }

    #endregion



    #region Builder Methods

    public void BuildCorpus()
    {
        // internal variables.
        Dictionary<string, List<Document>> _index = new Dictionary<string, List<Document>>();
        List<Document> _documents = new List<Document>();
        int pc = 0;
        int lc = 0;

        // collect the documents to be indexed (in this case, the this.rawData is what we are iterating over)
        for(int i = 0;  i < this.rawData.Count; i++)
        {
            int pageNum = Int32.Parse(this.rawData[i].Split(this.delimeter)[0]);  // page number
            int lineNum = Int32.Parse(this.rawData[i].Split(this.delimeter)[1]);  // line number
            string text = this.rawData[i].Split(this.delimeter)[2].Trim();               // the text

            // in each document, tokenize each line.
            // NOTE, ideally we would want to do something like this:
            /*
                // linguistic pre-processing for each token TODO use tokenizer class.
                string[] tokens = new Tokenizer(text).Tokenize();
                TODO
            */
            // string[] tokens = text.Split(" ");
            string[] tokens = new Tokenizer(text).Tokenize();


            // make a new document here.
            Document _d = new Document(pageNum, lineNum, text, 0);

            // loop through each token.
            for(int j = 0; j < tokens.Length; j++)
            {

                string token = tokens[j].Trim();

                // check if the index contains the token.
                if(_index.ContainsKey(token))
                {
                    _index[token].Add(
                        _d
                        );
                }
                else
                {
                    _index.Add(token, new List<Document>(){
                        _d
                    });
                }

            }


            // confirm page count and document count.
            if(pageNum > pc)
                pc = pageNum;
            if(lineNum > lc)
                lc = lineNum;


            _documents.Add(_d);

        }

        this.SetIndex(_index);
        this.SetDocuments(_documents);
        this.SetPageCount(pc);
        this.SetDocCount(lc);

    }


    // makes a map of all tokens in the corpus
    // and counts the frequency of each occurence.
    public void MakeTokenIndex()
    {
        Dictionary<string, int> tokenMap = new Dictionary<string, int>();
        // loop through each document in the corpus
        foreach(var document in this.documents)
        {
            // get the token set for the document.
            string[] tokenSet = document.GetTokenizedText();
            for( int i = 0 ; i < tokenSet.Length ; i++ )
            {
                // if the token map contains the token, increment count by 1 and skip.
                if( tokenMap.ContainsKey(tokenSet[i]) )
                {
                    tokenMap[tokenSet[i]] = tokenMap[tokenSet[i]] + 1;
                    continue;
                }
                // otherwise, add the token to the map
                else
                {
                    tokenMap.Add( tokenSet[i], 1 );
                }
            }
            this.tokenIndex = tokenMap;
        }
    }



    #endregion



}
