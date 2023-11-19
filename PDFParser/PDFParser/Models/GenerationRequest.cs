
namespace PDFParser.Models;

public class Message
{
    public string role { get; set; }
    public string content { get; set; }
}

public class GenerationRequest
{
    public string model { get; set; }
    public List<Message> messages { get; set; }
    public double temperature { get; set; }
}
