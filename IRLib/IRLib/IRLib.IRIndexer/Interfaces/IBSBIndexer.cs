namespace IRLib.IRIndexer.Interfaces;


public interface IBSBIndexer
{
    public void ValidateSourceFolder();
    public void BuildIndex();
    public void WriteIndexToFile();

    public List<string> ParsePDF(MemoryStream ms);
    public string[] ParseSentences(string text);
    public void AddDocumentToIndex(List<string> tokens, int docNum, Dictionary<string, List<int>> _index);
}
