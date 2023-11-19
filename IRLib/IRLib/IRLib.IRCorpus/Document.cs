using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRLib.IRCorpus.Interfaces;
using IRLib.IRTokenizer;

namespace IRLib.IRCorpus
{
    public class Document : IDocument, IComparable
    {
        private int pageNumber;
        private int lineNumber;
        private string text;
        private double score;
        private string[] tokenizedText = new string[0];

        public Document(int pageNumber, int lineNumber, string text, double score)
        {
            this.pageNumber = pageNumber;
            this.lineNumber = lineNumber;
            this.text = text;
            this.score = score;
        }


        #region Getter Methods for our document.
        public int GetPageNumber()
        {
            return this.pageNumber;
        }
        public int GetLineNumber()
        {
            return this.lineNumber;
        }
        public string GetDocumentText()
        {
            return this.text.Trim();
        }
        public double GetDocumentScore()
        {
            return this.score;
        }
        public string[] GetTokenizedText()
        {
            if(this.tokenizedText == null)
                return this.TokenizeDocument();
            this.tokenizedText = this.TokenizeDocument();
            return this.tokenizedText;
        }
        #endregion



        #region Setter Methods
        public void SetDocumentScore(double score)
        {
            this.score = score;
        }
        public void SetDocumentText(string text)
        {
            this.text = text;
        }
        public void SetDocumentLineNumber(int lineNumber)
        {
            this.lineNumber = lineNumber;
        }
        public void SetDocumentPageNumber(int pageNumber)
        {
            this.pageNumber = pageNumber;
        }

        #endregion


        #region Builder and Invoker Methods for our document.
        public string[] TokenizeDocument()
        {
            return new Tokenizer(
                    this.text
                    ).Tokenize();
            //return this.text.Trim().Split(" ");
        }
        #endregion

        #region Comparable Methods
        public int CompareTo(object obj)
        {
            if(obj == null) return 1;
            Document d = obj as Document;
            if(d != null)
                return this.GetPageNumber()
                           .CompareTo(d.GetPageNumber());
            else
                throw new ArgumentException("Comparable method cannot compare two different object types");
        }

        #endregion

    }
}
