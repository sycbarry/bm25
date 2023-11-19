using System;


namespace IRLib.IRTokenizer;



public interface ITokenizer
{
    public string[] Tokenize();
    public string RemoveSpecialCharacters(string str);
}
