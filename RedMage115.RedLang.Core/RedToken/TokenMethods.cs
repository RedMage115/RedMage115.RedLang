namespace RedMage115.RedLang.Core.RedToken;

public partial class Token {

    private static readonly Dictionary<string, TokenType> KeywordsMap = new() {
        {"fn", TokenType.FUNCTION},
        {"let", TokenType.LET},
        {"true", TokenType.TRUE},
        {"false", TokenType.FALSE},
        {"if", TokenType.IF},
        {"else", TokenType.ELSE},
        {"return", TokenType.RETURN},
    };

/// <summary>
/// Checks the keywords dictionary for keywords
/// </summary>
/// <param name="identifier">The identifier to lookup</param>
/// <returns>A keyword token type or IDENT</returns>
    public static TokenType LookupIdentifier(string identifier) {
        return KeywordsMap.GetValueOrDefault(identifier, TokenType.IDENT);
    }
    
    public override string ToString() {
        return $"""[{Type} "{Literal}"]""";
    }
}