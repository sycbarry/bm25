using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRLib.IRCorpus.Interfaces
{
    public interface ICorpus
    {

        #region Getter Corpus Methods

        // get corpus size - in page count. 
        public int GetCorpusPageCount();

        // returns an iterator over the terms in the corpus.
        public IEnumerable<string> Terms();


        // get documents. 
        public List<Document> GetDocuments();

        // get a specific document at index 
        public Document GetDocumentAtIndex(int i);


        // get a specific document by score 
        public Document GetDocumentByScore(long score);

        // get a specific document by text 
        public Document GetDocumentByText(string text);

        // get a specific document by tokenized text. 
        // public Document GetDocumentByTokens(string[] text);

        // get a document by line number 
        public Document GetDocumentByLineNumber(int lineNumber);

        // get a list of documents by page number
        public List<Document> GetDocumentsByPageNumber(int pageNumber);

        // returns the number of words in the corpus. 
        public int GetAllTokenCount(); 

        // returns the number of documents in the corpus. 
        public int NDoc();

        // returns the number of unique termsin the corpus. 
        public int UTokens();

        //returns the total frequency of the term in the corpus. 
        public int TotalTermFrequency(string term); 

        // returns hte iterator over the termsin the corpus. 
        public IEnumerable<string> GetTerms();

        // returns the frequency of a term (word) in a particular document.
        public int GetDocTokenFrequency(string[] doc, string term);

        // return the average document size in the corpus.
        public int GetAvgDocSize();

        // returns the iterator over the set of documents containing the given term.
        public IEnumerable<string> Search(string term);


        #endregion


        #region Setter Methods

        public void SetPageCount(int pageCount);

        public void SetDocCount(int docCount);

        public void SetDocuments(List<Document> documents);

        public void SetIndex(Dictionary<string, List<Document>> index); 

        #endregion


        #region Builder Methods

        // build the corpus. 
        public void BuildCorpus();

        // build a token index containing a unique token and its 
        // frequency in the corpus.
        public void MakeTokenIndex();


        #endregion








        // TODO finish here.




    }
}