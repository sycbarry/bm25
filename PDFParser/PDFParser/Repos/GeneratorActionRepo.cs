
/*

this repo stores prompts to pass on to the generator service.
*/


using PDFParser.Enums;

using IRLib.IRIndexer;
using IRLib.IRLib.IRIndexer;
using IRLib.IRRetriever;
using IRLib.IRScorer.ProbabilisticModels;
using IRLib.IRCorpus;
using IRLib.IRIndexer.Enums;
using PDFParser.Enums;
using System.Text;


namespace PDFParser.Repos;


public class GeneratorActionRepo
{
    private GeneratorAction action;
    private List<Document> documents;
    private string query;
    public GeneratorActionRepo(GeneratorAction action, List<Document> documents, string query)
    {
        this.action = action;
        this.query = query;
        this.documents = documents;
    }

    public string BuildPrompt()
    {
        switch(this.action)
        {
            case GeneratorAction.SIMPLIFY:
                return BuildSimplifyPrompt();
            case GeneratorAction.EXPLAIN:
                return BuildExplainPrompt();
            default:
                return "";
                // do something here.
        }
    }

    private string BuildSimplifyPrompt()
    {
        StringBuilder sb = new StringBuilder();
        foreach(Document doc in this.documents)
            sb.Append(doc.GetDocumentText().Trim() + $" [ Page Number: {doc.GetPageNumber()} ] . ");
        string prompt = $@"

            Here is what the user has requested: {this.query}

            Here is some information about the request:
            {sb.ToString()}

            Return a simplified version of the above information to the user. Do not explain it. Just simplify it.

            Ensure to include the page numbers that where included in the reponse as a citation to your response.

            Structure the visualization response this way:

            1. include code samples in <code></code> blocks.
            2. ensure to add some line breaks  to make the response more legible.
            3. add html tags to emphasize important points where needed.
            4. include <br> tags where necessary.


            ";

        return prompt;
    }


    private string BuildExplainPrompt()
    {
        StringBuilder sb = new StringBuilder();
        foreach(Document doc in this.documents)
            sb.Append(doc.GetDocumentText().Trim() + $" [ Page Number: {doc.GetPageNumber()} ] . ");
        string prompt = $@"

            Here is what the user has requested: {this.query}

            Here is some information about the request:
            {sb.ToString()}

            Explain the above information, in accordance to the user's question.

            Ensure to include the page numbers that where included in the reponse as a citation to your response.

            Structure the visualization response this way:

            1. include code samples in <code></code> blocks.
            2. ensure to add some line breaks  to make the response more legible.
            3. add html tags to emphasize important points where needed.
            4. include <br> tags where necessary.

            ";

        return prompt;

    }


}
