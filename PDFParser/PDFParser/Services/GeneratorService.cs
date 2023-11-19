
using IRLib.IRIndexer;
using IRLib.IRLib.IRIndexer;
using IRLib.IRRetriever;
using IRLib.IRScorer.ProbabilisticModels;
using IRLib.IRCorpus;
using IRLib.IRIndexer.Enums;
using PDFParser.Enums;
using PDFParser.Repos;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PDFParser.Models;
using System.Text;

namespace PDFParser.Services;



public class GeneratorService
{

    public List<Document> indexerResult;
    public GeneratorAction action;
    private int TOKENLIMIT = 2500;
    private string url  = "https://api.openai.com/v1/chat/completions";
    private string API_KEY = "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";
    private string MODEL = "gpt-3.5-turbo";
    //private string MODEL = "gpt-4";
    private string query;
    private GeneratorActionRepo generatorRepo;

    public GeneratorService(List<Document> indexerResult, GeneratorAction action, string query)
    {
        this.action = action;
        this.indexerResult = indexerResult;
        this.query = query;
        this.generatorRepo = new GeneratorActionRepo(action, TrimValues(), query);
    }

    public async Task<GenerationResponse>  MakeRequest()
    {

        string prompt = this.generatorRepo.BuildPrompt();

        // build the body.
        var rPayload = new GenerationRequest()
        {
            model = MODEL,
            messages = new List<Message>()
            {
                new Message()
                {
                    role = "user",
                    content = $"{prompt}"
                }
            },
            temperature = 0.7
        };

        string payload = JsonConvert.SerializeObject(rPayload);


        // Console.WriteLine(payload);

        using(HttpClient client  = new HttpClient())
        {

            // add the payload props here.
            using(HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), this.url))
            {
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + this.API_KEY);

                request.Content = new StringContent(payload);

                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = client.SendAsync(request).Result;

                if( ((int)response.StatusCode) < 200 || ((int)response.StatusCode) > 299 )
                {
                    Console.WriteLine("Response: " + response);
                }


                try {

                    var json = response.Content.ReadAsStringAsync().Result;
                    //dynamic? dynObj = JsonConvert.DeserializeObject(json);

                    dynamic? obj = JsonConvert.DeserializeObject(json, typeof(GenerationResponse));


                    return obj;

                }
                catch (Exception e) {

                    System.Console.WriteLine("Error: " + e);

                }

            }

        }

        return null;

    }

    // ensure that we are grabbing the top results from the list
    // enough to satisfy a local maximum value within the inclusive range of token limits.
    private List<Document> TrimValues()
    {


        int counter = 0;
        int listRange = 0;
        foreach(Document doc in indexerResult)
        {
            // get the text size length
            int textSize = doc.GetDocumentText().Length;


            // pre-condition to check if the token limit is exceeded
            if( (counter + textSize) > TOKENLIMIT)
            {
                // we have our token limit
                break;
            }

            counter += textSize; // increase the text size
            listRange += 1; // increase the list range to split by

        }


        // if not, cut our corpus size
        List<Document> _index = new List<Document>();
        for(int i = 0; i < listRange; i++)
            _index.Add(indexerResult[i]);

        return _index;


    }


}
