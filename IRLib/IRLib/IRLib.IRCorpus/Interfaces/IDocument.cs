using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRLib.IRCorpus.Interfaces
{
    public interface IDocument 
    {
        #region Getter Methods for our document. 
        
        // return the page number of the document
        public int GetPageNumber();

        // return the line number of the document. 
        public int GetLineNumber();

        // return the text of the document. 
        public string GetDocumentText();

        // return the score of the document. 
        public double GetDocumentScore();

        // returns the tokenized text
        public string[] GetTokenizedText();

        #endregion 

        #region Setter Methods

        // set the document's score. 
        public void SetDocumentScore(double score);

        // set the document's text. 
        public void SetDocumentText(string text);

        // set the document's line number; 
        public void SetDocumentLineNumber(int lineNumber);

        // set the document's pageNumber 
        public void SetDocumentPageNumber(int pageNumber);

        #endregion


        #region Builder and Invoker Methods for our document. 
        
        // return a tokenized array representing the words in the document.
        public string[] TokenizeDocument();

        #endregion
    }
}