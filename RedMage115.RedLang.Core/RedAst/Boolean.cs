using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class Boolean : Expression {
    public Token Token { get; set; }
    public bool Value { get; set; }

    public Boolean(Token token, bool value) {
        Token = token;
        Value = value;
    }

    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        return $"<Boolean Expression [{Value}]>";
    }

    public string GetRawExpression() {
        return Value.ToString();
    }
}