using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class StringLiteral : Expression {
    public Token Token { get; set; }
    public string Value { get; set; }
    
    public StringLiteral(Token token, string value) {
        Token = token;
        Value = value;
    }

    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        return $"<String Literal [{Value}]>";
    }

    public string GetRawExpression() {
        return Value;
    }
}