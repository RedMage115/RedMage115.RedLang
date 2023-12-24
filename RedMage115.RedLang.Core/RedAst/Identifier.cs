using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class Identifier : Expression {

    public  Token Token { get; set; }
    public string Value { get; set; }

    public Identifier(Token token, string value) {
        Token = token;
        Value = value;
    }

    public string GetTokenLiteral() {
        return Token.Literal;
    }
    
    public string GetNodeTypeString() {
        return $"<Identifier [{Token.Literal} {Value}]>";
    }
}