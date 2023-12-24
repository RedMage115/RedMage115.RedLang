namespace RedMage115.RedLang.Core.RedToken;

public partial class Token {
    public TokenType Type { get; set; }
    public string Literal { get; set; }

    public Token(TokenType tokenType, string literal) {
        Type = tokenType;
        Literal = literal;
    }
    
    public Token(TokenType tokenType, char literal) {
        Type = tokenType;
        Literal = literal.ToString();
    }
}