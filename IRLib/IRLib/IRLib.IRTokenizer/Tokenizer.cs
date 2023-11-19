using System.Text;
using System.Text.RegularExpressions;
using System;

namespace IRLib.IRTokenizer;


public class Tokenizer  : ITokenizer
{

    private string input;

    public Tokenizer(string input)
    {
        this.input = input;
    }

    // tokenize the input into a list of tokens.
    public string[] Tokenize()
    {
        if(input == null || input.Length <= 0)
            return new string[]{""};

        input = RemoveSpecialCharacters(input);

        return input
            .Trim()
            .ToLower()
            .Split(" ");


    }

    public string RemoveSpecialCharacters(string str) {
        StringBuilder sb = new StringBuilder();
        foreach (char c in str) {
            if ( /* (c >= '0' && c <= '9') || */ (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || /* c == '.'  || */ c == '_' || c == '-' || c == ' ' )
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

}
